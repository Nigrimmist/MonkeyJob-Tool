﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCore.Entities
{
    public class IntegrationClientSettings
    {
        public int InstanceCount { get; set; }

        public IntegrationClientSettings()
        {
            InstanceCount = 1;
        }
    }
}
