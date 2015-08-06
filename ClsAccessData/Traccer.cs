using System;
using System.IO;

//http://www.daveoncsharp.com/2009/09/create-a-logger-using-the-trace-listener-in-csharp/
//todo: Mirar a leer a través de la clase tracer.
//http://www.google.es/#pq=como+usar+trace+c%23&hl=es&cp=7&gs_id=16&xhr=t&q=ejemplo+trace+c%23&pf=p&sclient=psy-ab&source=hp&pbx=1&oq=ejemplo+trace+c%23&aq=f&aqi=&aql=&gs_sm=&gs_upl=&fp=1&biw=1440&bih=799&bav=on.2,or.r_gc.r_pw.r_cp.,cf.osb&cad=b
//http://www.google.es/#hl=es&cp=8&gs_id=1e&xhr=t&q=trace+c%23&pf=p&sclient=psy-ab&source=hp&pbx=1&oq=trace+c%23&aq=0&aqi=g1&aql=&gs_sm=&gs_upl=&fp=1&biw=1440&bih=799&bav=on.2,or.r_gc.r_pw.r_cp.,cf.osb&cad=b
//http://msdn.microsoft.com/es-es/library/15t15zda.aspx

//TraceListener http://msdn.microsoft.com/es-es/library/system.diagnostics.consoletracelistener.aspx
//textWriteTraceListener http://msdn.microsoft.com/es-es/library/system.diagnostics.textwritertracelistener.aspx
/*Trace (Clase)

TraceEventCache (Clase).Proporciona los datos de evento de seguimiento específicos de un subproceso y un proceso.
TraceEventType (Enumeración)Identifica el tipo de evento que ha originado el seguimiento.
TraceFilter (Clase)Proporciona la clase base para las implementaciones de los filtros de seguimiento.
TraceLevel (Enumeración)Especifica qué mensajes se van a enviar para las clases Debug, Trace y TraceSwitch.
TraceListener (Clase)Proporciona la clase base abstract a los agentes de escucha que supervisan los resultados de seguimiento y de depuración.
TraceListenerCollection (Clase)Proporciona una lista de objetos TraceListener seguros para la ejecución de subprocesos.
TraceLogRetentionOption (Enumeración)Especifica la estructura de archivos que se utilizará para el registro de EventSchemaTraceListener.
TraceOptions (Enumeración)Especifica las opciones de los datos de seguimiento que se van a escribir en el resultado de seguimiento.
TraceSource (Clase)Proporciona un conjunto de métodos y propiedades que permiten a las aplicaciones realizar un seguimiento de la ejecución de código y asociar los mensajes de seguimiento a su origen.
TraceSwitch (Clase)Proporciona un modificador multinivel para controlar la generación de traza y depuración sin recompilar su código.*/


//todo: Esta clase debe heredar de la clase Exception. http://msdn.microsoft.com/es-es/library/ms173163.aspx

//Todo: Intentar que escriba los errores en un fichero de Log si está activa la traza, sino debería mostrarlos por la consola.
//Implementar un método que permite realizar trazas.
//En caso de error recuperar la pila de llamada

namespace SimpleDataMapper
{
    //Todo: Revisar esta función de traza, no debería mostrar mensajes muy descriptivos.
    //Todo: Mirar en la documentación MSDN, como tracear.
    internal class Traccer
    {
        /// <summary>
        ///     Guarda un texto en un Fichero de Texto
        /// </summary>
        public static bool bDebug = true;


        public static void GuardarTrazaSql(String sTexto)
        {
            String fic;
            fic = Directory.GetCurrentDirectory() + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day +
                  "-TracerSql" + ".log";
            var sw = new StreamWriter(fic, true);
            sw.WriteLine(DateTime.Now + " - " + sTexto);
            sw.Close();
        }

        public static void RunException(Exception ex, String sStringError, String sNameFunction)
        {
            var ePerson = new Exception(sStringError);
            if (bDebug)
                GuardarTrazaSql(sNameFunction + " " + ex);
            throw ePerson;
        }

        public static void RunException(String sStringError, String sNameFunction)
        {
            if (bDebug)
                GuardarTrazaSql(sNameFunction + " - " + sStringError);

            var ePerson = new Exception(sStringError);
            throw ePerson;
        }
    }
}