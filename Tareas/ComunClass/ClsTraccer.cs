using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Tareas.ComunClass
{
    /// <summary>
    /// Clase de gestión de errores.
    /// </summary>
    class ClsTraccer
    {
        /// <summary>
        /// Guarda un texto en un Fichero de Texto.
        /// </summary>
        public static bool bDebug = true;

        /// <summary>
        /// Guarda el error en un fichero de log.
        /// </summary>
        /// <param name="sTexto">Descripción del error.</param>
        private static void GuardarTraza(String sTexto )
        {
            String fic  ;
            fic = Directory.GetCurrentDirectory() + DateTime.Now.Year+DateTime.Now.Month+DateTime.Now.Day + "-TracerSql"  + ".log";
            StreamWriter sw = new StreamWriter(fic, true);
            sw.WriteLine(DateTime.Now.ToString() +" - "+ sTexto);
            sw.Close();
        }

        /// <summary>
        /// Gestiona los mensaje de error de la aplicación.
        /// </summary>
        /// <param name="ex">Excepción producida por la aplicación</param>
        /// <param name="sStringError">"Cadena de error."</param>
        /// <param name="sNameFunction">"Nombre de la función que produce el error."</param>
        public static void MessageError(Exception ex, String sStringError, String sNameFunction)
        {
            String sError = "";
            if (!String.IsNullOrEmpty(sStringError))
                sError = sStringError;
            else
            {
                if (ex != null && !bDebug)
                    sError = ex.Message;
                else if (ex != null && bDebug)
                    sError = ex.ToString();
            }
                
                
            if (bDebug)
            {
                ClsTraccer.GuardarTraza(sNameFunction + " - " + sError); 
            }
            ShowMessage(sError);
        }

        /// <summary>
        /// Gestiona los mensaje de error de la aplicación.
        /// </summary>
        /// <param name="sStringError">"Cadena de error."</param>
        /// <param name="sNameFunction">"Nombre de la función que produce el error."</param>
        public static void MessageError(String sStringError, String sNameFunction)
        {
            MessageError(null, sStringError, sNameFunction);
           
        }

        /// <summary>
        /// Gestiona los mensaje de error de la aplicación.
        /// </summary>
        /// <param name="ex">Excepción producida por la aplicación</param>
        /// <param name="sNameFunction">"Nombre de la función que produce el error."</param>
        public static void MessageError(Exception ex, String sNameFunction)
        {
            MessageError(ex, "", sNameFunction);

        }

        /// <summary>
        /// Muestra el mensaje de error producido por la aplicación.
        /// </summary>
        /// <param name="sError"></param>
        private static void ShowMessage(String sError)
        {
            MessageBox.Show(sError,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }
}
