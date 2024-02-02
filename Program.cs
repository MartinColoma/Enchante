using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enchante
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Syncfusion Licensing
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA4MDI5NEAzMjM0MmUzMDJlMzBXSzN0UXY0aU1nMDJxTWQzdzR5UmIxa09HYWZNdHArTHkwVktvUkhLNHRrPQ==");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Enchante());
        }
    }
}
