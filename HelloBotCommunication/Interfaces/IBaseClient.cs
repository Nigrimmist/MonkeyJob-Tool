﻿namespace HelloBotCommunication.Interfaces
{
    public interface IBaseClient
    {
        /// <summary>
        /// Save you custom setting object to storage
        /// </summary>
        /// <param name="serializableSettingObject"></param>
        void SaveSettings(object serializableSettingObject);

        /// <summary>
        /// Return your settings if exist.
        /// </summary>
        /// <typeparam name="T">Type of your settings</typeparam>
        T GetSettings<T>() where T : class;

        /// <summary>
        /// Determine client ui language
        /// </summary>
        ClientLanguage ClientLanguage { get; }

        /// <summary>
        /// Determine bot version
        /// </summary>
        double BotVersion { get; }

        /// <summary>
        /// Determine ui app version
        /// </summary>
        double UiClientVersion { get; }
    }
}