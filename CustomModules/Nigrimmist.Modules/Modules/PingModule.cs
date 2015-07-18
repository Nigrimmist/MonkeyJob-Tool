using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace Nigrimmist.Modules.Modules
{
    public class PingModule : ModuleTrayBase
    {
        private ITrayClient _client;
        public override void Init(ITrayClient client)
        {
            _client = client;
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromSeconds(1); }
        }

        public override void OnFire(Guid trayModuleToken)
        {
            var ping = new System.Net.NetworkInformation.Ping();

            var result = ping.Send("www.google.com");
            if (result != null)
            {
                if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    _client.UpdateTrayText(trayModuleToken,result.RoundtripTime.ToString());
                    _client.UpdateTrayColor(trayModuleToken,Color.Green);
                }
                else
                {
                    _client.UpdateTrayColor(trayModuleToken,Color.Red);
                }
            }
        }

        public override string TrayIconIn64Base
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAACt0lEQVRYhc2Xv2tUQRDHP4XCFQemEVREIkSwsLjCQvCwshBUEBWSIsUVQQ4MohhFIXDFFQEDpkhxYIRDLFIo+B94xQki8QfyihQKJ6QIEiTFCQ954FnsDDe3997dux+RDCy8993vzM7O7M7uQnqZAGaAChAADSCS1hCsIpzMAHb7ykngMfAVaKVsH4EHwIlRBj4AzAHbCYMEwKq0zQTONlAQWwNJFqgnGG0CszHOFnHpiNOpi82hBg+M4QjI99C9afQiOiOTygl/8BVgwfwvppjAkuHPA9W0TmS8wXV1hwbbEqxX26IzXbpbFKuRsCbuEZ+/vWi34mavq31TwlaVGehqrg7YdkV3x2ANwb75UbgvHX+A64IdBX4K/jQuZH3kmeh+p533WROFohIngQ8CvjAGLhny9BAOzBn984IdBF4L9hY4DC78k9LsCs0afOBC4tm1pXliRLvjlzJuv6ssCKayaPrjuCWPq7pLnp1SEreCy8lFXFgi8z8l329ESbl509cCcsBp+V4X7rrpyyVwXwKckZ8qsIwrPKEMWjLOAJylXSFXDXdNdFvAOeHmDbeSwM1pOOq4Pb8LvBLPItz+/0KnbAgeGm6I2+8bHjcw3IpERbk1S5zxQmRDVvCMFntw/VNy3vRNSXT0/5olZiQ09rApC+bfbrIjcjV9+2MLwj4oRKeAz7i8PDfEy7TzdWMIBwpG/4LBdXu+A44p+EjA38BVwY4DvwR/MoQDWjN+AIcEmwb+Cn7Hku1xHDCe43iH7uNYr2hdxzHAQ/7fheR2XMgydF4iQ7qvZI2Uzbdhr2T1uNmrHDFORLhSXDbKd5MUjeh6Uv6KN3jfm7F1QpV0BiH9r+XKbeLK+ECDWycC4vPXBK7E6BQY08NEpd/TrIY785eB9wmcoZ9mVoZ5nH5iDI9TX/bkef4PdYqs4+fYLn8AAAAASUVORK5CYII="; }
        }


    }
}
