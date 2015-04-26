using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HelloBotCore.Entities;
using Microsoft.Win32;
using MonkeyJobTool.Controls.Settings;
using MonkeyJobTool.Entities;

namespace MonkeyJobTool.Forms
{
    public partial class SettingsForm : Form
    {
       private List<CommandReplaceBlock> _replaceBlocks = new List<CommandReplaceBlock>();

        public SettingsForm()
        {
            InitializeComponent();
        }
        
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            chkIsWithWindowsStart.Checked = IsStartupShortcutExist();
            chkIsHideDonateBtn.Checked = !App.Instance.AppConf.ShowDonateButton;
            chkIsDenyCollectingStats.Checked = !App.Instance.AppConf.AllowUsingGoogleAnalytics;
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
        }


        private void chkIsWithWindowsStart_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private Dictionary<string, Keys> specialKeys = new Dictionary<string, Keys>()
        {
            {"", Keys.None},
            {"CTRL", Keys.Control},
            {"ALT", Keys.Alt},
            {"SHIFT", Keys.Shift}
        };

        private Dictionary<string, Keys> ordinalKeys = new Dictionary<string, Keys>()
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
            cmbKey1.DataSource = new BindingSource(specialKeys, null);
            cmbKey1.DisplayMember = "Key";
            cmbKey1.ValueMember = "Value";

            cmbKey2.DataSource = new BindingSource(specialKeys, null);
            cmbKey2.DisplayMember = "Key";
            cmbKey2.ValueMember = "Value";

            cmbKey3.DataSource = new BindingSource(ordinalKeys, null);
            cmbKey3.DisplayMember = "Key";
            cmbKey3.ValueMember = "Value";

            var openAppHotKeys = App.Instance.AppConf.HotKeys.ProgramOpen;
            var oahkParts = openAppHotKeys.Split('+');
            cmbKey3.SelectedIndex = cmbKey3.FindString(oahkParts.Last()); 
            if(oahkParts.Length==3)
                cmbKey2.SelectedIndex = cmbKey2.FindString(oahkParts[1]); 
            cmbKey1.SelectedIndex = cmbKey1.FindString(oahkParts.First());
        }

         
        private void CommandReplaceDatabind()
        {
            var replaces = App.Instance.AppConf.CommandReplaces;
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
            App.Instance.AppConf.HotKeys.ProgramOpen = string.Join("+", new List<string>() { cmbKey1.Text, cmbKey2.Text, cmbKey3.Text }.Where(x=>!string.IsNullOrEmpty(x)).ToArray());
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
            foreach (var mod in App.Instance.Bot.AllModules.OrderByDescending(x=>x.ModuleType).ThenBy(x=>x.ModuleSystemName))
            {
                AddModuleInfoToGrid(mod.GetModuleName(), mod.ModuleType == ModuleType.Handler ? "Команда" : "Событийный", mod.IsEnabled, mod.ModuleSystemName);
            }
        }

        private void AddModuleInfoToGrid(string name, string type, bool enabled, string uniqueName)
        {
            DataGridViewRow r = new DataGridViewRow {ErrorText = uniqueName};
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

            gridModules.Rows.Add(r);
        }
        #endregion

        private void gridModules_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var moduleKey = gridModules.Rows[e.RowIndex].ErrorText;
            var baseModuleInfo = App.Instance.Bot.AllModules.SingleOrDefault(x => x.ModuleSystemName == moduleKey);

            if (baseModuleInfo != null)
            {
                txtDescr.Text = baseModuleInfo.CommandDescription.Description;
                txtAuthorEmail.Text = baseModuleInfo.Author != null && !string.IsNullOrEmpty(baseModuleInfo.Author.ContactEmail) ? baseModuleInfo.Author.ContactEmail : "Не указан";
                txtAuthorName.Text = baseModuleInfo.Author != null && !string.IsNullOrEmpty(baseModuleInfo.Author.Name) ? baseModuleInfo.Author.Name : "Не указано";
                btnEnabledDisableModule.Text = (!baseModuleInfo.IsEnabled ? "В" : "Вы") + "ключить модуль";
                gridModules.Rows[e.RowIndex].Cells[gridModules.Rows[e.RowIndex].Cells.Count - 1].Value = baseModuleInfo.IsEnabled ? "Вкл" : "Выкл";
                if (baseModuleInfo.ModuleType == ModuleType.Handler)
                {
                    txtSamples.Text = string.Join(Environment.NewLine, (baseModuleInfo as ModuleCommandInfo).CommandDescription.SamplesOfUsing.ToArray());
                    txtScheme.Text = (baseModuleInfo as ModuleCommandInfo).CommandDescription.CommandScheme;
                }
            }
        }


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
            gridModules_RowEnter(null, new DataGridViewCellContextMenuStripNeededEventArgs(0, gridModules.SelectedRows[0].Index));
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }



    }
}
