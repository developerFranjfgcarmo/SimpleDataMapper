using System;
using System.Collections.Generic;
using System.Text;

namespace Tareas.ClsTablas
{
    class ClsTipoDesarrolloUsuarios : ClsDisposable
    {
        #region"Declaración de datos miembros."

            private String sNombreTabla = "tipo_desarrollo_usuarios";
            private long id_encargo_tipo_desarrollo;
            private long id_usuario;
            private DateTime fecha_inicio;
            private DateTime fecha_fin;
            private int id_estado_desarrollo;

        #endregion;

        #region"Constructores de la clase"

        public ClsTipoDesarrolloUsuarios(long id_encargo_tipo_desarrollo, long id_usuario, Estado update)
        {

                this.id_encargo_tipo_desarrollo = id_encargo_tipo_desarrollo;
                this.id_usuario = id_usuario;

               // if (update) { 
                    //Inicializo el resto de valores de la tabla
                    /*
                    this.fecha_inicio=fecha_inicio;
                    this. fecha_fin =fecha_fin ;
                    this.id_estado_desarrollo=id_estado_desarrollo;*/
              //  }
                
            }

            public ClsTipoDesarrolloUsuarios(long id_encargo_tipo_desarrollo,long id_usuario,DateTime fecha_inicio,
                                               DateTime fecha_fin,int id_estado_desarrollo){
                 
                this.id_encargo_tipo_desarrollo =id_encargo_tipo_desarrollo;
                this.id_usuario=id_usuario;
                this.fecha_inicio=fecha_inicio;
                this. fecha_fin =fecha_fin ;
                this.id_estado_desarrollo=id_estado_desarrollo;

            }

        #endregion;

        #region"Declaración de métodos miembros de la clase"
                public long Id_encargo_tipo_desarrollo
                {
                    get { return id_encargo_tipo_desarrollo; }
                    set { id_encargo_tipo_desarrollo = value; }
                }

                public long Id_usuario
                {
                    get { return id_usuario; }
                    set { id_usuario = value; }
                }

                public DateTime Fecha_inicio
                {
                    get { return fecha_inicio; }
                    set { fecha_inicio = value; }
                }

                public DateTime Fecha_fin
                {
                    get { return fecha_fin; }
                    set { fecha_fin = value; }
                }
                
                public int Id_estado_desarrollo
                {
                    get { return id_estado_desarrollo; }
                    set { id_estado_desarrollo = value; }
                }

        #endregion;

    }
}
