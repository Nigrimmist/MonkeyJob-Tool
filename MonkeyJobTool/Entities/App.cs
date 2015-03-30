using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MonkeyJobTool.Entities
{
    /// <summary>
    /// Main Application class for communication between forms including business logic. GOD CLASS! HAHHAHA!
    /// </summary>
    public class App
    {
        private static object _appInstanceLock = new object();
        private static App _instance;
        private ApplicationConfiguration _appConf;
        
        private KeyboardHook _hook = new KeyboardHook();
        private object _hookLock = new object();
        private List<InfoPopup> _openedPopups = new List<InfoPopup>();
        private object _openedPopupsLock = new object();
        private MainForm _mainForm;

        public delegate void OnSettingsChangedDelegate();
        public event OnSettingsChangedDelegate OnSettingsChanged;

        private string _executionFolder;
        private string _executionPath;

        public string ExecutionFolder {get { return _executionFolder; }}
        public string ExecutionPath { get { return _executionPath; } }

        /// <summary>
        /// Collection of event delegates for hotkeys. one delegate for one hotkeytype
        /// </summary>
        private readonly Dictionary<HotKeyType, EventHandler<KeyPressedEventArgs>> _hotKeysHadlers = new Dictionary<HotKeyType, EventHandler<KeyPressedEventArgs>>();

        
        public static App Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_appInstanceLock)
                    {
                        if (_instance==null)
                        {
                            _instance = new App();
                        }
                    }
                }
                return _instance;
            }
            
        }

        public void Init(EventHandler<KeyPressedEventArgs> mainFormOpenHotKeyRaisedHandler, MainForm mainForm)
        {
            //read and load config
            if (!File.Exists(_executionFolder + AppConstants.Paths.MainConfFileName)) throw new Exception("Config missing");
            var json = File.ReadAllText(_executionFolder + AppConstants.Paths.MainConfFileName);
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = AppConstants.DateTimeFormat };
            _appConf = JsonConvert.DeserializeObject<ApplicationConfiguration>(json, dateTimeConverter);
            _mainForm = mainForm;

            //register open program hotkey using incoming (main form) delegate
            _hotKeysHadlers.Add(HotKeyType.OpenProgram, mainFormOpenHotKeyRaisedHandler);
            ReInitHotKeys();
            
        }
        

        public void ReInitHotKeys()
        {
            var openHotKeys = Instance.AppConf.HotKeys.ProgramOpen;
            RegisterHotKey(KeyboardHook.ToSpecialKeys(openHotKeys), KeyboardHook.ToOrdinalKeys(openHotKeys), HotKeyType.OpenProgram);
        }

        public App()
        {
            _executionFolder = Application.StartupPath+@"\";
            _executionPath = Application.ExecutablePath;
        }

        private void RegisterHotKey(ModifierHookKeys modifierHook, Keys key, HotKeyType hkType)
        {
            UnRegisterHotKey(hkType);
            lock (_hookLock)
            {
                _hook.KeyPressed += _hotKeysHadlers[hkType];
                _hook.RegisterHotKey(modifierHook, key, (int) HotKeyType.OpenProgram);
            }
        }

        private void UnRegisterHotKey(HotKeyType hkType)
        {
            lock (_hookLock)
            {
                _hook.KeyPressed -= _hotKeysHadlers[hkType];
                _hook.UnregisterHotKey((int) hkType);
            }
        }

        public ApplicationConfiguration AppConf
        {
            get { return _appConf; }
        }

        public void ShowFixedPopup(string message, TimeSpan? timeToShow)
        {
            ShowPopup(string.Empty, message, timeToShow);
        }

        public void ShowPopup(string message, TimeSpan? timeToShow)
        {
            ShowPopup(string.Empty, message, timeToShow,isFixed:false);
        }

        public void ShowPopup(string title, string text, TimeSpan? displayTime, Guid? commandToken=null, bool isFixed = true)
        {
            
            InfoPopup popup = new InfoPopup(isFixed,title, text, displayTime,commandToken);
            popup.Width = _mainForm.Width;
            
            
            var totalPopupY = _openedPopups.Sum(x => x.Height);
            popup.ToTop();
            popup.Location = new Point(_mainForm.Location.X, _mainForm.Location.Y - popup.Height - totalPopupY);
            popup.FormClosed += popup_FormClosed;
            popup.OnPopupClosed += popup_OnPopupClosed;
            
            lock (_openedPopups)
            {
                if (isFixed)
                {
                    //close previous fixed popup
                    CloseFixedPopup();
                }
                _openedPopups.Add(popup);
            }

            ReorderPopupsPositions();
        }

        

        public void CloseFixedPopup()
        {
            lock (_openedPopups)
            {
                var fixedPopup = _openedPopups.SingleOrDefault(x => x.IsFixed);
                if (fixedPopup != null)
                {
                    fixedPopup.Close();
                }
            }
        }

        public void ReorderPopupsPositions()
        {
            lock (_openedPopups)
            {
                int xPos = _mainForm.Location.X;
                int yPos = Screen.PrimaryScreen.WorkingArea.Bottom;
                if (_mainForm.Visible)
                {
                    yPos -= _mainForm.Height;
                }

                var fixedPopup = _openedPopups.SingleOrDefault(x => x.IsFixed);
                if (fixedPopup != null)
                {
                    yPos -= fixedPopup.Height;
                    fixedPopup.Location = new Point(xPos, yPos);
                }

                foreach (var popup in _openedPopups.Where(x => !x.IsFixed))
                {
                    yPos -= popup.Height;
                    popup.Location = new Point(xPos, yPos);
                }
            }
        }

        void popup_OnPopupClosed(ClosePopupReasonType reason, object sessionData)
        {
            if(sessionData!=null)
                _mainForm.HandleCommandInfoPopupClose((Guid?)sessionData,reason);
        }

        void popup_FormClosed(object sender, FormClosedEventArgs e)
        {
            lock (_openedPopupsLock)
            {
                _openedPopups.Remove((InfoPopup) sender);
            }
            ReorderPopupsPositions();
        }

        public void CloseAllPopups()
        {
            lock (_openedPopupsLock)
            {
                while (_openedPopups.Any())
                {
                    _openedPopups.First().Close();
                }
            }
        }

        public void AllPopupsToTop()
        {
            lock (_openedPopupsLock)
            {
                if (_openedPopups.Any())
                {
                    _openedPopups.ForEach(p => p.ToTop());
                }
            }
        }


        public void NotifyAboutSettingsChanged()
        {
            if (OnSettingsChanged != null)
            {
                OnSettingsChanged();
            }
        }

        /// <summary>Returns true if the current application has focus, false otherwise</summary>
        public static bool ApplicationIsActivated()
        {
            
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }

        

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
    }
}
