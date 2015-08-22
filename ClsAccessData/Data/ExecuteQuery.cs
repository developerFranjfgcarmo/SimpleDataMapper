//Todo: Mirar a crear otro método sobrecargado de la Select en el que se pase el Objeto sWhere y el Objeto a devolver.
//Todo: Mirar a crear una sobre carga de los métodos update, delete, in-

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using SimpleDataMapper.Connector;

namespace SimpleDataMapper.Data
{
    /// <summary>
    ///     Clase que realiza todas las llamadas instrucciónes de DML sobre la base de datos.
    /// </summary>
    public class ExecuteQuery
    {
        #region [Private Methods]

        /// <summary>
        ///     Almacena la conexión a la base de datos.
        /// </summary>
        private readonly Connection _con;

        /// <summary>
        ///     Almacena una colección de objetos de diferentes clases pasados por parámetros.
        /// </summary>
        private Object[] _coumnlObject;

        /// <summary>
        ///     Almacena una colección de objetos de la misma clase.
        /// </summary>
        private List<object> _listTables;

        /// <summary>
        ///     Valida el Objeto con la tabla correspondiente.
        /// </summary>
        private ValidateObjects _validateColumns;

        /// <summary>
        ///     Contiene la función delegado a la que tiene que apuntar para devolver la lista de resultados.
        /// </summary>
        /// <returns></returns>
        private delegate ArrayList DelegadoSelect();

        #endregion

        #region [Constructor]

        /// <summary>
        ///     Constructor de la clase ClsExecuteQuery.
        /// </summary>
        /// <param name="oCon">Instancia al Objeto conexión de la base de datos.</param>
        public ExecuteQuery(Connection oCon)
        {
            _con = oCon;
        }

        #endregion

        #region Declaración de métodos públicos.

        /// <summary>
        ///     Inserción de registros de la tabla.
        /// </summary>
        /// <param name="table">
        ///     Instancia de una lista de objetos de una clase con la que se compone la instrucción de Insert para realizar
        ///     las diferentes inserciones. Los miembros de la clase deben ser System.Nullable.
        /// </param>
        public void Insert(List<Object> table)
        {
            IniParamDml(table);
            Execute(QueryType.Insert);
        }

        /// <summary>
        ///     Inserción de registros de la tabla.
        /// </summary>
        /// <param name="arrayTables">
        ///     Instancia de una clase o instancias de diferentes clase con la que se compone la instrucción de Insert para
        ///     realizar
        ///     las diferentes inserciones. Los miembros de la clase deben ser System.Nullable. Cada Objeto debe ser de una clase
        ///     diferente y se debe pasar en el
        ///     orden en el que se desea realizar cada operación, se debe tener cuidado con las foreing Key de las tablas.
        /// </param>
        public void Insert(params Object[] arrayTables)
        {
            IniParamDml(arrayTables);
            Execute(QueryType.Insert);
        }

        /// <summary>
        ///     Actualización de registros de la tabla.
        /// </summary>
        /// <param name="listTables">
        ///     Instancia de una lista de objetos de una clase con la que se compone la instrucción de Update para realizar
        ///     las diferentes actualizaciones. Los miembros de la clase deben ser System.Nullable.
        /// </param>
        public void Update(List<Object> listTables)
        {
            IniParamDml(listTables);
            Execute(QueryType.Update);
        }

        /// <summary>
        ///     Actualización de registros de la tabla.
        /// </summary>
        /// <param name="arrayTables">
        ///     Instancia de una clase o  instancias de diferentes clase con la que se compone la instrucción de Update para
        ///     realizar
        ///     las diferentes actualizaciones. Los miembros de la clase deben ser System.Nullable. Cada Objeto debe ser de una
        ///     clase diferente y se debe pasar en el
        ///     orden en el que se desea realizar cada operación, es decir se debe tener cuidado con las foreing Key de las tablas.
        /// </param>
        public void Update(params Object[] arrayTables)
        {
            IniParamDml(arrayTables);
            Execute(QueryType.Update);
        }

        /// <summary>
        ///     Elimina los registros de la tabla.
        /// </summary>
        /// <param name="arrayTables">
        ///     Instancia de una clase o instacias de diferentes clases con la que se compone la instrucción DELETE para eliminar
        ///     los registros de cada tabla. Los miembros de la clase deben System.Nullable. Cada Objeto debe ser de una clase
        ///     diferente y se debe pasar en el
        ///     orden en el que se desea realizar cada operación, es decir se debe tener cuidado con las foreing Key de las tablas.
        /// </param>
        public void Delete(params Object[] arrayTables)
        {
            IniParamDml(arrayTables);
            Execute(QueryType.Delete);
        }

        /// <summary>
        ///     Elimina los registros de la tabla.
        /// </summary>
        /// <param name="listTables">
        ///     Array de un de diferentes instancias de una clase, con la que se compone cada instrucción DELETE para eliminar
        ///     un registro de tabla. Los miembros de la clase deben System.Nullable.
        /// </param>
        public void Delete(List<Object> listTables)
        {
            IniParamDml(listTables);
            Execute(QueryType.Delete);
        }

        /// <summary>
        ///     Obtiene el conjunto de resultados de una consulta Select en función del Objeto pasado.
        /// </summary>
        /// <param name="table">
        ///     Objeto que se utilizará como filtro de la consulta Select y como resultado del Objeto
        ///     ArrayList.
        /// </param>
        /// <returns>Devuelve un ArrayList del objeto pasado por parámetro.</returns>
        public ArrayList Select(Object table)
        {
            IniParamDml(table);
            return SelectQuery(SelectDataReader);
        }

