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
        public ClientInstanceToModuleCommunication ClientData { get; set; }

        public delegate void OnSaveDelegate(ClientInstanceToModuleCommunication clientData);
        public event OnSaveDelegate OnSave;


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
                new {Id = (int)ComponentType.Event, Value = "Интервальный"},
                new {Id = (int)ComponentType.Handler, Value = "Команда"},
                new {Id = (int)ComponentType.Tray, Value = "Трей"},
            };
            cmb.DisplayMember = "Value";
            cmb.ValueMember = "Id";
        }

        private void InitModuleLists()
        {
            lstAvailableModules.DataSource = App.Instance.Bot.Modules.SelectMany(x=>x.Instances).Select(x => new
            {
                Name = x.GetComponentName()+" ("+x.GetTypeDescription()+")",
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
            if (ClientData.EnabledForAll)
            {
                rdAll.Checked = ClientData.EnabledForAll;
            }

            if (ClientData.EnabledByType.HasValue)
            {
                rdByModuleType.Checked = true;
                cmbModuleTypes.SelectedValue = (int) ClientData.EnabledByType.Value;
            }

            if (ClientData.DisabledByType.HasValue)
            {
                rdExceptType.Checked = true;
                cmbExceptType.SelectedValue = (int)ClientData.DisabledByType.Value;
            }
                

            if (ClientData.EnabledModules.Any())
            {
                rdOnlyModules.Checked = true;
                foreach (var moduleName in ClientData.EnabledModules)
                {
                    var module = App.Instance.Bot.Modules.FirstOrDefault(x => x.SystemName == moduleName);
                    MoveModule(lstAvailableModules, lstCheckedModules, new
                    {
                        module.Id,
                        Name = module.GetComponentName()+" (" + module.GetTypeDescription() + ")", module.SystemName
                    });
                }
            }
            else
                if (ClientData.DisabledModules.Any())
                {
                    rdExceptModules.Checked = true;
                    foreach (var moduleName in ClientData.DisabledModules)
                    {
                        var module = App.Instance.Bot.Modules.FirstOrDefault(x => x.SystemName == moduleName);
                        MoveModule(lstAvailableModules, lstCheckedModules, new
                        {
                            module.Id,
                            Name = module.GetComponentName() + " (" + module.GetTypeDescription() + ")",
                            module.SystemName
                        });
                    }
                }
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            ClientData.Reset();
            if (rdAll.Checked)
            {
                ClientData.EnabledForAll = true;
            }
            else if (rdByModuleType.Checked)
            {
                ClientData.EnabledByType = (ComponentType)(int) cmbModuleTypes.SelectedValue;
            }
            else if (rdExceptType.Checked)
            {
                ClientData.DisabledByType = (ComponentType)(int)cmbExceptType.SelectedValue;
            }
            else if (rdOnlyModules.Checked)
            {
                ClientData.EnabledModules = lstCheckedModules.Items.Cast<dynamic>().Select(x=>(string)x.SystemName).ToList();
                if (!ClientData.EnabledModules.Any()) return;
            }
            else if (rdExceptModules.Checked)
            {
                ClientData.DisabledModules = lstCheckedModules.Items.Cast<dynamic>().Select(x => (string)x.SystemName).ToList();
                if (!ClientData.DisabledModules.Any()) return;
            }

            if (OnSave!=null)
                OnSave(ClientData);
            
            this.Close();
        }
    }
}
