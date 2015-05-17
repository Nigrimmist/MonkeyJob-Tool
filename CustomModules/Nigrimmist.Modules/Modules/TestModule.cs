using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Nigrimmist.Modules.Modules
{
    

    public class TestModule : ModuleEventBase
    {
        private IClient _client;
        private TestSettings _settings;

        public override void Init(IClient client)
        {
            _client = client;
            _settings = _client.GetSettings<TestSettings>();

            //save empty settings for manual edit
            if (_settings == null)
            {
                _settings = new TestSettings()
                {
                    //DiaryList = new List<test>()
                    //{
                    //    new test()
                    //    {
                    //        DiaryList3 = new List<test2>()
                    //        {
                    //            new test2(){}
                    //        }
                    //    }
                    //}
                };
                _client.SaveSettings(_settings);
            }
        }

        public override string ModuleTitle
        {
            get { return "test"; }
        }

        public override string ModuleDescription
        {
            get { return "Оповещения о новых дискуссиях, u-mail'ах и комментариях на сайте Diary.ru"; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromMinutes(5); }
        }
        

        public override void OnFire(Guid eventToken)
        {
            _client.ShowMessage(eventToken, "test").OnClick(() =>
            {
                _client.ShowMessage(eventToken, "http://diary.ru", answerType: AnswerBehaviourType.OpenLink);
            });
        }

        
    }


    [ModuleSettingsFor(typeof(TestModule))]
    public class TestSettings
    {
        //[SettingsNameField("1 уровень.заголовок коллекции номер 1")]
        //public List<test> DiaryList { get; set; }

        //[SettingsNameField("1 уровень.заголовок коллекции номер 2")]
        //public List<test> DiaryList2 { get; set; }
        //[SettingsNameField("1 уровень. кол-во")]
        //public int Count { get; set; }

        [SettingsNameField("1 уровень.объект")]
        public test2 Test { get; set; }

        public TestSettings()
        {
            Test = new test2();
            //DiaryList = new List<test>();
            //DiaryList2 = new List<test>();
        }
    }

    public class test
    {
        [SettingsNameField("2 уровень.Логин")]
        public string UserName { get; set; }
        [SettingsNameField("2 уровень.Пароль")]
        public string Password { get; set; }
        [SettingsNameField("2 уровень.Проверять U-mail?")]
        public bool CheckUmails { get; set; }
        [SettingsNameField("2 уровень.Проверять комментарии?")]
        public bool CheckNewComments { get; set; }
        [SettingsNameField("2 уровень.Проверять дискуссии?")]
        public bool CheckDiscussions { get; set; }

        [SettingsNameField("2 уровень.заголовок вложенной коллекции")]
        public List<test2> DiaryList3 { get; set; }


        [SettingsNameField("2 уровень.объект")]
        public test2 Test { get; set; }

        public test()
        {
            DiaryList3 = new List<test2>();
            Test = new test2();
        }
    }

    public class test2
    {
        [SettingsNameField("3 уровень.имя")]
        public string name { get; set; }
    }
}
