using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HelloBotCore.Entities;
using Microsoft.Win32;
using MonkeyJobTool.Controls.Settings;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Helpers;
using MonkeyJobTool.Managers;
using MonkeyJobTool.Properties;

namespace MonkeyJobTool.Forms
{
    public partial class SettingsForm : Form
    {
        private List<CommandReplaceBlock> _replaceBlocks = new List<CommandReplaceBlock>();
        public List<ModuleInfoBase> ChangedModules { get; set; }


        public SettingsForm()
        {
            ChangedModules = new List<ModuleInfoBase>();
            InitializeComponent();
        }
        
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            chkIsWithWindowsStart.Checked = IsStartupShortcutExist();
            chkIsHideDonateBtn.Checked = !App.Instance.AppConf.ShowDonateButton;
            chkIsDenyCollectingStats.Checked = !App.Instance.AppConf.AllowUsingGoogleAnalytics;
            chkDenyErrorInfoSend.Checked = !App.Instance.AppConf.AllowSendCrashReports;
            HotKeysDatabind();
            CommandReplaceDatabind();
            DatabindCommandGrid();
            DatabindHelpTooltips();
        }

        private void DatabindHelpTooltips()
        {
            htStatsCollect.HelpText = "Сбор некоторой несущественной для пользователя статистики необходим для разработки MonkeyJob Tool." + Environment.NewLine +
                                      "На данный момент при запуске собирается : версия windows, разрешение экрана, кол-во запусков, факт первого запуска, гео (страна/город), локальный язык системы.";
            htDonateBtn.HelpText = @"Если вам мешает пункт ""Сказать спасибо"", вы можете его убрать из меню трея.";
            htHotKey.HelpText = "По нажатию этих клавиш будет появляться программа. Альтернатива клика на значке в трее";
            htReplace.HelpText = "Вы можете заменить любую из команд на любое удобно вам сокращение. Сокращения имеют бóльший приоритет, чем команда." + Environment.NewLine + Environment.NewLine +
                                 @"Например, замена вида ""лала >> калькулятор"" при вводе ""лала 1+2"" будет эквивалентно команде ""калькулятор 1+2"" и на экран выведется ""3""" + Environment.NewLine + Environment.NewLine +
                                 @"Система замен поддерживают назначение произвольного места аргументов. К примеру, вы часто выполняете вычисление по формуле ""x+2-y"" посредством калькулятора, где x и y -" +
                                 @" каждый раз новое число. Написав замену вида ""лулу >> калькулятор {0}+2-{1}"" вы получите гибкий вариант применения формулы и написав ""лулу 10 => 3"" получите эквивалент в виде ""калькулятор 10+2-3"". " + Environment.NewLine + Environment.NewLine +
                                 @"Аргументы записываются фигурными скобками, начиная с нуля. {0},{1},{2} и так далее. Разделителем между ними при вводе должен быть ""=>"" (если их 2 и более)."+Environment.NewLine + Environment.NewLine +
                                 @"Таким образом можно строить целые цепочки замен. Допустим, если первая замена ""лулу >> калькулятор 1+{0}"", то вписав следующую замену в виде ""лала >> лулу 5"" и набрав ""лала"" вы получите эквивалент ""лулу 5""."+Environment.NewLine+
                                 "Поддерживается не более 20 связных перенаправлений.";
            hsSendErrorReport.HelpText = @"Если в приложении случается ошибка, то программа автоматически отсылает лог ошибки с базовой информацией о системе (тип ОС, версия .net Framework, версия программы). Пожалуйста, не отключайте эту опцию, отчёты об ошибках позволяют делать программу стабильнее";
        }


        private void chkIsWithWindowsStart_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private Dictionary<string, Keys> _specialKeys = new Dictionary<string, Keys>()
        {
            {"", Keys.None},
            {"CTRL", Keys.Control},
            {"ALT", Keys.Alt},
            {"SHIFT", Keys.Shift}
        };

