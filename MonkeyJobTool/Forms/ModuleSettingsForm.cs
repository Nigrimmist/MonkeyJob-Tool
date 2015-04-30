using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCore.Entities;

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

        }


        public void BindObject(object obj)
        {
            var props = obj.GetType().GetProperties();

            foreach (var info in props)
            {
                var tInfo = info.GetType();
                var propTitle = Attribute.GetCustomAttribute(tInfo, typeof (SettingsNameFieldAttribute)) as SettingsNameFieldAttribute;

                if (propTitle != null)
                {
                    if (tInfo == typeof (int))
                    {
                        AddControl(propTitle.Label, new TextBox() {Text = info.GetValue(obj, null).ToString()}, info.Name);
                    }
                    else if (tInfo == typeof (string))
                    {
                        AddControl(propTitle.Label, new TextBox() { Text = info.GetValue(obj, null).ToString() }, info.Name);
                    }
                    else if (tInfo == typeof (bool))
                    {
                        AddControl(propTitle.Label, new CheckBox() { Checked = (bool)info.GetValue(obj, null) }, info.Name);
                    }
                    else
                    {
                        BindObject(info);
                    }
                }
                
            }
        }

        private void AddControl(string label, Control cntrl, string propName)
        {
            this.Controls.Add(new Label()
            {
                Text = label,
            });

            cntrl.AccessibleName = propName;

            this.Controls.Add(cntrl);
        }
    }
}
