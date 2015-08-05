using System;
using System.Collections.Generic;
using System.Text;

namespace Tareas.ClsTablas
{
    class ClsEstadoDesarrollos
    {
        private String sNombreTabla = "estado_desarrollos";
        private int id_estado_desarrollo;
        private String ds_estado_desarrollo;
        private String observaciones;

        public int Id_estado_desarrollo
        {
            get { return id_estado_desarrollo; }
            set { id_estado_desarrollo = value; }
       }

        public String Observaciones
        {
            get { return observaciones; }
            set { observaciones = value; }
        }

        public String Ds_estado_desarrollo
        {
            get { return ds_estado_desarrollo; }
            set { ds_estado_desarrollo = value; }
        }
        


    }
}
