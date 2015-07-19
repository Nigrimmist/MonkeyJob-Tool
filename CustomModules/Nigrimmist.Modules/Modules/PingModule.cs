using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;

namespace Nigrimmist.Modules.Modules
{
    public class PingModule : ModuleTrayBase
    {
        private ITrayClient _client;
        public override void Init(ITrayClient client)
        {
            _client = client;
            if (_client.GetSettings<PingSettings>() == null)
            {
                _client.SaveSettings(new PingSettings()
                {
                    ShowBorder = true,
                    GreenBorderBefore = 50,
                    YellowBorderAfter = 50,
                    RedBorderAfter = 200,
                    ShowYellowBorder = true,
                    ShowRedBorder = true,
                    ShowGreenBorder = true,
                    PingTo = "www.google.com"
                });
            }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromSeconds(1); }
        }

        public override void OnFire(Guid trayModuleToken)
        {
            var settings = _client.GetSettings<PingSettings>();
            if (settings != null)
            {
                var ping = new System.Net.NetworkInformation.Ping();

                var pingResult = ping.Send(settings.PingTo);
                if (pingResult != null)
                {
                    
                    if (pingResult.Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        long result = pingResult.RoundtripTime;
                        
                        Color? borderColor=null;
                        if (settings.ShowBorder)
                        {
                            if (result <= settings.GreenBorderBefore && settings.ShowGreenBorder)
                                borderColor = Color.Green;
                            else if (result >= settings.RedBorderAfter && settings.ShowRedBorder)
                                borderColor = Color.DarkRed;
                            else if (result > settings.GreenBorderBefore && result < settings.RedBorderAfter && settings.ShowYellowBorder)
                                borderColor = Color.Yellow;
                        }

                        _client.UpdateTrayText(trayModuleToken, result.ToString(), Color.White, Color.Black, 6, "Tahoma", borderColor);
                    }
                    else
                    {
                        _client.UpdateTrayText(trayModuleToken,"---", Color.White, Color.Black, 6, "Tahoma", settings.ShowBorder?Color.Red:(Color?)null);
                    }
                }
            }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAACt0lEQVRYhc2Xv2tUQRDHP4XCFQemEVREIkSwsLjCQvCwshBUEBWSIsUVQQ4MohhFIXDFFQEDpkhxYIRDLFIo+B94xQki8QfyihQKJ6QIEiTFCQ954FnsDDe3997dux+RDCy8993vzM7O7M7uQnqZAGaAChAADSCS1hCsIpzMAHb7ykngMfAVaKVsH4EHwIlRBj4AzAHbCYMEwKq0zQTONlAQWwNJFqgnGG0CszHOFnHpiNOpi82hBg+M4QjI99C9afQiOiOTygl/8BVgwfwvppjAkuHPA9W0TmS8wXV1hwbbEqxX26IzXbpbFKuRsCbuEZ+/vWi34mavq31TwlaVGehqrg7YdkV3x2ANwb75UbgvHX+A64IdBX4K/jQuZH3kmeh+p533WROFohIngQ8CvjAGLhny9BAOzBn984IdBF4L9hY4DC78k9LsCs0afOBC4tm1pXliRLvjlzJuv6ssCKayaPrjuCWPq7pLnp1SEreCy8lFXFgi8z8l329ESbl509cCcsBp+V4X7rrpyyVwXwKckZ8qsIwrPKEMWjLOAJylXSFXDXdNdFvAOeHmDbeSwM1pOOq4Pb8LvBLPItz+/0KnbAgeGm6I2+8bHjcw3IpERbk1S5zxQmRDVvCMFntw/VNy3vRNSXT0/5olZiQ09rApC+bfbrIjcjV9+2MLwj4oRKeAz7i8PDfEy7TzdWMIBwpG/4LBdXu+A44p+EjA38BVwY4DvwR/MoQDWjN+AIcEmwb+Cn7Hku1xHDCe43iH7uNYr2hdxzHAQ/7fheR2XMgydF4iQ7qvZI2Uzbdhr2T1uNmrHDFORLhSXDbKd5MUjeh6Uv6KN3jfm7F1QpV0BiH9r+XKbeLK+ECDWycC4vPXBK7E6BQY08NEpd/TrIY785eB9wmcoZ9mVoZ5nH5iDI9TX/bkef4PdYqs4+fYLn8AAAAASUVORK5CYII="; }
        }

