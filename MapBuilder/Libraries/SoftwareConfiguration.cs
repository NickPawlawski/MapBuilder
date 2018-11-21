using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareConfiguation
{
    public static class SoftwareConfiguration
    {

        #region Variables
        /// <summary>
        /// Dictionary for the configuration file.
        /// </summary>
        private static readonly Dictionary<string, Tuple<string, string[]>> ConfiguratonDictionary = new Dictionary<string, Tuple<string, string[]>>
        {
            //The order of the strings are
            //Name                                        Default            Accecptable values
            {"debug",         new Tuple<string, string[]>("false",           new []{"false","true"})},//Printing of error messages to the log
            

            //Extra hidden values
            
            
            
        };

        
        
        public static bool Debug => bool.Parse(ConfiguratonDictionary["debug"].Item1);

        
        #endregion Variables



        #region Methods

        /// <summary>
        /// Takes key-value pair and sets actual program value.
        /// </summary>
        /// <param name="key">Config dictionary key.</param>
        /// <param name="value">Config dictionary value.</param>
        /// <returns></returns>
        public static int SetValue(string key, string value)
        {
            // No key found in possible config options.
            if (!ConfiguratonDictionary.ContainsKey(key)) return 2;
            ConfiguratonDictionary.TryGetValue(key, out Tuple<string, string[]> configTuple);

            // Invalid or no value found for key.
            if ((configTuple == null || !configTuple.Item2.Contains(value)) && configTuple.Item2.Length > 0) return 1;
            var newConfigTuple = new Tuple<string, string[]>(value, configTuple.Item2);

            ConfiguratonDictionary[key] = newConfigTuple;

            return 0;
        }

        #endregion Methods
    }
}
