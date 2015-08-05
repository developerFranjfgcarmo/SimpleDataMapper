using System;
using System.Data;
using Npgsql;

namespace SimpleDataMapper.Connection
{
    /// <summary>
    /// </summary>
    public class ClsConnection
    {
        #region" Región de declaración de variables."

        /// <summary>
        ///     Instancia de tipo SqlConnection que se conecta a la be de datos
        /// </summary>
        private readonly NpgsqlConnection _oConnection;

        /// <summary>
        ///     Datos miembros para la conexión a la base datos.
        /// </summary>
        private readonly string _sBd;

        /// <summary>
        ///     Datos miembros para la conexión a la base datos.
        /// </summary>
        private readonly string _sContrenaBd;

        /// <summary>
        ///     Datos miembros para la conexión a la base datos.
        /// </summary>
        private readonly string _sServidorDb;

        /// <summary>
        ///     Datos miembros para la conexión a la base datos.
        /// </summary>
        private readonly string _sUsuarioBd;

        /// <summary>
        ///     Representa un conjunto de comandos de datos y una conexión de base de
        ///     datos que se utilizan para rellenar un DataSet
        /// </summary>
        private NpgsqlDataAdapter _oMiDataAdapter;


        /// <summary>
        ///     Representa una transacción de Transact-SQL que se realiza en una be de datos de SQL Server
        /// </summary>
        private NpgsqlTransaction _oTransaccion;

        #endregion;

        #region"Declaración de los métodos miembros de la clase."

        /// <summary>
        ///     Método constructor de la clase conexión
        /// </summary>
        /// <param name="sServidorDb">Servidor de la base datos.</param>
        /// <param name="sBd">Catálogo de la base datos.</param>
        /// <param name="sUsuarioBd">Usuario de la base datos.</param>
        /// <param name="sContrenaBd">Contraseña de la base de datos.</param>
        public ClsConnection(string sServidorDb, string sBd, string sUsuarioBd, string sContrenaBd)
        {
            _sServidorDb = sServidorDb;
            _sBd = sBd;
            _sUsuarioBd = sUsuarioBd;
            _sContrenaBd = sContrenaBd;

            string sConectionString = string.Format("User Id={0};Password={1};Host={2};Database={3}", _sUsuarioBd,
                _sContrenaBd, _sServidorDb, _sBd);
            _oConnection = new NpgsqlConnection {ConnectionString = sConectionString};
            // this.oConnection.Open();
        }

        /// <summary>
        ///     Devuelve el servidor de la base de datos.
        /// </summary>
        private string SServidorDb
        {
            get { return _sServidorDb; }
        }

        /// <summary>
        ///     Devuelve la contraseña a la base de datos.
        /// </summary>
        private string SContrenaBd
        {
            get { return _sContrenaBd; }
        }

        /// <summary>
        ///     Devuelve en nombre de la base datos.
        /// </summary>
        private string SBD
        {
            get { return _sBd; }
        }

        /// <summary>
        ///     Devuelve el usuario de la base datos.
        /// </summary>
        private string SUsuarioBD
        {
            get { return _sUsuarioBd; }
        }

        /// <summary>
        ///     Obtiene el objeto oTransaccion.
        /// </summary>
        internal NpgsqlTransaction DBTransaccion
        {
            get { return _oTransaccion; }
        }

        /// <summary>
        ///     Obtiene el objeto oConnection.
        /// </summary>
        public NpgsqlConnection DbConnection
        {
            get { return _oConnection; }
        }

        /// <summary>
        ///     Indica el estado de la conexión de la base de datos.
        /// </summary>
        /// <returns>Devuelve un tipo ConnectionState que indica el estado de la conexión.</returns>
        internal ConnectionState Status()
        {
            return _oConnection.State;
        }

        /// <summary>
        ///     Abre la conexión a la base datos.
        /// </summary>
        public void DbOpen()
        {
            if (_oConnection.State == ConnectionState.Closed)
            {
                _oConnection.Open();
            }
        }

        /// <summary>
        ///     Cierra la conexión a la base datos.
        /// </summary>
        public void DbClose()
        {
            if (_oConnection.State == ConnectionState.Open)
            {
                _oConnection.Close();
            }
        }

        /// <summary>
        ///     Comienza una transacción a la base datos.
        /// </summary>
        internal void DbBeginTransaction()
        {
            _oTransaccion = _oConnection.BeginTransaction();
        }

        /// <summary>
        ///     Confirma la transacción actual a la base datos.
        /// </summary>
        internal void BdCommit()
        {
            _oTransaccion.Commit();
            _oTransaccion = null;
        }

        /// <summary>
        ///     Cancela la transacción actual a la base datos.
        /// </summary>
        internal void BdRollBack()
        {
            _oTransaccion.Rollback();
            _oTransaccion = null;
        }

        /// <summary>
        ///     Executa una instrucción DML (Insert, Delete, Update).
        /// </summary>
        /// <param name="sSql">Instrucción DML</param>
        /// <returns>Devuelve el número de registro afectados.</returns>
        public int DbExecute(string sSql)
        {
            int iCount = 0;
            NpgsqlCommand oCmd = _oConnection.CreateCommand();
            if (_oTransaccion != null)
            {
                oCmd.Transaction = _oTransaccion;
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
                _oMiDataAdapter = new NpgsqlDataAdapter(sQuery, DbConnection)
                {
                    SelectCommand = {Transaction = DBTransaccion}
                };

                _oMiDataAdapter.Fill(dsRes);
                _oMiDataAdapter = null;
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
            var command = new NpgsqlCommand(sQuery, _oConnection);
            if (Status() == ConnectionState.Closed)
                DbOpen();
            NpgsqlDataReader reader = command.ExecuteReader();

            return reader;
        }

        #endregion;
    }
}