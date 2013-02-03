using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace LeapPaint
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {





            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //LeapWrapper.LeapListerner listener = new LeapWrapper.LeapListerner();
                Application.Run(new LeapPaintMain());
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }


}
