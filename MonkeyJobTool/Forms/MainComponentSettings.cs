using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HelloBotCore.Entities;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Properties;
using NLog.LayoutRenderers;

namespace MonkeyJobTool.Forms
{
    public partial class MainComponentSettings : Form
    {
        public MainComponentSettings()
        {
            InitializeComponent();
        }
        private bool _gridRowsInited = false;
        public ComponentInfoBase Component { get; set; }

        private void MainComponentSettings_Load(object sender, EventArgs e)
        {
            btnShowModuleCommunication.Visible = Component.ModuleType == ModuleType.IntegrationClient;
            this.Text =  "Экземпляр "+(Component.ModuleType == ModuleType.IntegrationClient ? "клиента" : "модуля");
            DatabindGrid();
        }

        private void gridClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = ((DataGridView)sender);

            if (e.RowIndex >= 0 && e.ColumnIndex == grid.Rows[e.RowIndex].Cells["settingsCol"].ColumnIndex)
            {
                var compSystemName = grid.Rows[e.RowIndex].ErrorText;
                var client = Component.Instances.SingleOrDefault(x => x.SystemName == compSystemName);

                if (client.SettingsType != null)
                {
                    var setMod = new ModuleSettingsForm()
                    {
                        Module = client
                    };
                    setMod.ShowDialog();
                }
            }
        }

        private void gridClients_SelectionChanged(object sender, EventArgs e)
        {
            var grid = ((DataGridView)sender);
            if (grid.SelectedRows.Count == 1)
            {

                var compSystemName = grid.Rows[grid.SelectedRows[0].Index].ErrorText;

                var client = Component.Instances.SingleOrDefault(x => x.SystemName == compSystemName);
                if (_gridRowsInited)
                {
                    btnEnabledDisableClient.Enabled = true;
                    if(Component.ModuleType==ModuleType.IntegrationClient)
                        btnShowModuleCommunication.Enabled = true;
                }
                btnEnabledDisableClient.Text = (!client.IsEnabled ? "В" : "Вы") + "ключить";
                btnShowClientLogs.Enabled = client.Trace.TraceMessages.Any();
                btnRemoveClient.Enabled = true;
            }
            else
            {
                btnRemoveClient.Enabled = false;
                btnShowClientLogs.Enabled = false;
                if(Component.ModuleType==ModuleType.IntegrationClient)
                    btnShowModuleCommunication.Enabled = false;
                btnEnabledDisableClient.Enabled = false;
            }
        }

        private void btnShowModuleCommunication_Click(object sender, EventArgs e)
        {
            var compSystemName = gridClients.Rows[gridClients.SelectedRows[0].Index].ErrorText;
            var client = Component.Instances.SingleOrDefault(x => x.SystemName== compSystemName);
            var commForm = new ClientToModulesForm()
            {
                ClientData = (client as IntegrationClientInfo).InstanceCommunication,
            };
            commForm.OnSave += data =>
            {
                (client as IntegrationClientInfo).InstanceCommunication = data;
                HelloBotCore.Entities.MainIntegrationClientServiceSettings serviceData;
                client.GetSettings<object, HelloBotCore.Entities.MainIntegrationClientServiceSettings>(out serviceData);
                if (serviceData == null) serviceData = new HelloBotCore.Entities.MainIntegrationClientServiceSettings();
                serviceData.ClientInstanceToModuleCommunication = data;
                client.SaveServiceData(serviceData);
            };
            commForm.ShowDialog();
        }

        private void DatabindGrid()
        {   
            gridClients.Rows.Clear();
            var items = Component.Instances;
            foreach (var inst in items)
            {
                AddModuleInfoToGrid(inst.SystemName,inst.IsEnabled,  inst.SettingsType != null);
            }
        }

        private void AddModuleInfoToGrid(string systemName, bool enabled, bool isWithSettings, Color? rowColor = null)
        {
            DataGridViewRow r = new DataGridViewRow { ErrorText = systemName };
            if (rowColor.HasValue)
            {
                r.DefaultCellStyle.BackColor = rowColor.Value;
            }
            r.Cells.Add(new DataGridViewTextBoxCell()
            {
                Value = systemName,
                Style = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });
            
            r.Cells.Add(new DataGridViewTextBoxCell()
            {
                Value = enabled ? "Вкл" : "Выкл",
                Style = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            r.Cells.Add(new DataGridViewImageCell()
            {
                Value = isWithSettings ? Resources.settings_small : new Bitmap(1, 1),

                Style = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            gridClients.Rows.Add(r);
        }

        private void btnEnabledDisableClient_Click(object sender, EventArgs e)
        {
            DataGridView grid =  gridClients;
            
            var compSystemName = grid.Rows[grid.SelectedRows[0].Index].ErrorText;

            var instance = Component.Instances.Select(x => x).SingleOrDefault(x => x.SystemName == compSystemName);
            if (instance.IsEnabled)
            {
                App.Instance.DisableModule(instance.SystemName);
            }
            else
            {
                App.Instance.EnableModule(instance.SystemName);
            }
            
            btnEnabledDisableClient.Text = (!instance.IsEnabled ? "Под" : "Вы") + "ключить клиент";

            grid.Rows[grid.SelectedRows[0].Index].Cells["colIsEnabled"].Value = instance.IsEnabled ? "Вкл" : "Выкл";
        }

        private void btnShowClientLogs_Click(object sender, EventArgs e)
        {
            DataGridView grid =  gridClients;
            if (grid.SelectedRows.Count == 1)
            {
                var compSystemName = grid.Rows[grid.SelectedRows[0].Index].ErrorText;

                var instance = Component.Instances.Select(x => (ComponentInfoBase)x).SingleOrDefault(x => x.SystemName == compSystemName);
                var messages = instance.Trace.TraceMessages;
                var mlForm = new ModuleLogsForm() { LogMessages = messages };
                mlForm.ShowDialog();
            }
        }

        private void MainComponentSettings_Shown(object sender, EventArgs e)
        {
            _gridRowsInited = true;
            gridClients.ClearSelection();
        }

        private void btnAddComponent_Click(object sender, EventArgs e)
        {
            App.Instance.Bot.AddComponentInstance(Component.SystemName);
            DatabindGrid();
        }

        private void btnRemoveComponent_Click(object sender, EventArgs e)
        {
            DataGridView grid =  gridClients;
            if (grid.SelectedRows.Count == 1)
            {
                var compSystemName = grid.Rows[grid.SelectedRows[0].Index].ErrorText;

                var instance = Component.Instances.Select(x => x).SingleOrDefault(x => x.SystemName == compSystemName);
                App.Instance.Bot.RemoveComponentInstance(Component.SystemName);
                DatabindGrid();
            }
        }

        
    }
}
