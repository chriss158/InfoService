using System;
using InfoService;

namespace InfoServicePluginLoader
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            InfoService.InfoServiceCore plugin = new InfoServiceCore();
            plugin.ShowPlugin();
        }
    }
}
