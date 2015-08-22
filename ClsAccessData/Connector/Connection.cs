using System;
using System.Data;
using Npgsql;

namespace SimpleDataMapper.Connector
{
    /// <summary>
    /// </summary>
    public class Connection
    {
        #region" Región de declaración de variables."

        /// <summary>
        ///     Representa un conjunto de comandos de datos y una conexión de base de
        ///     datos que se utilizan para rellenar un DataSet
        /// </summary>
        private NpgsqlDataAdapter _miDataAdapter;

        #endregion;

        #region [Properties and Constructor]

        /// <summary>
        ///     Método constructor de la clase conexión
        /// </summary>
        /// <param name="sServidorDb">Servidor de la base datos.</param>
        /// <param name="sBd">Catálogo de la base datos.</param>
        /// <param name="sUsuarioBd">Usuario de la base datos.</param>
        /// <param name="sContrenaBd">Contraseña de la base de datos.</param>
        public Connection(string sServidorDb, string sBd, string sUsuarioBd, string sContrenaBd)
        {
            Server = sServidorDb;
            DataBase = sBd;
            User = sUsuarioBd;
            Password = sContrenaBd;

            string sConectionString = string.Format("User Id={0};Port=5432;Password={1};Server={2};Database={3}", sUsuarioBd,
                Password, Server, DataBase);
            DbConnection = new NpgsqlConnection {ConnectionString = sConectionString};
            // this.oConnection.Open();
        }

        /// <summary>
        ///     Devuelve el servidor de la base de datos.
        /// </summary>
        private string Server { get; set; }

        /// <summary>
        ///     Devuelve la contraseña a la base de datos.
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        ///     Devuelve en nombre de la base datos.
        /// </summary>
        private string DataBase { get; set; }

        /// <summary>
        ///     Devuelve el usuario de la base datos.
        /// </summary>
        private string User { get; set; }

        /// <summary>
        ///     Obtiene el objeto oTransaccion.
        /// </summary>
        internal NpgsqlTransaction Transaccion { get; private set; }

        /// <summary>
        ///     Obtiene el objeto oConnection.
        /// </summary>
        public NpgsqlConnection DbConnection { get; private set; }

        /// <summary>
        ///     Indica el estado de la conexión de la base de datos.
        /// </summary>
        /// <returns>Devuelve un tipo ConnectionState que indica el estado de la conexión.</returns>
        internal ConnectionState Status()
        {
            return DbConnection.State;
        }

        /// <summary>
        ///     Abre la conexión a la base datos.
        /// </summary>
        public void DbOpen()
        {
            if (DbConnection.State == ConnectionState.Closed)
            {
                DbConnection.Open();
            }
        }

        /// <summary>
        ///     Cierra la conexión a la base datos.
        /// </summary>
        public void DbClose()
        {
            if (DbConnection.State == ConnectionState.Open)
            {
                DbConnection.Close();
            }
        }

        /// <summary>
        ///     Comienza una transacción a la base datos.
        /// </summary>
        internal void BeginTransaction()
        {
            Transaccion = DbConnection.BeginTransaction();
        }

        /// <summary>
        ///     Confirma la transacción actual a la base datos.
        /// </summary>
        internal void Commit()
        {
            Transaccion.Commit();
            Transaccion = null;
        }

        /// <summary>
        ///     Cancela la transacción actual a la base datos.
        /// </summary>
        internal void RollBack()
        {
            Transaccion.Rollback();
            Transaccion = null;
        }

        /// <summary>
        ///     Executa una instrucción DML (Insert, Delete, Update).
        /// </summary>
        /// <param name="sSql">Instrucción DML</param>
        /// <returns>Devuelve el número de registro afectados.</returns>
        public int Execute(string sSql)
        {
            var iCount = 0;
            var oCmd = DbConnection.CreateCommand();
            if (Transaccion != null)
            {
                oCmd.Transaction = Transaccion;
            }
            oCmd.CommandText = sSql;
            oCmd.CommandTimeout = 120;

            iCount = oCmd.ExecuteNonQuery();
            return iCount;
        }

        /// <summary>
        ///     Ejecuta una instrucción Select
        /// </summary>
        /// <param name="sQuery">Instrucción Select.</param>
        /// <returns>Devuelve un objeto DataSet.</returns>
        public DataSet InitDataAdapter(String sQuery)
        {
            var dsRes = new DataSet();
            try
            {
                _miDataAdapter = new NpgsqlDataAdapter(sQuery, DbConnection)
                {
                    SelectCommand = {Transaction = Transaccion}
                };

                _miDataAdapter.Fill(dsRes);
                _miDataAdapter = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return dsRes;
        }

        /// <summary>
        ///     Ejecuta una instrucción Select
        /// </summary>
        /// <param name="sQuery">Instrucción Select.</param>
        /// <returns>Devuelve un objeto DataSet.</returns>
        public NpgsqlDataReader DataReader(String sQuery)
        {
            var command = new NpgsqlCommand(sQuery, DbConnection);
            if (Status() == ConnectionState.Closed)
                DbOpen();
            var reader = command.ExecuteReader();

            return reader;
        }

        #endregion;
    }
}