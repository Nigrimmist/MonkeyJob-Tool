using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private MainForm _mainForm;
        
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

        public void ShowPopup(string message, TimeSpan? timeToShow)
        {
            ShowPopup(string.Empty, message, timeToShow);
        }

        public void ShowPopup(string title, string text, TimeSpan? displayTime, Guid? commandToken=null)
        {
            CloseAllPopups();
            InfoPopup popup = new InfoPopup(title, text, displayTime,commandToken);
            popup.Width = _mainForm.Width;
            popup.ToTop();
            var totalPopupY = _openedPopups.Sum(x => x.Height);
            popup.Location = new Point(_mainForm.Location.X, _mainForm.Location.Y - popup.Height - totalPopupY);
            popup.FormClosed += popup_FormClosed;
            popup.OnPopupClosed += popup_OnPopupClosed;
            _openedPopups.Add(popup);
        }

        void popup_OnPopupClosed(ClosePopupReasonType reason, object sessionData)
        {
            if(sessionData!=null)
                _mainForm.HandleCommandInfoPopupClose((Guid?)sessionData,reason);
        }

        void popup_FormClosed(object sender, FormClosedEventArgs e)
        {
            _openedPopups.Remove((InfoPopup)sender);
        }

        public void CloseAllPopups()
        {
            while (_openedPopups.Any())
            {
                _openedPopups.First().Close();
            }
        }

        public void AllPopupsToTop()
        {
            if (_openedPopups.Any())
            {
                _openedPopups.ForEach(p => p.ToTop());
            }
        }
    }
}
