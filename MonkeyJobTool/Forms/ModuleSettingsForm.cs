using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCore.Entities;
using MonkeyJobTool.Controls.Autocomplete;
using MonkeyJobTool.Controls.Settings;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Entities.Json;
using MonkeyJobTool.Helpers;
using MonkeyJobTool.Managers;
using Newtonsoft.Json;


namespace MonkeyJobTool.Forms
{
    public partial class ModuleSettingsForm : Form
    {
        public ComponentInfoBase Module { get; set; }

        public ModuleSettingsForm()
        {
            InitializeComponent();
        }

        private void ModuleSettingsForm_Load(object sender, EventArgs e)
        {
            string fullPath = Module.GetSettingFileFullPath();
            object moduleSettings;
            if (!File.Exists(fullPath))
            {
                moduleSettings = Activator.CreateInstance(Module.SettingsType);
            }
            else
            {
                string data = File.ReadAllText(fullPath);
                var settingsWrapper = JsonConvert.DeserializeObject(data, typeof (ModuleSettings)) as ModuleSettings;
                if (settingsWrapper.ModuleData != null)
                {
                    var rawSettingsJson = JsonConvert.SerializeObject(settingsWrapper.ModuleData);
                    moduleSettings = JsonConvert.DeserializeObject(rawSettingsJson, Module.SettingsType);
                }
                else
                    moduleSettings = Activator.CreateInstance(Module.SettingsType);
            }
            var table = new TableLayoutPanel()
            {
                AutoSize = true,
            };
            pnlSettings.Controls.Add(table);
            BindObject("",moduleSettings, table);
        }

        

