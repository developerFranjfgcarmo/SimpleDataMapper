using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Tareas.ControlData;

namespace Tareas.ClsTablas
{
    class ClsUsuarios: ClsDisposable
    {
        #region"Declaración de datos miembros."

        private String BaseTable = "usuarios";
        private int id_usuario;
        private String usuario;
        private String contrasena;
        private String nombre;
        private String apellidos;
        private DateTime fc_alta;
        private DateTime fc_modi;
        private DateTime fc_baja;
              //  private Hashtable colUsuarios = new Hashtable();

        #endregion;

        #region"Contructores de la clase."

        /// <summary>
        /// Constructor para la actualización o borrado de usuarios.
        /// </summary>
        /// <param name="id_usuario">Identificador del usuario.</param>
        public ClsUsuarios(int id_usuario)
        { 
            //Inicialización de los datos del usuario.
            //oQuery.SQLQuery("SELECT ")
            this.id_usuario = id_usuario;
           /* try
            {
                Tareas.Program.clsStruct.GetFields(this);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }*/
            
        }
        /// <summary>
        /// Declaración para el alta de usuarios.
        /// </summary>
        /// <param name="usuario">Usuario</param>
        /// <param name="contrasena">Contraseña del Usuario.</param>
        /// <param name="nombre">Nombre del Usuario.</param>
        /// <param name="apellidos">Apellido del Usuario.</param>
        public ClsUsuarios(String usuario,String contrasena,String nombre,String apellidos)
        {
            /*ClsDataQuery oQuery = new ClsDataQuery();
            //Este campo se inicializa con el valor de la secuencia.
            oQuery.SQLQuery(Tareas.Program.oCon, "SELECT nextval('usuarios_id_usuario_seq') AS valor",true);
            if (oQuery.Count>0 )
                this.id_usuario=int.Parse(oQuery.Fields("valor"));
            oQuery.SQLQuery(Tareas.Program.oCon, "SELECT COUNT(usuario) AS n_cuenta WHERE usuario = '" + usuario + "'",true);
            if (oQuery.Count==0)
                throw new System.ArgumentException("El Usuario "+ usuario +" ya existe en la Base de datos.", "Error");
            this.usuario=usuario;
            this.contrasena =contrasena ;
            this.nombre =nombre ;
            this.apellidos =apellidos ;
            this.fc_alta =DateTime.Now ;
            //Estos campos soló se modifican con instrucción update y delete repectivamente.
            //fc_modi 
            //fc_baja*/
        }

        public ClsUsuarios() { }
            
        #endregion;

        #region "Declaración de métodos miembros."

        public int Id_usuario
        {
            get { return this.Id_usuario; }
        }

        public String Usuario
        {
            get { return this.usuario; }
            set { usuario = value;}
        }
        

        public String Contrasena
        {
            get { return this.contrasena; }
            set { contrasena = value;}
        }
        

        public String Nombre
        {
            get { return this.nombre; }
            set { nombre = value;}
        }
        

        public String Apellidos
        {
            get { return this.apellidos; }
            set { apellidos = value;}
        }


        public DateTime Fc_alta
        {
            get { return this.fc_alta; }
            set { fc_alta = value;}
        }


        public DateTime Fc_modi
        {
            get { return this.fc_modi; }
            set { fc_modi = value;}
        }

        public DateTime Fc_baja
        {
            get { return this.fc_baja; }
            set { fc_baja = value;}
        }
        #endregion;
        /*
        public void Guardar() {
            //Abrimos la conexión.
            String sComa = ",";
            String sCampos = "";
            String sValor="";
            AccessData.ClsConnection oConn = new AccessData.ClsConnection("localHost", "tareas", "postgres", "qpmz56tg");
            foreach (DictionaryEntry de in colUsuarios)
            {
                //System.Windows.Forms.MessageBox.Show("Key " + de.Key + " valor " + de.Value);
                if (!String.IsNullOrEmpty(sCampos))
                { 
                    sCampos += sComa;
                }
                sCampos += de.Key;

                if (!String.IsNullOrEmpty(sValor))
                {
                    sValor += sComa;
                }
                sValor +="'"+ de.Value + "'";
                
            }
            try
            {
                oConn.DBOpen();
                oConn.DBBeginTransaction();
                oConn.DBExecute("INSERT INTO usuarios (" + sCampos + ") values (" + sValor + ")");
                oConn.BDCommit();
            }
            catch {
                oConn.BDRollBack();
                System.Windows.Forms.MessageBox.Show("Error al abrir la conexión.");
            }
    }*/

    }
}
