using System;

namespace NEOSimulation
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var win = new Simulation())
                win.Run();
        }
    }
}