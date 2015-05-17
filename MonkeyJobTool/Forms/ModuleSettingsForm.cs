using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCore.Entities;
using MonkeyJobTool.Controls.Autocomplete;
using MonkeyJobTool.Controls.Settings;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Entities.Json;
using MonkeyJobTool.Helpers;
using Newtonsoft.Json;


namespace MonkeyJobTool.Forms
{
    public partial class ModuleSettingsForm : Form
    {
        public ModuleCommandInfoBase Module { get; set; }
        public string ModuleSettingsPath { get; set; }

        public ModuleSettingsForm()
        {
            InitializeComponent();
        }

        private void ModuleSettingsForm_Load(object sender, EventArgs e)
        {
            string fullPath = Module.GetSettingFileFullPath(App.Instance.FolderSettingPath);
            
            string data = File.ReadAllText(fullPath);
            var settingsWrapper = JsonConvert.DeserializeObject(data, typeof(ModuleSettings)) as ModuleSettings;
            var rawSettingsJson = JsonConvert.SerializeObject(settingsWrapper.ModuleData);
            var settings = JsonConvert.DeserializeObject(rawSettingsJson, Module.ModuleSettingsType);
            pnlSettings.AccessibleName = Guid.NewGuid().ToString();

            var table = new TableLayoutPanel()
            {
                AutoSize = true,
                //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
            pnlSettings.Controls.Add(table);
            BindObject(settings, table);
        }

        
        public void BindObject(object obj, TableLayoutPanel parentControl)
        {
            var props = obj.GetType().GetProperties();
            
            foreach (var info in props)
            {
                
                var tInfo = info.PropertyType;
                var propTitle = Attribute.GetCustomAttribute(info, typeof(SettingsNameFieldAttribute)) as SettingsNameFieldAttribute;

                if (propTitle != null)
                {
                    var objVal = info.GetValue(obj, null) ;
                    if (tInfo == typeof (int))
                    {
                        AddControl(propTitle.Label, new TextBox() { Text = objVal!=null?objVal.ToString():"" }, info.Name, parentControl);
                    }
                    else if (tInfo == typeof (string))
                    {
                        AddControl(propTitle.Label, new TextBox() { Text = objVal != null ? objVal.ToString() : "" }, info.Name, parentControl);
                    }
                    else if (tInfo == typeof(bool))
                    {
                        AddControl("", new CheckBox() { Checked = objVal != null ? (bool)objVal : false, Text = propTitle.Label }, info.Name, parentControl);
                    }
                    else if (typeof(IList).IsAssignableFrom(info.PropertyType))
                    {
                        var collectionPanel = new TableLayoutPanel()
                        {
                            AutoSize = true,
                            //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                        };
                        AddControlToNewPanelRow(parentControl, GetTitleControl(propTitle.Label), 0);
                        AddControlToNewPanelRow(parentControl, collectionPanel, 20);
                        
                        DataButton btnAddNewItem = new DataButton()
                        {
                            BackColor = Color.Moccasin,
                            FlatStyle = FlatStyle.Popup,
                            UseVisualStyleBackColor = false
                        };
                        btnAddNewItem.ParentPanel = collectionPanel;
                        btnAddNewItem.ParentParentPanel = parentControl;
                        btnAddNewItem.Text = "Добавить";
                        btnAddNewItem.Width = 100;

                        var itemList = ((IEnumerable) objVal).Cast<object>().ToList();

                        if (!itemList.Any())
                        {
                            Type listItemType = objVal.GetType().GetGenericArguments().First();
                            //add one for user input
                            itemList.Add(Activator.CreateInstance(listItemType));
                        }
                        
                        foreach (var item in itemList)
                        {
                            btnAddNewItem.Data = new CloneObjData()
                            {
                                Data = item,
                                DataType = item.GetType()
                            }; //save last for clone by btn click

                            AddItemCollectionToUI(item, collectionPanel);
                        }

                        
                        btnAddNewItem.Click += (sender, args) =>
                        {
                            var clonedObj = ((DataButton)sender).Data as CloneObjData;

                            if (clonedObj != null)
                            {
                                var jsonObj = JsonConvert.SerializeObject(clonedObj.Data);
                                var materializedObj = JsonConvert.DeserializeObject(jsonObj, clonedObj.DataType);
                                AddItemCollectionToUI(materializedObj, ((DataButton)sender).ParentPanel);
                            }
                        };

                        //button to next row
                        AddControlToNewPanelRow(parentControl, btnAddNewItem, parentControl.Width - btnAddNewItem.Width);
                    }
                    else //complex object
                    {
                        var collectionPanel = new TableLayoutPanel()
                        {
                            AutoSize = true,
                            //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                        };
                        AddControlToNewPanelRow(parentControl, GetTitleControl(propTitle.Label), 0);
                        AddControlToNewPanelRow(parentControl, collectionPanel, 0);
                        if (objVal == null)
                        {
                            objVal = Activator.CreateInstance(tInfo.UnderlyingSystemType);
                        }
                        BindObject(objVal, collectionPanel);
                    }
                }

                
            }
            
        }

        private void AddItemCollectionToUI(object item,TableLayoutPanel parentControl)
        {
            var collectionItemPanel = new TableLayoutPanel()
            {
                AutoSize = true
            };
            AddControlToNewPanelRow(parentControl, collectionItemPanel, 0);
            BindObject(item, collectionItemPanel);
            AddRemoveBtn(collectionItemPanel, parentControl);
        }

        private void AddRemoveBtn(TableLayoutPanel addToControl, TableLayoutPanel parentControl)
        {
            DataButton btnRemoveItem = new DataButton()
            {
                BackColor = Color.Moccasin,
                FlatStyle = FlatStyle.Popup,
                UseVisualStyleBackColor = false,
                Text = "Удалить"
            };
            btnRemoveItem.ParentPanel = addToControl;
            btnRemoveItem.ParentParentPanel = parentControl;
            btnRemoveItem.Click += (sender, args) =>
            {
                var parentContainer = ((DataButton)sender).ParentPanel;
                var parentParentContainer = ((DataButton)sender).ParentParentPanel;
                parentParentContainer.Controls.Remove(parentContainer);
            };
            AddControlToNewPanelRow(addToControl, btnRemoveItem, 0);
        }

        private Control GetTitleControl(string title)
        {
            return new RichTextLabel()
            {
                Text = title,
                Dock = DockStyle.Fill,
                BackColor = Color.Bisque
            };
        }

        private void AddControl(string label, Control cntrl, string propName, TableLayoutPanel parentControl)
        {
            Panel tPanel = new Panel();
            tPanel.Width = 310;
            if (!string.IsNullOrEmpty(label))
            {
                var lbl = new RichTextLabel()
                {
                    Text = label,
                    AutoSize = false,
                    Width = 100,
                    BackColor = Color.White
                    //Top = top
                };
                tPanel.Controls.Add(lbl);
                
                cntrl.Left += lbl.Width;
            }
            cntrl.Width = 200;
            cntrl.AccessibleName = propName;
            tPanel.Controls.Add(cntrl);
            AddControlToNewPanelRow(parentControl, tPanel, 0);
            tPanel.Height = (from Control control in tPanel.Controls select control.Height).Concat(new[] { 0 }).Max();
        }

        private void AddControlToNewPanelRow(TableLayoutPanel panel, Control cntrl, int leftMargin)
        {
            cntrl.Margin = new Padding(leftMargin, 2, 0, 0);
            panel.RowStyles.Add(new RowStyle());
            panel.Controls.Add(cntrl, 1, panel.RowCount);
            panel.RowCount++;
        }

        static public Type GetDeclaredType<TSelf>(TSelf self)
        {
            return typeof(TSelf);
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {

        }
    }
}
