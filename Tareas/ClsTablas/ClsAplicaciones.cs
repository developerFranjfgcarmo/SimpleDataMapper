using System;
using System.Collections.Generic;
using System.Text;

namespace Tareas.ClsTablas
{
    class ClsAplicaciones: ClsDisposable
    {
        #region"Declaración de datos miembros de la clase."

        private String sNombreTabla = "aplicaciones";
        private String cd_aplicacion;
        private String ds_aplicacion;
        private long id_aplicacion;
        private DateTime fc_baja;
        private DateTime fc_alta;
        private DateTime fc_modi;

        #endregion;

        #region

        public ClsAplicaciones(String cd_aplicacion,String ds_aplicacion)
        {
            //Se debe obtener el codigo de la aplicación a través de la secuencia.
            //id_aplicacion = ;
            this.cd_aplicacion = cd_aplicacion;
            this.ds_aplicacion = ds_aplicacion;

            //todo: la fecha de alta se debe obtener de esta QUERY.SELECT LOCALTIMESTAMP;
            //this.fc_alta = 
        }
        public ClsAplicaciones(long id_aplicacion, DateTime fc_baja)
        {
            this.id_aplicacion = id_aplicacion;
            if (fc_baja == null)
            {
                //Obtenemos los valores de los campos.
            }
            else 
            {
 
            }
        }

        #endregion;

        #region"Declaración de métodos miembros de la clase."

        public String Cd_aplicacion
        {
            get { return this.cd_aplicacion; }
            set { this.cd_aplicacion = value; }
        }
        
        public String Ds_aplicacion
        {
            get { return this.ds_aplicacion; }
            set { this.ds_aplicacion = value; }
        }
        
        public long Id_aplicacion
        {
            get { return this.id_aplicacion; }
        }

        #endregion
    }
}
