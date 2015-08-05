using System;
using System.Collections.Generic;
using System.Text;

namespace Tareas.ClsTablas
{
    class ClsEncargos
    {
        private String sNombreTabla = "encargos";
        private String cd_seco;
        private long id_encargo;
        private String cd_remedy;

        public ClsEncargos(){
            //todo:inicializar la clave
        }
        public String Cd_seco
        {
            get { return cd_seco; }
            set { cd_seco = value; }
        }
        
        public String Cd_remedy
        {
            get { return cd_remedy; }
            set { cd_remedy = value; }
        }
        
        public long Id_encargo
        {
            get { return id_encargo; }
        }


    }
}
