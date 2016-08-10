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

namespace MonkeyJobTool.Forms
{
    public partial class ClientToModulesForm : Form
    {
        public ModuleToModuleCommunication ClientToModuleData { get; set; }

        public ClientToModulesForm()
        {
            InitializeComponent();
        }

        private void ClientToModulesForm_Load(object sender, EventArgs e)
        {
            InitModuleTypes(cmbExceptType);
            InitModuleTypes(cmbModuleTypes);
            InitModuleLists();
            DataBindFromConfig();
            DataBindPanels();
        }

        private void btnAddModule_Click(object sender, EventArgs e)
        {
            foreach (dynamic selectedItem in lstAvailableModules.SelectedItems)
            {
                MoveModule(lstAvailableModules, lstCheckedModules, selectedItem);
            }
        }

        private void btnRemoveModule_Click(object sender, EventArgs e)
        {
            foreach (dynamic selectedItem in lstCheckedModules.SelectedItems)
            {
                MoveModule(lstCheckedModules, lstAvailableModules, selectedItem);
            }
        }

        private void MoveModule(ListBox from, ListBox to, dynamic item)
        {
            var checkedList = to.Items.Cast<dynamic>().Select(x => new { x.Id, x.Name, x.SystemName }).ToList();
            checkedList.Add(new { item.Id, item.Name,item.SystemName });
            to.DataSource = checkedList;
            from.DataSource = from.Items.Cast<dynamic>().Select(x => new { x.Id, x.Name,x.SystemName }).Where(x => x.Id != item.Id).ToList();
        }

        private void InitModuleTypes(ComboBox cmb)
        {
            cmb.DataSource = new List<object>()
            {
                new {Id = (int)ModuleType.Event, Value = "Событийный"},
                new {Id = (int)ModuleType.Handler, Value = "Команда"},
                new {Id = (int)ModuleType.Tray, Value = "Трей"},
            };
            cmb.DisplayMember = "Value";
            cmb.ValueMember = "Id";
        }

        private void InitModuleLists()
        {
            lstAvailableModules.DataSource = App.Instance.Bot.Modules.Select(x => new
            {
                Name = x.GetModuleName()+" ("+x.GetTypeDescription()+")",
                x.Id,x.SystemName
            }).ToList();
            lstAvailableModules.DisplayMember = "Name";
            lstAvailableModules.ValueMember = "Id";

            lstCheckedModules.DisplayMember = "Name";
            lstCheckedModules.ValueMember = "Id";
        }

        private void DataBindPanels()
        {
            pnlByType.Visible = rdByModuleType.Checked;
            pnlListOfModules.Enabled = rdExceptModules.Checked || rdOnlyModules.Checked;
            pnlExceptType.Visible = rdExceptType.Checked;
            if (rdExceptModules.Checked)
                lblModuleLabel.Text = "Кроме : ";
            else if (rdOnlyModules.Checked)
                lblModuleLabel.Text = "Только : ";
            else
                lblModuleLabel.Text = "";
        }

        private void rd_CheckedChanged(object sender, EventArgs e)
        {
            DataBindPanels();
        }

        private void DataBindFromConfig()
        {
            if (ClientToModuleData.EnabledForAll)
            {
                rdAll.Checked = ClientToModuleData.EnabledForAll;
            }

            if (ClientToModuleData.EnabledByType.HasValue)
            {
                rdByModuleType.Checked = true;
                cmbModuleTypes.SelectedValue = (int) ClientToModuleData.EnabledByType.Value;
            }

            if (ClientToModuleData.DisabledByType.HasValue)
            {
                rdExceptType.Checked = true;
                cmbExceptType.SelectedValue = (int)ClientToModuleData.DisabledByType.Value;
            }
                

            if (ClientToModuleData.EnabledModules.Any())
            {
                rdOnlyModules.Checked = true;
                foreach (var moduleName in ClientToModuleData.EnabledModules)
                {
                    var module = App.Instance.Bot.Modules.FirstOrDefault(x => x.SystemName == moduleName);
                    MoveModule(lstAvailableModules, lstCheckedModules, new
                    {
                        module.Id,
                        Name = module.GetModuleName()+" (" + module.GetTypeDescription() + ")", module.SystemName
                    });
                }
            }
            else
                if (ClientToModuleData.DisabledModules.Any())
                {
                    rdExceptModules.Checked = true;
                    foreach (var moduleName in ClientToModuleData.DisabledModules)
                    {
                        var module = App.Instance.Bot.Modules.FirstOrDefault(x => x.SystemName == moduleName);
                        MoveModule(lstAvailableModules, lstCheckedModules, new
                        {
                            module.Id,
                            Name = module.GetModuleName() + " (" + module.GetTypeDescription() + ")",
                            module.SystemName
                        });
                    }
                }
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            ClientToModuleData.Reset();
            if (rdAll.Checked)
            {
                ClientToModuleData.EnabledForAll = true;
            }
            else if (rdByModuleType.Checked)
            {
                ClientToModuleData.EnabledByType = (ModuleType)(int) cmbModuleTypes.SelectedValue;
            }
            else if (rdExceptType.Checked)
            {
                ClientToModuleData.DisabledByType = (ModuleType)(int)cmbExceptType.SelectedValue;
            }
            else if (rdOnlyModules.Checked)
            {
                ClientToModuleData.EnabledModules = lstCheckedModules.Items.Cast<dynamic>().Select(x=>(string)x.SystemName).ToList();
                if (!ClientToModuleData.EnabledModules.Any()) return;
            }
            else if (rdExceptModules.Checked)
            {
                ClientToModuleData.DisabledModules = lstCheckedModules.Items.Cast<dynamic>().Select(x => (string)x.SystemName).ToList();
                if (!ClientToModuleData.DisabledModules.Any()) return;
            }

            //App.Instance.AppConf.SystemData.AddUpdateModuleCommunicationForClient(ClientToModuleData);
            App.Instance.AppConf.Save();
            this.Close();
        }
    }
}