        public override string ModuleTitle
        {
            get { return "Ping"; }
        }

        public override string ModuleDescription
        {
            get { return "Выводит в трей качество текущего интернет-соединения на основе пинга какого-либо ip или урла посредством указания кол-ва миллисекунд и цвета бордера иконки (настраивается).";}
        }

        public override string TrayIconIn64Base
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAACt0lEQVRYhc2Xv2tUQRDHP4XCFQemEVREIkSwsLjCQvCwshBUEBWSIsUVQQ4MohhFIXDFFQEDpkhxYIRDLFIo+B94xQki8QfyihQKJ6QIEiTFCQ954FnsDDe3997dux+RDCy8993vzM7O7M7uQnqZAGaAChAADSCS1hCsIpzMAHb7ykngMfAVaKVsH4EHwIlRBj4AzAHbCYMEwKq0zQTONlAQWwNJFqgnGG0CszHOFnHpiNOpi82hBg+M4QjI99C9afQiOiOTygl/8BVgwfwvppjAkuHPA9W0TmS8wXV1hwbbEqxX26IzXbpbFKuRsCbuEZ+/vWi34mavq31TwlaVGehqrg7YdkV3x2ANwb75UbgvHX+A64IdBX4K/jQuZH3kmeh+p533WROFohIngQ8CvjAGLhny9BAOzBn984IdBF4L9hY4DC78k9LsCs0afOBC4tm1pXliRLvjlzJuv6ssCKayaPrjuCWPq7pLnp1SEreCy8lFXFgi8z8l329ESbl509cCcsBp+V4X7rrpyyVwXwKckZ8qsIwrPKEMWjLOAJylXSFXDXdNdFvAOeHmDbeSwM1pOOq4Pb8LvBLPItz+/0KnbAgeGm6I2+8bHjcw3IpERbk1S5zxQmRDVvCMFntw/VNy3vRNSXT0/5olZiQ09rApC+bfbrIjcjV9+2MLwj4oRKeAz7i8PDfEy7TzdWMIBwpG/4LBdXu+A44p+EjA38BVwY4DvwR/MoQDWjN+AIcEmwb+Cn7Hku1xHDCe43iH7uNYr2hdxzHAQ/7fheR2XMgydF4iQ7qvZI2Uzbdhr2T1uNmrHDFORLhSXDbKd5MUjeh6Uv6KN3jfm7F1QpV0BiH9r+XKbeLK+ECDWycC4vPXBK7E6BQY08NEpd/TrIY785eB9wmcoZ9mVoZ5nH5iDI9TX/bkef4PdYqs4+fYLn8AAAAASUVORK5CYII="; }
        }

        [ModuleSettingsFor(typeof(PingModule))]
        public class PingSettings
        {

            [SettingsNameField("Url или IP для пинга")]
            public string PingTo { get; set; }

            [SettingsNameField("Показывать цветные рамку")]
            public bool ShowBorder { get; set; }

            [SettingsNameField("Показывать зелёную рамку")]
            public bool ShowGreenBorder { get; set; }
            [SettingsNameField("Зелёный до мс :")]
            public int GreenBorderBefore { get; set; }

            [SettingsNameField("Показывать жёлтую рамку")]
            public bool ShowYellowBorder { get; set; }
            [SettingsNameField("Жёлтый начиная с мс")]
            public int YellowBorderAfter { get; set; }

            [SettingsNameField("Показывать красную рамку")]
            public bool ShowRedBorder { get; set; }
            [SettingsNameField("Красный начиная с мс")]
            public int RedBorderAfter { get; set; }
        }
    }
}
