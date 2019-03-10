using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public enum ModuleType
    {
        Handler=1,
        Event=2,
        Tray=3,
        IntegrationClient=4
    }

    public enum BaseModuleType
    {
        Modules=1,
        Clients =2
    }

    public static class ModuleTypeExtensions
    {
        public static string ToReadableName(this ModuleType type)
        {
            switch (type)
            {
                case ModuleType.Handler:
                    return "Команда";
                case ModuleType.Event:
                    return "Интервальный";
                case ModuleType.Tray:
                    return "Трей";
                case ModuleType.IntegrationClient:
                    return "Клиент";
                default:
                    return "Unknown type";                    
            }
        }

        public static string ToParentReadableName(this ModuleType type)
        {
            switch (type.ToBaseType())
            {
                case BaseModuleType.Modules:                
                    return "Модуль";
                case BaseModuleType.Clients:
                    return "Клиент";
                default:
                    return "Unknown type";
            }
        }
        public static BaseModuleType ToBaseType(this ModuleType type)
        {
            switch (type)
            {
                case ModuleType.Handler:
                case ModuleType.Event:
                case ModuleType.Tray:
                    return BaseModuleType.Modules;
                case ModuleType.IntegrationClient:
                    return BaseModuleType.Clients;
                default:
                    throw new NotImplementedException("BaseModuleType.ModuleType is unknown");
            }
        }
    }
}