using System;
using System.Collections.Generic;
using System.Text;

namespace Tareas.ClsTablas
{
    class ClsOperacionEncargos
    {
        private String sNombreTabla = "operacion_encargos";
        private long id_encargo;
        private String estado;
        private int id_operacion;

        public long Id_encargo
        {
            get { return id_encargo; }
            set { id_encargo = value; }
        }
        
        public int Id_operacion
        {
            get { return id_operacion; }
            set { id_operacion = value; }
        }
        
        public String Estado
        {
            get { return estado; }
            set { estado = value; }
        }



    }
}