        private Dictionary<string, Keys> _ordinalKeys = new Dictionary<string, Keys>()
        {
            {"", Keys.None},
            {"Q", Keys.Q},
            {"W", Keys.W},
            {"E", Keys.E},
            {"R", Keys.R},
            {"T", Keys.T},
            {"Y", Keys.Y},
            {"U", Keys.U},
            {"I", Keys.I},
            {"O", Keys.O},
            {"P", Keys.P},
            {"A", Keys.A},
            {"S", Keys.S},
            {"D", Keys.D},
            {"F", Keys.F},
            {"G", Keys.G},
            {"H", Keys.H},
            {"J", Keys.J},
            {"K", Keys.K},
            {"L", Keys.L},
            {"Z", Keys.Z},
            {"X", Keys.X},
            {"C", Keys.C},
            {"V", Keys.V},
            {"B", Keys.B},
            {"N", Keys.N},
            {"M", Keys.M},
            {"F1", Keys.F1},
            {"F2", Keys.F2},
            {"F3", Keys.F3},
            {"F4", Keys.F4},
            {"F5", Keys.F5},
            {"F6", Keys.F6},
            {"F7", Keys.F7},
            {"F8", Keys.F8},
            {"F9", Keys.F9},
            {"F10", Keys.F10},
            {"F11", Keys.F11},
            {"F12", Keys.F12},
        };

        private void HotKeysDatabind()
        {
            cmbKey1.DataSource = new BindingSource(_specialKeys, null);
            cmbKey1.DisplayMember = "Key";
            cmbKey1.ValueMember = "Value";

            cmbKey2.DataSource = new BindingSource(_specialKeys, null);
            cmbKey2.DisplayMember = "Key";
            cmbKey2.ValueMember = "Value";

            cmbKey3.DataSource = new BindingSource(_ordinalKeys, null);
            cmbKey3.DisplayMember = "Key";
            cmbKey3.ValueMember = "Value";


            //hack ¯\_(ツ)_/¯
            cmbKey1.SelectedIndex = 1;
            cmbKey1.SelectedIndex = 0;

            cmbKey2.SelectedIndex = 1;
            cmbKey2.SelectedIndex = 0;

            cmbKey3.SelectedIndex = 1;
            cmbKey3.SelectedIndex = 0;

            var openAppHotKeys = App.Instance.AppConf.HotKeys.ProgramOpen;
            var oahkParts = openAppHotKeys.Split('+');
            cmbKey3.SelectedIndex = cmbKey3.FindString(oahkParts.Last()); 
            if(oahkParts.Length==3)
                cmbKey2.SelectedIndex = cmbKey2.FindString(oahkParts[1]); 
            cmbKey1.SelectedIndex = cmbKey1.FindString(oahkParts.First());


            
        }

         
        private void CommandReplaceDatabind()
        {
            var replaces = App.Instance.AppConf.CommandReplaces.CloneJson();
            replaces.Add(new CommandReplace() { From = "", To = "" });
            
            foreach (CommandReplace replace in replaces)
            {
                AddReplaceBlock(replace.From, replace.To);
            }
            SubscribeLastReplaceBlock();
        }

        private void SubscribeLastReplaceBlock()
        {
            var lastBlock = _replaceBlocks.Last();
            lastBlock.OnOneOfFieldFilled += OnNewBlockRequired;
        }

        private void UnsubscribeLastReplaceBlock()
        {
            var lastBlock = _replaceBlocks.Last();
            lastBlock.OnOneOfFieldFilled -= OnNewBlockRequired;
        }

        private void OnNewBlockRequired()
        {
            UnsubscribeLastReplaceBlock();
            AddReplaceBlock(string.Empty, string.Empty);
            SubscribeLastReplaceBlock();
        }

        private void AddReplaceBlock(string from, string to)
        {
            var crBlock = new CommandReplaceBlock();
            crBlock.From = from;
            crBlock.To = to;
            pnlCommandReplaces.Controls.Add(crBlock);
            _replaceBlocks.Add(crBlock);
        }

        private void SaveConfiguration()
        {
            if (chkIsWithWindowsStart.Checked)
            {
                AppShortcutToStartup();
            }
            else
            {
                DelAppShortcutFromStartup();
            }

            App.Instance.AppConf.ShowDonateButton = !chkIsHideDonateBtn.Checked;
            App.Instance.AppConf.AllowUsingGoogleAnalytics = !chkIsDenyCollectingStats.Checked;
            App.Instance.AppConf.AllowSendCrashReports = !chkDenyErrorInfoSend.Checked;
            App.Instance.AppConf.HotKeys.ProgramOpen = string.Join("+", new List<string>() { cmbKey1.Text,cmbKey2.Text ,cmbKey3.Text }.Where(x => !string.IsNullOrEmpty(x)).ToArray());
            App.Instance.AppConf.CommandReplaces = GetCommandReplaces();
            App.Instance.AppConf.Save();
            App.Instance.ReInitHotKeys();
            App.Instance.NotifyAboutSettingsChanged();
            this.Close();
        }

