﻿using System.Collections.Generic;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;


namespace SmartAssHandlerLib
{
    public class VariousHandlersRegister : ModuleRegister
    {
        public List<ModuleHandlerBase> GetModules()
        {
            return new List<ModuleHandlerBase>()
            {
                new SmartAssStuffHandlerHandlerBase(),
                new YesNoHandlerHandlerBase(),
                new FckinWeatherModuleHandlerBase()
            };
        }

        public override string ByAuthor
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