        public void BindObject(string parentName, object obj, TableLayoutPanel parentControl, int deepLevel=1, int? collectionIndex = null)
        {
            var objType = obj.GetType();
            if (objType == typeof (string))
            {
                AddControl("", new DataTextBox() { Text = obj.ToString() }, parentName, parentControl, deepLevel, collectionIndex);
            }
            else
            {
                var props = objType.GetProperties();

                foreach (var info in props)
                {

                    var tInfo = info.PropertyType;
                    var propTitle = Attribute.GetCustomAttribute(info, typeof(SettingsNameFieldAttribute)) as SettingsNameFieldAttribute;

                    if (propTitle != null)
                    {
                        var objVal = info.GetValue(obj, null);
                        if (tInfo == typeof(int))
                        {
                            AddControl(propTitle.Label, new DataTextBox() { Text = objVal != null ? objVal.ToString() : "" }, info.Name, parentControl, deepLevel, collectionIndex);
                        }
                        else if (tInfo == typeof(string))
                        {
                            AddControl(propTitle.Label, new DataTextBox() { Text = objVal != null ? objVal.ToString() : "" }, info.Name, parentControl, deepLevel, collectionIndex);
                        }
                        else if (tInfo == typeof(bool))
                        {
                            AddControl("", new DataCheckBox() { Checked = objVal != null ? (bool)objVal : false, Text = propTitle.Label }, info.Name, parentControl, deepLevel, collectionIndex);
                        }
                        else if (typeof(IList).IsAssignableFrom(info.PropertyType))
                        {
                            var collectionPanel = new DataTableLayoutPanel()
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

                            if (objVal == null)
                            {
                                objVal = Activator.CreateInstance(tInfo.UnderlyingSystemType);
                            }

                            var itemList = ((IEnumerable)objVal).Cast<object>().ToList();

                            if (!itemList.Any())
                            {
                                Type listItemType = objVal.GetType().GetGenericArguments().First();
                                object objToAdd = null;
                                //add one for user input

                                if (listItemType == typeof(string))
                                    objToAdd = ""; //because string haven't constructor and cannot be inited through CreateInstance
                                else
                                    objToAdd = Activator.CreateInstance(listItemType);

                                itemList.Add(objToAdd);
                            }

                            collectionPanel.ChildCount = itemList.Count;
                            for (int i = 0; i < itemList.Count; i++)
                            {
                                var item = itemList[i];
                                btnAddNewItem.Data = new CloneObjData()
                                {
                                    Data = item,
                                    DataType = item.GetType(),
                                    ParentPropName = info.Name
                                }; //save last for clone by btn click

                                AddItemCollectionToUI(info.Name,item, collectionPanel, deepLevel, i);
                            }


                            btnAddNewItem.Click += (sender, args) =>
                            {
                                var clonedObj = ((DataButton)sender).Data as CloneObjData;

                                if (clonedObj != null)
                                {
                                    var jsonObj = JsonConvert.SerializeObject(clonedObj.Data);
                                    var materializedObj = JsonConvert.DeserializeObject(jsonObj, clonedObj.DataType);
                                    var colPanel = (DataTableLayoutPanel)((DataButton)sender).ParentPanel;
                                    colPanel.ChildCount++;
                                    AddItemCollectionToUI(clonedObj.ParentPropName, materializedObj, colPanel, ((DataButton)sender).DeepLevel, colPanel.ChildCount - 1);
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
                            BindObject(info.Name,objVal, collectionPanel, deepLevel + 1);
                        }
                    }


                }
            }
            
            
            
        }

        private void AddItemCollectionToUI(string parentPropName,object item, TableLayoutPanel parentControl, int deepLevel, int collectionIndex)
        {
            var collectionItemPanel = new TableLayoutPanel()
            {
                AutoSize = true
            };
            AddControlToNewPanelRow(parentControl, collectionItemPanel, 0);
            BindObject(parentPropName,item, collectionItemPanel, deepLevel+1, collectionIndex);
            AddRemoveBtn(collectionItemPanel, parentControl, deepLevel, collectionIndex);
        }

        private void AddRemoveBtn(TableLayoutPanel addToControl, TableLayoutPanel parentControl, int deepLevel, int collectionIndex)
        {
            DataButton btnRemoveItem = new DataButton()
            {
                BackColor = Color.Moccasin,
                FlatStyle = FlatStyle.Popup,
                UseVisualStyleBackColor = false,
                Text = "Удалить",
                DeepLevel = deepLevel,
                CollectionIndex = collectionIndex
            };
            btnRemoveItem.ParentPanel = addToControl;
            btnRemoveItem.ParentParentPanel = parentControl;
            btnRemoveItem.Click += OnBtnRemoveItemOnClick;
            AddControlToNewPanelRow(addToControl, btnRemoveItem, 0);
        }

        private void OnBtnRemoveItemOnClick(object sender, EventArgs args)
        {
            var parentContainer = ((DataButton) sender).ParentPanel;
            var collectionPanel = ((DataButton) sender).ParentParentPanel;
            var deepLevel = ((DataButton)sender).DeepLevel;
            var collectionIndex = ((DataButton)sender).CollectionIndex;
            collectionPanel.Controls.Remove(parentContainer);
            
            //забрать collectionPanel.itemsCount

            //reassign collection indexes for correct data grabbing on save
            ReassignCollectionIndexes(collectionPanel as DataTableLayoutPanel, deepLevel+1, collectionIndex+1);
        }

        private void ReassignCollectionIndexes(DataTableLayoutPanel collectionPanel,int searchDeepLevel, int fromIndex)
        {
            int collectionLength = collectionPanel.ChildCount;
            for (var i = fromIndex; i < collectionLength; i++)
            {
                foreach (IUIDataItem found in FindControlsByCollectionIndex(collectionPanel, searchDeepLevel, i))
                {
                    found.CollectionIndex--;
                }
            }
            collectionPanel.ChildCount--;
        }

        private Control GetTitleControl(string title)
        {
            return new RichTextLabelResizeable()
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
                var lbl = new RichTextLabelResizeable()
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
            var uiDataItem = (cntrl as IUIDataItem);
            
            uiDataItem.PropName = propName;
            uiDataItem.DeepLevel = deepLevel;
            uiDataItem.CollectionIndex = collectionIndex;
            //cntrl.AccessibleName = propName + deepLevel + (collectionIndex!=null?"_"+collectionIndex:"");
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
            try
            {
                string fullPath = Module.GetSettingFileFullPath();
                object moduleSettings;
                ModuleSettings ms=null;
                if (!File.Exists(fullPath))
                {
                    moduleSettings = Activator.CreateInstance(Module.SettingsType);
                }
                else
                {
                    string data = File.ReadAllText(fullPath);
                    ms = JsonConvert.DeserializeObject(data, typeof (ModuleSettings)) as ModuleSettings;

                    if (ms.ModuleData != null)
                    {
                        var rawSettingsJson = JsonConvert.SerializeObject(ms.ModuleData);
                        moduleSettings = JsonConvert.DeserializeObject(rawSettingsJson, Module.SettingsType);
                    }
                    else
                        moduleSettings = Activator.CreateInstance(Module.SettingsType);
                }

                var serviceData = ms != null ? ms.ServiceData : null;

                if (Module.ModuleType == ModuleType.IntegrationClient)
                    if (serviceData == null) serviceData = new ClientSettings();

                ms = new ModuleSettings(Module.Version, Module.SettingsModuleVersion, FillObjectFromUI("",moduleSettings),serviceData);
                
                var json = JsonConvert.SerializeObject(ms, Formatting.Indented);
                File.WriteAllText(fullPath, json);
                this.Close();
            }
            catch(Exception ex)
            {
                LogManager.Error(ex,"save config from ui");
            }
        }


        private object FillObjectFromUI(string parentPropName,object fillingObject, int deepLevel=1,int? collectionIndex=null)
        {
            if (fillingObject.GetType() == typeof (string))
            {
                return GetItemValueFromUI(this, parentPropName, deepLevel, collectionIndex);
            }
            else
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
                                object collectionItem = null;
                                if (listItemType == typeof(string))
                                    collectionItem = "";
                                else
                                    collectionItem = Activator.CreateInstance(listItemType);
                                var filedCollectionItemValue = FillObjectFromUI(info.Name,collectionItem, deepLevel + 1, i);
                                if (filedCollectionItemValue == null) break; //no any new items in collection found on ui
                                objVal.GetType().GetMethod("Add").Invoke(objVal, new[] { filedCollectionItemValue });
                                i++;
                            } while (true);

                        }
                        else //complex object
                        {
                            var filledComplexObject = FillObjectFromUI(info.Name,Activator.CreateInstance(tInfo), deepLevel + 1);
                            info.SetValue(fillingObject, Convert.ChangeType(filledComplexObject, info.PropertyType), null);
                        }
                    }
                }
            }
            
            return fillingObject;
        }

        private object GetItemValueFromUI(Control contentControl, string propName, int deepLevel, int? collectionIndex = null)
        {
            foreach (Control cntrl in contentControl.Controls)
            {
                var uiSettingDataControl = cntrl as IUIDataItem;
                if (uiSettingDataControl != null)
                {
                    if (uiSettingDataControl.PropName == propName && uiSettingDataControl.DeepLevel == deepLevel)
                    {
                        if (!collectionIndex.HasValue || uiSettingDataControl.CollectionIndex==collectionIndex)
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

        private IEnumerable<IUIDataItem> FindControlsByCollectionIndex(Control contentControl, int deepLevel, int collectionIndex)
        {
            foreach (Control cntrl in contentControl.Controls)
            {
                var uiDataCntrl = (cntrl as IUIDataItem);
                if ( uiDataCntrl != null && uiDataCntrl.DeepLevel==deepLevel && uiDataCntrl.CollectionIndex==collectionIndex)
                {
                    yield return uiDataCntrl;
                    break;
                }
                else
                {
                    foreach (var control in FindControlsByCollectionIndex(cntrl, deepLevel, collectionIndex))
                    {
                        yield return control;
                    }
                }
            }
        }
    }
}
