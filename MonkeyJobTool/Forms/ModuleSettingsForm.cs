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
            object moduleSettings;
            if (!File.Exists(fullPath))
            {
                moduleSettings = Activator.CreateInstance(Module.ModuleSettingsType);
            }
            else
            {
                string data = File.ReadAllText(fullPath);
                var settingsWrapper = JsonConvert.DeserializeObject(data, typeof (ModuleSettings)) as ModuleSettings;
                var rawSettingsJson = JsonConvert.SerializeObject(settingsWrapper.ModuleData);
                moduleSettings = JsonConvert.DeserializeObject(rawSettingsJson, Module.ModuleSettingsType);
                //pnlSettings.AccessibleName = Guid.NewGuid().ToString();
            }
            var table = new TableLayoutPanel()
            {
                AutoSize = true,
                //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
            pnlSettings.Controls.Add(table);
            BindObject(moduleSettings, table);
        }

        
        public void BindObject(object obj, TableLayoutPanel parentControl, int deepLevel=1, int? collectionIndex = null)
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
                        AddControl(propTitle.Label, new TextBox() { Text = objVal != null ? objVal.ToString() : "" }, info.Name, parentControl, deepLevel, collectionIndex);
                    }
                    else if (tInfo == typeof (string))
                    {
                        AddControl(propTitle.Label, new TextBox() { Text = objVal != null ? objVal.ToString() : "" }, info.Name, parentControl, deepLevel, collectionIndex);
                    }
                    else if (tInfo == typeof(bool))
                    {
                        AddControl("", new CheckBox() { Checked = objVal != null ? (bool)objVal : false, Text = propTitle.Label }, info.Name, parentControl, deepLevel, collectionIndex);
                    }
                    else if (typeof(IList).IsAssignableFrom(info.PropertyType))
                    {
                        var collectionPanel = new TableLayoutPanel()
                        {
                            AutoSize = true,
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
                        btnAddNewItem.DeepLevel = deepLevel;
                        btnAddNewItem.Text = "Добавить";
                        btnAddNewItem.Width = 100;

                        var itemList = ((IEnumerable) objVal).Cast<object>().ToList();

                        if (!itemList.Any())
                        {
                            Type listItemType = objVal.GetType().GetGenericArguments().First();
                            //add one for user input
                            itemList.Add(Activator.CreateInstance(listItemType));
                        }

                        
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            var item = itemList[i];
                            btnAddNewItem.Data = new CloneObjData()
                            {
                                Data = item,
                                DataType = item.GetType()
                            }; //save last for clone by btn click

                            AddItemCollectionToUI(item, collectionPanel, deepLevel,i);
                        }


                        btnAddNewItem.Click += (sender, args) =>
                        {
                            var clonedObj = ((DataButton)sender).Data as CloneObjData;

                            if (clonedObj != null)
                            {
                                var jsonObj = JsonConvert.SerializeObject(clonedObj.Data);
                                var materializedObj = JsonConvert.DeserializeObject(jsonObj, clonedObj.DataType);
                                
                                AddItemCollectionToUI(materializedObj, ((DataButton)sender).ParentPanel, ((DataButton)sender).DeepLevel,);//какой тут индекс? что делать при удалении?
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
                        };
                        AddControlToNewPanelRow(parentControl, GetTitleControl(propTitle.Label), 0);
                        AddControlToNewPanelRow(parentControl, collectionPanel, 0);
                        if (objVal == null)
                        {
                            objVal = Activator.CreateInstance(tInfo.UnderlyingSystemType);
                        }
                        BindObject(objVal, collectionPanel,++deepLevel);
                    }
                }

                
            }
            
        }

        private void AddItemCollectionToUI(object item, TableLayoutPanel parentControl, int deepLevel, int? collectionIndex = null)
        {
            var collectionItemPanel = new TableLayoutPanel()
            {
                AutoSize = true
            };
            AddControlToNewPanelRow(parentControl, collectionItemPanel, 0);
            BindObject(item, collectionItemPanel, ++deepLevel, collectionIndex);
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

        private void AddControl(string label, Control cntrl, string propName, TableLayoutPanel parentControl, int deepLevel, int? collectionIndex=null)
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
            cntrl.AccessibleName = propName + deepLevel + (collectionIndex!=null?"_"+collectionIndex:"");
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
            string fullPath = Module.GetSettingFileFullPath(App.Instance.FolderSettingPath);
            object moduleSettings;
            ModuleSettings ms = new ModuleSettings(Module.Version,null);
            if (!File.Exists(fullPath))
            {
                moduleSettings = Activator.CreateInstance(Module.ModuleSettingsType);
            }
            else
            {
                string data = File.ReadAllText(fullPath);
                ms = JsonConvert.DeserializeObject(data, typeof(ModuleSettings)) as ModuleSettings;
                var rawSettingsJson = JsonConvert.SerializeObject(ms.ModuleData);
                moduleSettings = JsonConvert.DeserializeObject(rawSettingsJson, Module.ModuleSettingsType);
            }

            ms.ModuleData = FillObjectFromUI(moduleSettings);
            var json = JsonConvert.SerializeObject(ms);
            File.WriteAllText(fullPath,json);
        }


        private object FillObjectFromUI(object fillingObject, int deepLevel=1,int? collectionIndex=null)
        {
           
            var props = fillingObject.GetType().GetProperties();

            foreach (var info in props)
            {

                var tInfo = info.PropertyType;
                var propTitle = Attribute.GetCustomAttribute(info, typeof(SettingsNameFieldAttribute)) as SettingsNameFieldAttribute;

                if (propTitle != null)
                {
                    
                    if (tInfo == typeof(int) || tInfo == typeof(string) || tInfo == typeof(bool))
                    {
                        var filledVal = GetItemValueFromUI(this, info.Name, deepLevel, collectionIndex);
                        //return null for detecting empty object and non-exist ui-controls for that object
                        if (filledVal == null) 
                            return null;
                        info.SetValue(fillingObject, Convert.ChangeType(filledVal, info.PropertyType), null);
                    }
                    else if (typeof(IList).IsAssignableFrom(info.PropertyType))
                    {
                        var objVal = info.GetValue(fillingObject, null);
                        objVal.GetType().GetMethod("Clear").Invoke(objVal, null);//todo:for now, we clear collection. In future [ID] attribute will be required for list entity for mapping, preventing non-editable fields erase
                        int i = 0;
                        Type listItemType = objVal.GetType().GetGenericArguments().First();
                        do
                        {
                            var collectionItem = Activator.CreateInstance(listItemType);
                            var filedCollectionItemValue = FillObjectFromUI(collectionItem, deepLevel + 1, i);
                            if (filedCollectionItemValue == null) break; //no any new items in collection found on ui
                            objVal.GetType().GetMethod("Add").Invoke(objVal, new[] { filedCollectionItemValue });
                            i++;
                        } while (true);
                        
                    }
                    else //complex object
                    {
                        var filledComplexObject = FillObjectFromUI(Activator.CreateInstance(tInfo),deepLevel+1);
                        info.SetValue(fillingObject, Convert.ChangeType(filledComplexObject, info.PropertyType), null);
                    }
                }
            }
            return fillingObject;
        }

        private object GetItemValueFromUI(Control searchControl,string propName, int deepLevel,int? collectionIndex=null)
        {
            foreach (Control cntrl in searchControl.Controls)
            {
                if (cntrl.AccessibleName == propName + deepLevel + (collectionIndex.HasValue?"_"+collectionIndex.Value:""))
                {
                    var box = cntrl as TextBox;
                    if (box != null)
                    {
                        return box.Text;
                    }
                    var checkbox = cntrl as CheckBox;
                    if (checkbox != null)
                    {
                        return checkbox.Checked;
                    }
                }
                var val = GetItemValueFromUI(cntrl, propName, deepLevel, collectionIndex);
                if (val != null)
                {
                    return val;
                }
            }
            return null;
        }


    }
}
