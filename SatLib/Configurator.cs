using Newtonsoft.Json;
using SatLib.JsonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatLib
{
    /// <summary>
    /// Статический класс для работы с конфигурацией приложения
    /// </summary>
    public static class Configurator
    {
        private static ConfigJson? _mainConfig;

        /// <summary>
        /// Статическое свойство для получения конфига
        /// </summary>
        /// <exception cref="Exception">плохой json-формат</exception>
        public static ConfigJson MainConfig
        {
            get => _mainConfig ??= ParseConfig();
        }

        /// <summary>
        /// Метод для получения главного конфига
        /// </summary>
        /// <returns>ConfigJson класс</returns>
        /// <exception cref="Exception">плохой json-формат</exception>
        static ConfigJson ParseConfig()
        {
            return JsonConvert.DeserializeObject<ConfigJson>(File.ReadAllText("config/config.json"))
                ?? throw new Exception("bad config.json parse");
        }
    }
}
