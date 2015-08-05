using System;
using System.Collections.Generic;
using System.Windows.Forms;
using  ClsAccessData.Connection;
using ClsAccessData.Data;
using ClsAccessData;

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
        public static ClsConnection oCon=null;
        public static ClsExecuteQuery oExe = null;
        public static ClsStructure clsStruct = null;
        
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