using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tareas.ControlData;

namespace Tareas.ClsTablas
{
    class ClsIdiomas
    {
        #region "Declaración de campos de la clase."
        
        private String BaseTable = "idiomas";
        private String idiso;
        private String desidioma;
        
        #endregion;

        #region"Declaración de las propiedades."

        public String Idiso
        {
            get { return idiso; }
            set { idiso = value; }
        }
        public String DesIdioma
        {
            get { return desidioma; }
            set { desidioma = value; }
        } 

        #endregion;



    }
}
