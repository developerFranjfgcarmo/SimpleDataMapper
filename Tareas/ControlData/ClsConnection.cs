using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Data;

namespace Tareas.AccessData
{

    public class ClsConnection
    {
        #region" Región de declaración de variables."

            /// <summary>
            /// Datos miembros para la conexión a la base datos.
            /// </summary>
            private string sServidorDB, sBD, sUsuarioBD, sContrenaBD;

            
            /// <summary>
            /// Representa una transacción de Transact-SQL que se realiza en una be de datos de SQL Server
            /// </summary>
            private NpgsqlTransaction oTransaccion  ;

            /// <summary>
            /// Instancia de tipo SqlConnection que se conecta a la be de datos
            /// </summary>
            private NpgsqlConnection oConnection  ;
        
        #endregion;

        #region"Declaración de los métodos miembros de la clase."
            
        /// <summary>
        /// Método constructor de la clase conexión
        /// </summary>
        /// <param name="sServidorDB">Servidor de la base datos.</param>
        /// <param name="sBD">Catálogo de la base datos.</param>
        /// <param name="sUsuarioBD">Usuario de la base datos.</param>
        /// <param name="sContrenaBD">Contraseña de la base de datos.</param>
        public ClsConnection(string sServidorDB, string sBD, string sUsuarioBD, string sContrenaBD){
            string sConectionString;

            this.sServidorDB = sServidorDB;
            this.sBD = sBD;
            this.sUsuarioBD = sUsuarioBD;
            this.sContrenaBD = sContrenaBD;          

            sConectionString = "User Id=" + this.sUsuarioBD + 
                                ";Password=" + this.sContrenaBD + 
                                ";Host=" + this.sServidorDB +
                                ";Database=" + this.sBD;
            this.oConnection = new NpgsqlConnection();
            this.oConnection.ConnectionString =  sConectionString;
           // this.oConnection.Open();

        }


        /// <summary>
        /// 
        /// </summary>
        public string SServidorDB {
            get {
                return this.sServidorDB;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string SContrenaBD
        {
            get
            {
                return this.sContrenaBD;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SBD
        {
            get
            {
                return this.sBD;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string SUsuarioBD
            {
                get
                {
                    return this.sUsuarioBD;
                }
            }
        
        /// <summary>
        /// Obtiene el objeto oTransaccion.
        /// </summary>
        public NpgsqlTransaction DBTransaccion {
            get {
                return this.oTransaccion;
            }
        }

        /// <summary>
        /// Obtiene el objeto oConnection.
        /// </summary>
        public NpgsqlConnection DBConnection { 
            get {
                return this.oConnection;
            }
        }
        
        /// <summary>
        /// Abre la conexión a la base datos.
        /// </summary>
        public void DBOpen(){
            if (this.oConnection.State == ConnectionState.Closed) {
                this.oConnection.Open();
            }
        }

        /// <summary>
        /// Cierra la conexión a la base datos.
        /// </summary>
        public void DBClose(){
            if (this.oConnection.State == ConnectionState.Open) {
                this.oConnection.Close();
            }
        }

        /// <summary>
        /// Comienza una transacción a la base datos.
        /// </summary>
        public void DBBeginTransaction(){
            this.oTransaccion= this.oConnection.BeginTransaction();            
        }

        /// <summary>
        /// Confirma la transacción actual a la base datos.
        /// </summary>
        public void BDCommit(){
            this.oTransaccion.Commit();
            this.oTransaccion= null;
        }

        /// <summary>
        /// Cancela la transacción actual a la base datos.
        /// </summary>
        public void BDRollBack(){
            this.oTransaccion.Rollback();
            this.oTransaccion= null;
        }

        /// <summary>
        /// Executa la transacción actual.
        /// </summary>
        /// <param name="sSql"></param>
        /// <returns></returns>
        public int DBExecute(string sSql){
            int iCount= 0;
            try{
                NpgsqlCommand oCmd = this.oConnection.CreateCommand();
                if (this.oTransaccion != null){
                    oCmd.Transaction = this.oTransaccion;
                }
                oCmd.CommandText = sSql;
                oCmd.CommandTimeout = 120;

                iCount = oCmd.ExecuteNonQuery();

            }
            catch(Exception ex){
                //throw ex;
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                return iCount;
            }
            return iCount;
        }

        #endregion;

        

        }
}
