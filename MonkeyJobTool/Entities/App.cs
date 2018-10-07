using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HelloBotCore;
using Microsoft.Win32;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Forms;
using MonkeyJobTool.Managers;
using MonkeyJobTool.Managers.Interfaces;
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
        private int _notificationCount;
        public int NotificationCount
        {
            get { return _notificationCount; }
            set
            {
                _notificationCount = value;
                if (OnNotificationCountChanged != null)
                    OnNotificationCountChanged(_notificationCount);
            }
        }

    
        private object _notificationLock = new object();

        public delegate void OnSettingsChangedDelegate();
        public event OnSettingsChangedDelegate OnSettingsChanged;

        public delegate void OnNotificationCountChangedDelegate(int notificationCount);
        public event OnNotificationCountChangedDelegate OnNotificationCountChanged;

        private readonly string _executionFolder;
        private readonly string _executionPath;
        private readonly IsoDateTimeConverter _defaultDateTimeConverter;
        public IsoDateTimeConverter DefaultDateTimeConverter => _defaultDateTimeConverter;
        readonly int _popupMarginTop = 5;

        public string ExecutionFolder {get { return _executionFolder; }}
        public string FolderSettingPath {get { return _executionFolder + "ModuleSettings"; }}
        public string FolderLogPath { get { return _executionFolder + "ModuleLogs/Trace"; } }
        public string ExecutionPath { get { return _executionPath; } }
        public Guid? UserID = null;
        

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
        
        private static void OnError(object sender, ThreadExceptionEventArgs t)
        {
            LogManager.Error(t.Exception);
        }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogManager.Error(e.ExceptionObject as Exception);
        }

        public void Init(EventHandler<KeyPressedEventArgs> mainFormOpenHotKeyRaisedHandler, MainForm mainForm, IStorageManager storageManager)
        {
            Application.ThreadException += (OnError);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //read and load config
            if (!storageManager.Exist(AppConstants.Paths.MainConfFileName))
            {
                _appConf = new ApplicationConfiguration(storageManager,true);
                _appConf.Save();
            }
            
            _appConf = storageManager.Get<ApplicationConfiguration>(AppConstants.Paths.MainConfFileName);
            _appConf.StorageManager = storageManager;
            LogManager.Trace("App.Conf retrieved");
            _mainForm = mainForm;

            //register open program hotkey using incoming (main form) delegate
            _hotKeysHadlers.Add(HotKeyType.OpenProgram, mainFormOpenHotKeyRaisedHandler);
            ReInitHotKeys();
            ApplicationMigrations.UpdateAppMigrations();
            LogManager.Trace("End App.Init()");

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
            _defaultDateTimeConverter =  new IsoDateTimeConverter { DateTimeFormat = AppConstants.DateTimeFormat };
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

        public HelloBot Bot { get; set; }

        public void ShowNotification(string title, string text, Guid? commandToken, Image icon = null, Color? titleBackgroundColor = null, Color? bodyBackgroundColor = null)
        {
            ShowPopup(title, text, PopupType.Notification, null, commandToken, icon, titleBackgroundColor: titleBackgroundColor, bodyBackgroundColor: bodyBackgroundColor);
        }

        public void ShowFixedPopup(string title, string text, Guid? commandToken, Image icon = null, Color? titleBackgroundColor = null, Color? bodyBackgroundColor = null)
        {
            ShowPopup(title, text, PopupType.Fixed, commandToken: commandToken, icon: icon, titleBackgroundColor: titleBackgroundColor, bodyBackgroundColor: bodyBackgroundColor);
        }

        public void ShowInternalPopup(string title,string message, TimeSpan? timeToShow)
        {
            ShowPopup(title, message, PopupType.InternalMessage , timeToShow);
        }

        private void ShowPopup(string title, string text, PopupType popupType, TimeSpan? displayTime = null, Guid? commandToken = null, Image icon = null, Color? titleBackgroundColor = null, Color? bodyBackgroundColor = null)
        {
            LogManager.Trace("Start ShowPopup()");

            if (popupType == PopupType.Notification)
            {
                displayTime = TimeSpan.FromSeconds(10);
            }
            if (AppConf.SystemData.DoNotNotify && popupType == PopupType.Notification)
            {
                //null bcse it will be hided and showing only by user request
                displayTime = null;
            }
            InfoPopup popup = new InfoPopup(popupType, title, text, displayTime, commandToken,icon,titleBackgroundColor,bodyBackgroundColor);
            //popup.Width = _mainForm.Width;
            var totalPopupY = _openedPopups.Sum(x => x.Height);
            
            if (!AppConf.SystemData.DoNotNotify || popupType != PopupType.Notification)
            {
                
                var mainWindowActivated = App.ApplicationIsActivated();
                Debug.WriteLine("popup going to top. main window : "+mainWindowActivated);
                popup.ToTop();
                if(mainWindowActivated) //focus for main form was stealed after popup.toTop, restoring
                    _mainForm.ToTop(true);
            }
            popup.Location = new Point(_mainForm.Location.X + (_mainForm.Width - popup.Width), _mainForm.Location.Y - popup.Height - totalPopupY);
            
            popup.FormClosed += popup_FormClosed;
            popup.OnPopupClosedBy += PopupOnPopupClosedBy;
            popup.OnPopupHided += popup_OnPopupHided;
            lock (_openedPopups)
            {
                if (popupType==PopupType.Fixed)
                {
                    //close previous fixed popup
                    CloseFixedPopup();
                }
                _openedPopups.Add(popup);
            }
            if (popupType==PopupType.Notification)
            {
                lock (_notificationLock)
                {
                    ++NotificationCount;
                }
            }
            ReorderPopupsPositions();
            LogManager.Trace("End ShowPopup()");

        }

        void popup_OnPopupHided()
        {
            ReorderPopupsPositions();
        }

        public void CloseFixedPopup()
        {
            lock (_openedPopups)
            {
                var fixedPopup = _openedPopups.SingleOrDefault(x => x.PopupType==PopupType.Fixed);
                if (fixedPopup != null)
                {
                    fixedPopup.Close();
                }
            }
        }

        public InfoPopup GetOpenedFixedPopup()
        {
            lock (_openedPopups)
            {
                return _openedPopups.SingleOrDefault(x => x.PopupType == PopupType.Fixed);
            }
        }
        
        public void ReorderPopupsPositions()
        {
            LogManager.Trace("Start ReorderPopupsPositions");
            
            lock (_openedPopups)
            {
                //int xPos = _mainForm.Location.X;
                int yPos = Screen.PrimaryScreen.WorkingArea.Bottom;
                if (_mainForm.Visible)
                {
                    yPos -= _mainForm.Height;
                }

                var fixedPopup = _openedPopups.SingleOrDefault(x => x.PopupType == PopupType.Fixed);
                if (fixedPopup != null && fixedPopup.Visible)
                {
                    yPos -= fixedPopup.Height; //+ _popupMarginTop;
                    fixedPopup.Location = new Point(fixedPopup.Location.X, yPos);
                }
                if (!_mainForm.Visible)
                {
                    yPos += _popupMarginTop; //remove margin for first popup
                }
                foreach (var popup in _openedPopups.Where(x => x.PopupType != PopupType.Fixed && x.Visible))
                {
                    yPos -= popup.Height + _popupMarginTop;
                    popup.Location = new Point(popup.Location.X, yPos);
                }

            }
            LogManager.Trace("End ReorderPopupsPositions");
        }

        void PopupOnPopupClosedBy(ClosePopupReasonType reason, object sessionData)
        {
            if(sessionData!=null)
                _mainForm.HandleCommandInfoPopupClose((Guid?)sessionData,reason);
        }

        void popup_FormClosed(object sender, FormClosedEventArgs e)
        {
            Debug.WriteLine("popup_FormClosed");
            var popup = (InfoPopup) sender;
            lock (_openedPopupsLock)
            {
                _openedPopups.Remove((InfoPopup) sender);
            }
            if (popup.PopupType == PopupType.Notification)
            {
                lock (_notificationLock)
                {
                    --NotificationCount;
                }
            }
            ReorderPopupsPositions();
        }
        

        public void AllPopupsToTop()
        {
            LogManager.Trace("Start AllPopupsToTop");
            lock (_openedPopupsLock)
            {
                if (_openedPopups.Any())
                {
                    _openedPopups.ForEach(p => p.ToTop());
                }
            }
            LogManager.Trace("End AllPopupsToTop");
        }

        public void HideAllPopupsAvailableForHiding()
        {
            lock (_openedPopupsLock)
            {
                if (_openedPopups.Any())
                {
                    _openedPopups.Where(x=>x.AlreadyNotified).ToList().ForEach(p => p.Hide());
                }
            }
        }

        public void SetAllPopupsAsNotified()
        {
            lock (_openedPopupsLock)
            {
                if (_openedPopups.Any())
                {
                    _openedPopups.ForEach(p => p.AlreadyNotified=true);
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
            LogManager.Trace("Start ApplicationIsActivated()");
            
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                Debug.WriteLine("ApplicationIsActivated : activatedHandle == IntPtr.Zero => return false;");
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            LogManager.Trace("End ApplicationIsActivated()");
            return activeProcId == procId;
        }

        

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        public void CheckAllNotificationAsRead()
        {
            LogManager.Trace("Start CheckAllNotificationAsRead()");
                lock (_openedPopupsLock)
                {
                   var toClose = _openedPopups.Where(x => x.PopupType == PopupType.Notification).ToList();
                   toClose.ForEach(x => x.Close());
                }
            LogManager.Trace("End CheckAllNotificationAsRead()");
        }

        public bool NotificationPopupExist(string text, string title)
        {
            lock (_openedPopupsLock)
            {
                return _openedPopups.Where(x => x.PopupType == PopupType.Notification).Any(x => x.Text == text);
            }
        }

        public bool AnyPopupExist()
        {
            lock (_openedPopupsLock)
            {
                return _openedPopups.Any();
            }
        }

        public void DisableModule(string moduleSystemName)
        {
            Bot.DisableModule(moduleSystemName);
            if (!AppConf.SystemData.DisabledModules.Contains(moduleSystemName))
            {
                AppConf.SystemData.DisabledModules.Add(moduleSystemName);
            }
            if (AppConf.SystemData.EnabledModules.Contains(moduleSystemName))
                AppConf.SystemData.EnabledModules.Remove(moduleSystemName);
            AppConf.Save();
        }

        public void EnableModule(string moduleSystemName)
        {
            Bot.EnableModule(moduleSystemName);
            if (AppConf.SystemData.DisabledModules.Contains(moduleSystemName))
            {
                AppConf.SystemData.DisabledModules.Remove(moduleSystemName);
            }
            if (!AppConf.SystemData.EnabledModules.Contains(moduleSystemName))
                AppConf.SystemData.EnabledModules.Add(moduleSystemName);
            AppConf.Save();
        }
        public void RemoveModuleFromConfig(string moduleSystemName)
        {
            if (AppConf.SystemData.DisabledModules.Contains(moduleSystemName))
            {
                AppConf.SystemData.DisabledModules.Remove(moduleSystemName);
            }
            if (AppConf.SystemData.EnabledModules.Contains(moduleSystemName))
            {
                AppConf.SystemData.EnabledModules.Remove(moduleSystemName);
            }
            AppConf.Save();
        }
        public void CopyToClipboard(string popupTitle, string text)
        {
            LogManager.Trace("Start CopyToClipboard()");
            Clipboard.SetText(text.Trim());
            ShowInternalPopup(popupTitle, "Результат команды скопирован в буфер обмена", TimeSpan.FromSeconds(3));
            LogManager.Trace("End CopyToClipboard()");

        }
    }
}
