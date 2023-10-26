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
    /// Класс для работы с конфигурацией приложения
    /// </summary>
    public class Configurator
    {
        private ConfigJson _mainConfig;

        /// <summary>
        /// Свойство для получения конфига
        /// </summary>
        /// <exception cref="Exception">плохой json-формат</exception>
        public ConfigJson MainConfig
        {
            get => _mainConfig;
        }

        public Configurator(string configPath = "config/config.json")
        {
            _mainConfig = ParseConfig(configPath);
        }

        /// <summary>
        /// Метод для получения главного конфига
        /// </summary>
        /// <returns>ConfigJson класс</returns>
        /// <exception cref="Exception">плохой json-формат</exception>
        ConfigJson ParseConfig(string path)
        {
            return JsonConvert.DeserializeObject<ConfigJson>(File.ReadAllText(path))
                ?? throw new Exception("bad config.json parse");
        }
    }
}
