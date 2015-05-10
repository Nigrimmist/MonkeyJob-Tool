﻿using System;
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


        public ModuleSettingsForm()
        {
            InitializeComponent();
        }

        private void ModuleSettingsForm_Load(object sender, EventArgs e)
        {
            string moduleFileName = Module.ModuleSystemName + ".json";
            string fullPath = @"E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\ModuleSettings\" + moduleFileName;
            string data = File.ReadAllText(fullPath);
            var settingsWrapper = JsonConvert.DeserializeObject(data, typeof(ModuleSettings)) as ModuleSettings;
            var rawSettingsJson = JsonConvert.SerializeObject(settingsWrapper.ModuleData);
            var settings = JsonConvert.DeserializeObject(rawSettingsJson, Module.ModuleSettingsType);
            pnlSettings.AccessibleName = Guid.NewGuid().ToString();

            var table = new TableLayoutPanel()
            {
                RowCount = 1,
                ColumnCount = 1,
                AutoSize = true
            };
            pnlSettings.Controls.Add(table);
            BindObject(settings, table);
        }


        public void BindObject(object obj, TableLayoutPanel parentControl)
        {
            var props = obj.GetType().GetProperties();
            parentControl.ColumnStyles.Add(new ColumnStyle());

            foreach (var info in props)
            {
                var tInfo = info.PropertyType;
                var propTitle = Attribute.GetCustomAttribute(info, typeof(SettingsNameFieldAttribute)) as SettingsNameFieldAttribute;

                if (propTitle != null)
                {
                    if (tInfo == typeof (int))
                    {
                        AddControl(propTitle.Label, new TextBox() { Text = info.GetValue(obj, null).ToString() }, info.Name, parentControl);
                    }
                    else if (tInfo == typeof (string))
                    {
                        AddControl(propTitle.Label, new TextBox() { Text = info.GetValue(obj, null).ToString() }, info.Name, parentControl);
                    }
                    else if (tInfo == typeof(bool))
                    {
                        AddControl("", new CheckBox() { Checked = (bool)info.GetValue(obj, null), Text = propTitle.Label }, info.Name, parentControl);
                    }
                    else if (typeof(IList).IsAssignableFrom(info.PropertyType))
                    {
                        int lastPanelWidth = 100;
                        
                        
                        //FlowLayoutPanel collectionPanel = new FlowLayoutPanel
                        //{
                        //    AutoSizeMode = AutoSizeMode.GrowOnly,
                        //    BorderStyle = BorderStyle.FixedSingle,
                        //    AutoSize = true,
                        //    AccessibleName = Guid.NewGuid().ToString(),
                        //    FlowDirection = FlowDirection.TopDown,
                        //    Margin = new Padding() {Left = 10}
                        //};

                        var collectionPanel = new TableLayoutPanel()
                        {
                            RowCount = 1,
                            ColumnCount = 1,
                            AutoSize = true
                        };
                        
                        parentControl.Controls.Add(collectionPanel);
                        DataButton btnAddNewItem = new DataButton();
                        foreach (object item in (IEnumerable)info.GetValue(obj, null))
                        {
                            BindObject(item, collectionPanel);
                            btnAddNewItem.Data = new CloneObjData()
                            {
                                Data = item,
                                DataType = item.GetType()
                            }; //save last for clone by btn click
                        }
                        
                        //Panel btnPanel = new Panel {BorderStyle = BorderStyle.FixedSingle};


                        btnAddNewItem.Text = "Добавить";
                        btnAddNewItem.AccessibleName = parentControl.AccessibleName;
                        btnAddNewItem.Width = 100;
                        btnAddNewItem.Click += (sender, args) =>
                        {
                            var clonedObj = ((DataButton)sender).Data as CloneObjData;

                            if (clonedObj != null)
                            {
                                var jsonObj = JsonConvert.SerializeObject(clonedObj.Data);
                                var materializedObj = JsonConvert.DeserializeObject(jsonObj, clonedObj.DataType);
                                //AddNewItemToForm(materializedObj, collectionPanel);

                            }
                        };
                        //btnPanel.Controls.Add(btnAddNewItem);
                        //btnPanel.Width = lastPanelWidth;
                        //parentControl.Controls.Add(btnPanel);
                        ////btnAddNewItem.Left = parentControl.Left + lastPanelWidth - btnAddNewItem.Width - 20; //aligning by prev panel
                        //btnPanel.Margin = new Padding() {Left = 10};
                        //btnPanel.Height = btnAddNewItem.Height + 2;
                    }
                    else
                    {
                        BindObject(info, parentControl);
                    }

                    
                }
                
            }
            
        }

        //private int AddNewItemToForm(object item, Control collectionPanel)
        //{
        //    FlowLayoutPanel newPanel = new FlowLayoutPanel()
        //    {
        //        AutoSizeMode = AutoSizeMode.GrowOnly,
        //        BorderStyle = BorderStyle.FixedSingle,
        //        AutoSize = true,
        //        AccessibleName = Guid.NewGuid().ToString(),
        //        FlowDirection = FlowDirection.TopDown,
        //        //Width = 300
        //    };
        //    collectionPanel.Controls.Add(newPanel);
            
        //    return newPanel.Width;
        //}

        private void AddControl(string label, Control cntrl, string propName, TableLayoutPanel parentControl)
        {
            Panel tPanel = new Panel();
            tPanel.Width = 310;
            //tPanel.AutoSize = true;
            
            //tPanel.Height = 100;
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
            //cntrl.BackColor = Color.Aqua;
            cntrl.AccessibleName = propName;
            //cntrl.Top = top;
            tPanel.Controls.Add(cntrl);
            parentControl.Controls.Add(tPanel, 1, parentControl.RowCount);
            tPanel.Height = (from Control control in tPanel.Controls select control.Height).Concat(new[] { 0 }).Max();
        }
    }
}
