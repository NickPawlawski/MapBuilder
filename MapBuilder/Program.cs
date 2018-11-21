using System;
using System.Windows.Forms;


namespace MapBuilder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SoftwareConfiguation.ConfigReader cr = SoftwareConfiguation.ConfigReader.GetConfigReader();
            Reporter.Reporter.StartReporter("MapBuilder");
            

            Application.Run(new MapBuilder());
        }
    }
}
