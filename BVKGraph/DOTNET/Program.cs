using System;
using System.Windows.Forms;
// ReSharper disable All


namespace BVKGraph
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Подпись на событие глобального перехвата исключений
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException; 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new LoadForm());
            Application.Run(new LogoForm());
            Application.Run(new WorkForm());
        }
        //Глобальный обрабочик исключений
        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex == null) return;
            MessageBox.Show(ex.Message);
        }
    }
}
