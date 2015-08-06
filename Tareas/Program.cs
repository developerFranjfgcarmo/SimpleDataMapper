using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SimpleDataMapper;
using SimpleDataMapper.Connector;
using SimpleDataMapper.Data;

namespace Tareas
{
    /// <summary>
    /// Clase de inicio de la aplicación.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Almacena la conexión a la base datos.
        /// </summary>
        public static Connection oCon=null;
        public static ExecuteQuery oExe = null;
        public static Structure clsStruct = null;
        
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmAcceso());
            
        }
    }
}