        private void AppShortcutToStartup()
        {
            string linkName = AppConstants.AppName;
            string startDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            if (!File.Exists(startDir + "\\" + linkName + ".url"))
            {
                using (StreamWriter writer = new StreamWriter(startDir + "\\" + linkName + ".url"))
                {
                    writer.WriteLine("[InternetShortcut]");
                    writer.WriteLine("URL=file:///" + App.Instance.ExecutionPath);
                    writer.WriteLine("IconIndex=0");
                    string icon = App.Instance.ExecutionFolder + @"Res\mj.ico";
                    writer.WriteLine("IconFile=" + icon);
                    writer.Flush();
                }
            }
        }

        private void DelAppShortcutFromStartup()
        {
            string linkName = AppConstants.AppName;
            string startDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            if (File.Exists(startDir + "\\" + linkName + ".url"))
            {
                File.Delete(startDir + "\\" + linkName + ".url");
            }
        }

        public bool IsStartupShortcutExist()
        {
            string linkName = AppConstants.AppName;
            string startDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            return File.Exists(startDir + "\\" + linkName + ".url");
        }

        private List<CommandReplace> GetCommandReplaces()
        {
            return _replaceBlocks.Select(x => new CommandReplace()
            {
                From = x.From,
                To = x.To
            }).Where(x=>!string.IsNullOrEmpty(x.From.Trim())&&!string.IsNullOrEmpty(x.To.Trim())).ToList();
        } 

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            SaveConfiguration();
        }

        #region command grid
        private void DatabindCommandGrid()
        {
            var orderedModules = App.Instance.Bot.AllModules.OrderByDescending(x => ChangedModules.Contains(x)).ThenByDescending(x=>x.ModuleType).ThenBy(x => x.ModuleSystemName).ToList();

            foreach (var mod in orderedModules)
            {
                Color? rowColor = null;
                if(ChangedModules.Any(x=>x.Id==mod.Id))
                    rowColor = Color.PaleVioletRed;
                
                AddModuleInfoToGrid(mod.GetModuleName(), mod.GetTypeDescription(), mod.IsEnabled, mod.ModuleSystemName, mod.ModuleSettingsType != null,rowColor);
            }

            gridModules.Rows[0].Selected = true;
        }

        private void AddModuleInfoToGrid(string name, string type, bool enabled, string uniqueName, bool isWithSettings, Color? rowColor=null)
        {
            DataGridViewRow r = new DataGridViewRow {ErrorText = uniqueName};
            if (rowColor.HasValue)
            {
                r.DefaultCellStyle.BackColor = rowColor.Value;
            }
            r.Cells.Add(new DataGridViewTextBoxCell()
            {
                Value = name,
                Style = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleLeft }
            }); 
            r.Cells.Add(new DataGridViewTextBoxCell()
            {
                Value = type,
                Style = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            r.Cells.Add(new DataGridViewTextBoxCell()
            {
                Value = enabled?"Вкл":"Выкл",
                Style = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            r.Cells.Add(new DataGridViewImageCell()
            {
                Value = isWithSettings?Resources.settings_small:new Bitmap(1,1),

                Style = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            gridModules.Rows.Add(r);
        }

        private void gridModules_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(gridModules.SelectedRows.Count==1)
            {
                var moduleKey = gridModules.Rows[gridModules.SelectedRows[0].Index].ErrorText;
                var module = App.Instance.Bot.AllModules.SingleOrDefault(x => x.ModuleSystemName == moduleKey);
                btnEnabledDisableModule.Text = (!module.IsEnabled ? "В" : "Вы") + "ключить модуль";
                btnShowLogs.Enabled = module.Trace.TraceMessages.Any();
            }
        }

        private HelpPopup _commandHelpCommand = null;

        private int _displayHelpRowId = -1;
        private void gridModules_MouseMove(object sender, MouseEventArgs e)
        {
            int rowIndex = gridModules.HitTest(e.X, e.Y).RowIndex;
            if (rowIndex >= 0)
            {
                if (rowIndex != _displayHelpRowId)
                {
                    _displayHelpRowId = rowIndex;
                    var moduleKey = gridModules.Rows[rowIndex].ErrorText;
                    if (!string.IsNullOrEmpty(moduleKey))
                    {
                        var module = App.Instance.Bot.AllModules.SingleOrDefault(x => x.ModuleSystemName == moduleKey);

                        if (_commandHelpCommand != null)
                        {
                            //todo : do not hide, only change text (trouble with dynamic border exist). reason : blinking
                            _commandHelpCommand.Hide();
                        }
                        _commandHelpCommand = new HelpPopup { FormType = PopupFormType.CommandInfo };
                        _commandHelpCommand.HelpData = new HelpInfo()
                        {
                            Body = module.ToString(),
                            Icon = module.Icon ?? Resources.monkey_highres_img,
                            Title = "Информация о модуле " + module.GetModuleName()
                        };
                        _commandHelpCommand.Init();
                    }
                }
                if (_commandHelpCommand != null)
                {
                    var gridLoc = gridModules.PointToScreen(Point.Empty);
                    gridLoc.Y += gridModules.Height+10;
                    gridLoc.X -= 10;
                    _commandHelpCommand.Location = gridLoc;
                    _commandHelpCommand.ToTop();
                }
            }
            else
            {
                if (_commandHelpCommand != null)
                {
                    _commandHelpCommand.Hide();
                }
            }
        }

        private void gridModules_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {


        }
        #endregion

        private void btnEnabledDisableModule_Click(object sender, EventArgs e)
        {
            var moduleKey = gridModules.Rows[gridModules.SelectedRows[0].Index].ErrorText;
            
            var module = App.Instance.Bot.AllModules.SingleOrDefault(x => x.ModuleSystemName == moduleKey);
            if (module.IsEnabled)
            {
                App.Instance.DisableModule(module.ModuleSystemName);
            }
            else
            {
                App.Instance.EnableModule(module.ModuleSystemName);
            }
            gridModules.Rows[gridModules.SelectedRows[0].Index].Cells["colIsEnabled"].Value = module.IsEnabled ? "Вкл" : "Выкл";
            btnEnabledDisableModule.Text = (!module.IsEnabled ? "В" : "Вы") + "ключить модуль";
        }

        private void gridModules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == gridModules.Rows[e.RowIndex].Cells["settingsCol"].ColumnIndex)
            {
                var moduleKey = gridModules.Rows[e.RowIndex].ErrorText;
                var module = App.Instance.Bot.AllModules.SingleOrDefault(x => x.ModuleSystemName == moduleKey);
                if (module.ModuleSettingsType != null)
                {
                    var setMod = new ModuleSettingsForm()
                    {
                        Module = module
                    };
                    setMod.ShowDialog();
                }
            }
        }

        private void gridModules_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == gridModules.Rows[e.RowIndex].Cells["settingsCol"].ColumnIndex)
            {
                var moduleKey = gridModules.Rows[e.RowIndex].ErrorText;
                var module = App.Instance.Bot.AllModules.SingleOrDefault(x => x.ModuleSystemName == moduleKey);
                gridModules.Cursor = module.ModuleSettingsType != null ? Cursors.Hand : Cursors.Default;
            }
            else
            {
                gridModules.Cursor = Cursors.Default;
            }
        }

        private void gridModules_MouseLeave(object sender, EventArgs e)
        {
            if (_commandHelpCommand != null)
            {
                _commandHelpCommand.Hide();
            }
        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            if (ChangedModules.Any())
            {
                TPCommandReplace.SelectedTab = TPModuleSettings;
                MessageBox.Show("Внимание!\r\nСледующие модули изменили свои настройки ввиду новой версии модуля, проверьте пожалуйста, что все ваши настройки верны для этих модулей. Изменённые настройки модулей подсвечены красным цветом.\r\n" +
                                "Для того, чтобы это сообщение больше не появлялось, зайдите в настройки и пересохраните их в обновлённых модулях.", AppConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnShowLogs_Click(object sender, EventArgs e)
        {
            if(gridModules.SelectedRows.Count==1)
            {
                var moduleKey = gridModules.Rows[gridModules.SelectedRows[0].Index].ErrorText;
                var module = App.Instance.Bot.AllModules.SingleOrDefault(x => x.ModuleSystemName == moduleKey);
                var mlForm = new ModuleLogsForm(){Module = module};
                mlForm.ShowDialog();
            }
        }
    }
}
