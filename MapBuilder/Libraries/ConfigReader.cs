using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareConfiguation
{
    /// <summary>
    /// Handles reading of config file.
    /// </summary>
    class ConfigReader
    {
        #region Variables

        private const string _filename = "Config.cfg";
        private const string _foldername = "SoftwareConfiguration";
        private bool _attempt;
        private static ConfigReader _configReader;

        #endregion Variables

        #region Properties

        public string FolderName => _foldername;

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Class singleton.
        /// </summary>
        /// <returns>Current instance of class.</returns>
        public static ConfigReader GetConfigReader()
        {
            return _configReader ?? (_configReader = new ConfigReader(false));
        }

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="forceWrite">Force new config to be written even if already exists.</param>
        private ConfigReader(bool forceWrite)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            folder += "\\"+_foldername;

            // Check if new config (with defaults) should be written.
            if (!Directory.Exists(folder) || forceWrite)
            {
                Directory.CreateDirectory(folder);
                CreateConfig(folder);
            }

            ReadConfig(folder);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates new config file at specified path.
        /// </summary>
        /// <param name="path">Path to create config at.</param>
        private static void CreateConfig(string path)
        {
            try
            {
                using (var writer = new StreamWriter(path + "\\" + _filename))
                {
                    writer.WriteLine("debug=false");
                }
            }
            catch (FileNotFoundException fnf)
            {
                Reporter.Reporter.WriteContent(@"FileNotFound: " + _filename + @" " + fnf,0);
            }
            catch (Exception ex) // General exception.
            {
                Reporter.Reporter.WriteContent(@"Error: " + ex,0);
            }
        }


        /// <summary>
        /// Reads config file at specified path.
        /// </summary>
        /// <param name="path">Path to locate config at.</param>
        private void ReadConfig(string path)
        {
            try
            {
                using (var reader = new StreamReader(path + "\\" + _filename))
                {
                    // Config file found.
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] split = line.Split('=');

                        switch (SoftwareConfiguration.SetValue(split[0].Trim().ToLower(), split[1].Trim()))
                        {
                            case 0:
                                //It Worked!
                                break;
                            case 1:
                                // Unknown Value.
                                Reporter.Reporter.WriteContent("Value Not Accecptable: " + split[1].Trim(), 0);
                                break;
                            case 2:
                                // Invalid Key.
                                Reporter.Reporter.WriteContent("Key Does Not Exist: " + split[0], 0);
                                break;
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                // Error occured. Try to re-create the conf file and read it. If it fails again, show message.
                CreateConfig(path);
                if (!_attempt)
                {
                    _attempt = true;
                    ReadConfig(path);
                }
                else
                {
                    Reporter.Reporter.WriteContent(@"There was an error creating or reading the configuration file: " + e,0);
                }
            }
            catch (Exception ex)
            {
                Reporter.Reporter.WriteContent(@"Unknown Exception has been thrown: " + ex, 0);
            }
        }

        #endregion Methods
    }
}
