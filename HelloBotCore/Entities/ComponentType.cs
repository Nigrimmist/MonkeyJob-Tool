using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public enum ComponentType
    {
        Handler=1,
        Event=2,
        Tray=3,
        IntegrationClient=4
    }

    public enum BaseComponentType
    {
        Modules=1,
        Clients =2
    }

    public static class ModuleTypeExtensions
    {
        public static string ToReadableName(this ComponentType type)
        {
            switch (type)
            {
                case ComponentType.Handler:
                    return "Команда";
                case ComponentType.Event:
                    return "Интервальный";
                case ComponentType.Tray:
                    return "Трей";
                case ComponentType.IntegrationClient:
                    return "Клиент";
                default:
                    return "Unknown type";                    
            }
        }

        public static string ToParentReadableName(this ComponentType type)
        {
            switch (type.ToBaseType())
            {
                case BaseComponentType.Modules:                
                    return "Модуль";
                case BaseComponentType.Clients:
                    return "Клиент";
                default:
                    return "Unknown type";
            }
        }
        public static BaseComponentType ToBaseType(this ComponentType type)
        {
            switch (type)
            {
                case ComponentType.Handler:
                case ComponentType.Event:
                case ComponentType.Tray:
                    return BaseComponentType.Modules;
                case ComponentType.IntegrationClient:
                    return BaseComponentType.Clients;
                default:
                    throw new NotImplementedException("BaseModuleType.ModuleType is unknown");
            }
        }
    }
}