        /// <summary>
        ///     Obtiene el conjunto de resultados de una consulta Select en función del Objeto pasado.
        /// </summary>
        /// <param name="table">
        ///     Objeto que se utilizará como filtro de la consulta Select y como resultado del Objeto
        ///     ArrayList.
        /// </param>
        /// <returns>Devuelve un ArrayList del objeto pasado por parámetro.</returns>
        public ArrayList SelectDataSet(Object table)
        {
            IniParamDml(table);
            return SelectQuery(SelectDataSet);
        }

        #endregion

        #region [Private Methods]

        /// <summary>
        ///     Inicializa el objeto para la instrucción DML.
        /// </summary>
        /// <param name="arrayObject">Lista de Objetos para realizar la instrucción DML.</param>
        private void IniParamDml(params Object[] arrayObject)
        {
            _coumnlObject = arrayObject;
            _validateColumns = new ValidateObjects(_con, _coumnlObject);
        }

        /// <summary>
        ///     Inicializa el objeto para la instrucción DML.
        /// </summary>
        /// <param name="listObject">Lista de Objetos para realizar la instrucción DML.</param>
        private void IniParamDml(IList<object> listObject)
        {
            _listTables=(List<object>) listObject;
            _validateColumns = new ValidateObjects(_con, listObject[0]);
        }

        /// <summary>
        ///     Ejecuta la culsuta de UPDATE,INSERT,DELETE.
        /// </summary>
        /// <param name="queryType">Indica el tipo de operación.</param>
        private void Execute(QueryType queryType)
        {
            try
            {
                if (_con.Status() == ConnectionState.Closed)
                    _con.DbOpen();
                _con.BeginTransaction();

                foreach (var myClsObj in _validateColumns.ColumnObjects)
                {
                    if (_coumnlObject != null)
                    {
                        //Ejecutamos la transacción a la base de datos.
                        _con.Execute(_validateColumns.GetQuery(queryType, myClsObj.Value));
                    }
                    else
                    {
                        foreach (Object myObj in _listTables)
                        {
                            _con.Execute(_validateColumns.GetQuery(queryType, myClsObj.Value, myObj));
                        }
                    }
                }
                _con.Commit();
            }
            catch (NpgsqlException ex)
            {
                //todo:MEJORA. Quitar las exception y sustituir por una clase de negocio.
                _con.RollBack();
                throw new ArgumentException(ex.ToString(), ToString());
            }
            finally
            {
                _con.DbClose();
            }
        }

        /// <summary>
        ///     Devuelve un arraylist obtenidos a través de una consulta SQL.
        /// </summary>
        /// <param name="funDel">Nombre de la función delegado que debe de ejecutar.</param>
        /// <returns>Devuelve un objeto ArrayList con el resultado de la consulta SQL.</returns>
        private ArrayList SelectQuery(DelegadoSelect funDel)
        {
            ArrayList resultSelect;
            try
            {
                if (_con.Status() == ConnectionState.Closed)
                    _con.DbOpen();
                DelegadoSelect queryDelegate = funDel;
                resultSelect = queryDelegate();
                _con.DbClose();
            }
            catch (NpgsqlException ex)
            {
                throw new ArgumentException(ex.ToString(), ToString());
            }
            finally
            {
                _con.DbClose();
            }
            return resultSelect;
        }

        /// <summary>
        ///     Devuelve un arraylist obtenidos a través de una consulta SQL.
        /// </summary>
        /// <returns>Devuelve un objeto ArrayList con el resultado de la consulta SQL.</returns>
        private ArrayList SelectDataSet()
        {
            var resultSelect = new ArrayList();
            foreach (var myClsObj in _validateColumns.ColumnObjects)
            {
                if (_coumnlObject == null) continue;
                DataSet dtTablaSelect = _con.InitDataAdapter(_validateColumns.GetQuery(QueryType.Select,
                    myClsObj.Value));
                foreach (DataRow row in dtTablaSelect.Tables[0].Rows)
                {
                    myClsObj.Value.CreateInstance();
                    foreach (DataColumn col in dtTablaSelect.Tables[0].Columns)
                    {
                        myClsObj.Value[col.ColumnName] = row[col.ColumnName];
                    }
                    resultSelect.Add(myClsObj.Value.MyObject);
                }
            }
            return resultSelect;
        }

        /// <summary>
        ///     Devuelve un arraylist obtenidos a través de una consulta SQL.
        /// </summary>
        /// <returns>Devuelve un objeto ArrayList con el resultado de la consulta SQL.</returns>
        private ArrayList SelectDataReader()
        {
            var resultSelect = new ArrayList();
            foreach (var myClsObj in _validateColumns.ColumnObjects)
            {
                if (_coumnlObject == null) continue;
                //Abrimos la conexión con el data Reader para obtener los registros.
                NpgsqlDataReader reader = _con.DataReader(_validateColumns.GetQuery(QueryType.Select, myClsObj.Value));
                //Obtenemos los nombres de la columna y lo almacenamos 
                reader.GetSchemaTable();
                var nameColumns = new List<String>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    nameColumns.Add(reader.GetName(i));
                }
                //Si se han obtenidos registros los añadimos al arrayList
                if (!reader.HasRows) continue;
                while (reader.Read())
                {
                    myClsObj.Value.CreateInstance();
                    foreach (string nameColumn in nameColumns)
                    {
                        myClsObj.Value[nameColumn] = reader[nameColumn];
                    }
                    resultSelect.Add(myClsObj.Value.MyObject);
                }
            }
            return resultSelect;
        }

        #endregion
    }
}