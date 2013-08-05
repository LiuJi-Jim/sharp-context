using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace App {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormMain());
            //Application.Run(new LiquidParticles());
            //Application.Run(new TestML());
            //Application.Run(new Demo());
            //Application.Run(new Property());
            Application.Run(new ConfigTool());
        }
    }
}
