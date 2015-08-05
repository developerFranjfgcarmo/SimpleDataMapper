using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Tareas.ControlData
{
    public class  ClsGeneral
    {
        /// <summary>
        /// Devuelve un valor boolean que indica si la expresión de puede evaluar como número
        /// </summary>
        /// <param name="Expression">Parametro necesario, tipo Object</param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
           double retNum;
           
            return Double.TryParse(Convert.ToString(Expression), 
                                    NumberStyles.Any,
                                    NumberFormatInfo.InvariantInfo, 
                                    out retNum);
        }
        /// <summary>
        /// Devuelve un valor boolean que indica si la expresión de puede evaluar como número
        /// </summary>
        /// <param name="Expression">Parametro necesario, tipo Object </param>
        /// <returns></returns>
        public static bool IsDate(Object Expression)
        {
            String sFecha = (string)Expression;

            if (sFecha == null)
            {sFecha = "";}

            if (sFecha.Length > 0)
            {
                DateTime dummyDate;
                try { dummyDate = DateTime.Parse(sFecha); }
                catch{return false;}

                return true;
            }
            else
            {return false;}
        }

        /// <summary>
        /// Función que comprueba si un valor es nulo, en cuyo caso devuelve una 
        /// cadena vacía ("").
        /// </summary>
        /// <param name="Valor">
        /// Valor que se va a evaluar.
        /// </param>
        /// <returns>
        /// Devuelve el mismo valor o una cadena vacía("") si el valor es nulo.
        /// </returns>

        public static Object NoNull(Object Valor)
        {
            Object tmp;
            if (Valor == null)
                tmp = "";
            else
                tmp = Valor;

            return tmp;
        }
    }
}
