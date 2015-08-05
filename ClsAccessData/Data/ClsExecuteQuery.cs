using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Npgsql;

//Todo: Mirar a crear otro método sobrecargado de la Select en el que se pase el Objeto sWhere y el Objeto a devolver.
//Todo: Mirar a crear una sobre carga de los métodos update, delete, in-
using SimpleDataMapper.Connection;

namespace SimpleDataMapper.Data
{
    /// <summary>
    ///     Clase que realiza todas las llamadas instrucciónes de DML sobre la base de datos.
    /// </summary>
    public class ClsExecuteQuery
    {
        #region Declaracion de variables.

        /// <summary>
        ///     Almacena la conexión a la base de datos.
        /// </summary>
        private readonly ClsConnection oCon;

        /// <summary>
        ///     Almacena una colección de objetos de la misma clase.
        /// </summary>
        private List<Object> listObject;

        /// <summary>
        ///     Valida el Objeto con la tabla correspondiente.
        /// </summary>
        private ValidateObjects myValidateObject;

        /// <summary>
        ///     Almacena una colección de objetos de diferentes clases pasados por parámetros.
        /// </summary>
        private Object[] oColObject;

        private Object objectWhere;

        /// <summary>
        ///     Contiene la función delegado a la que tiene que apuntar para devolver la lista de resultados.
        /// </summary>
        /// <returns></returns>
        private delegate ArrayList DelegadoSelect();

        #endregion

        #region Declaración de Contructores.

        /// <summary>
        ///     Constructor de la clase ClsExecuteQuery.
        /// </summary>
        /// <param name="oCon">Instancia al Objeto conexión de la base de datos.</param>
        public ClsExecuteQuery(ClsConnection oCon)
        {
            this.oCon = oCon;
        }

        #endregion

        #region Declaración de métodos públicos.

        /// <summary>
        ///     Inserción de registros de la tabla.
        /// </summary>
        /// <param name="myObject">
        ///     Instancia de una lista de objetos de una clase con la que se compone la instrucción de Insert para realizar
        ///     las diferentes inserciones. Los miembros de la clase deben ser System.Nullable.
        /// </param>
        public void Insert(List<Object> myObject)
        {
            IniParamDml(myObject);
            ExecuteQuery(QueryType.Insert);
        }

        /// <summary>
        ///     Inserción de registros de la tabla.
        /// </summary>
        /// <param name="myObject">
        ///     Instancia de una clase o instancias de diferentes clase con la que se compone la instrucción de Insert para
        ///     realizar
        ///     las diferentes inserciones. Los miembros de la clase deben ser System.Nullable. Cada Objeto debe ser de una clase
        ///     diferente y se debe pasar en el
        ///     orden en el que se desea realizar cada operación, se debe tener cuidado con las foreing Key de las tablas.
        /// </param>
        public void Insert(params Object[] myObject)
        {
            IniParamDml(myObject);
            ExecuteQuery(QueryType.Insert);
        }

        /// <summary>
        ///     Actualización de registros de la tabla.
        /// </summary>
        /// <param name="obj">
        ///     Instancia de una lista de objetos de una clase con la que se compone la instrucción de Update para realizar
        ///     las diferentes actualizaciones. Los miembros de la clase deben ser System.Nullable.
        /// </param>
        public void Update(List<Object> myObject)
        {
            IniParamDml(myObject);
            ExecuteQuery(QueryType.Update);
        }

        /// <summary>
        ///     Actualización de registros de la tabla.
        /// </summary>
        /// <param name="obj">
        ///     Instancia de una clase o  instancias de diferentes clase con la que se compone la instrucción de Update para
        ///     realizar
        ///     las diferentes actualizaciones. Los miembros de la clase deben ser System.Nullable. Cada Objeto debe ser de una
        ///     clase diferente y se debe pasar en el
        ///     orden en el que se desea realizar cada operación, es decir se debe tener cuidado con las foreing Key de las tablas.
        /// </param>
        public void Update(params Object[] myObject)
        {
            IniParamDml(myObject);
            ExecuteQuery(QueryType.Update);
        }

        /// <summary>
        ///     Elimina los registros de la tabla.
        /// </summary>
        /// <param name="obj">
        ///     Instancia de una clase o instacias de diferentes clases con la que se compone la instrucción DELETE para eliminar
        ///     los registros de cada tabla. Los miembros de la clase deben System.Nullable. Cada Objeto debe ser de una clase
        ///     diferente y se debe pasar en el
        ///     orden en el que se desea realizar cada operación, es decir se debe tener cuidado con las foreing Key de las tablas.
        /// </param>
        public void Delete(params Object[] myObject)
        {
            IniParamDml(myObject);
            ExecuteQuery(QueryType.Delete);
        }

        /// <summary>
        ///     Elimina los registros de la tabla.
        /// </summary>
        /// <param name="obj">
        ///     Array de un de diferentes instancias de una clase, con la que se compone cada instrucción DELETE para eliminar
        ///     un registro de tabla. Los miembros de la clase deben System.Nullable.
        /// </param>
        public void Delete(List<Object> myObject)
        {
            IniParamDml(myObject);
            ExecuteQuery(QueryType.Delete);
        }

        /// <summary>
        ///     Obtiene el conjunto de resultados de una consulta Select en función del Objeto pasado.
        /// </summary>
        /// <param name="myObject">Objeto que se utilizará como filtro de la consulta Select y como resultado del Objeto ArrayList.</param>
        /// <returns>Devuelve un ArrayList del objeto pasado por parámetro.</returns>
        public ArrayList Select(Object myObject)
        {
            IniParamDml(myObject);
            return SelectQuery(SelectDataReader);
        }

        /// <summary>
        ///     Obtiene el conjunto de resultados de una consulta Select en función del Objeto pasado.
        /// </summary>
        /// <param name="myObject">Objeto que se utilizará como filtro de la consulta Select y como resultado del Objeto ArrayList.</param>
        /// <returns>Devuelve un ArrayList del objeto pasado por parámetro.</returns>
        public ArrayList SelectDataSet(Object myObject)
        {
            IniParamDml(myObject);
            return SelectQuery(SelectDataSet);
        }

        #endregion

        #region Declaración de métodos públicos.

        /// <summary>
        ///     Inicializa el objeto para la instrucción DML.
        /// </summary>
        /// <param name="myObject">Lista de Objetos para realizar la instrucción DML.</param>
        private void IniParamDml(params Object[] myObject)
        {
            oColObject = myObject;
            myValidateObject = new ValidateObjects(oCon, oColObject);
        }

        /// <summary>
        ///     Inicializa el objeto para la instrucción DML.
        /// </summary>
        /// <param name="myObject">Lista de Objetos para realizar la instrucción DML.</param>
        private void IniParamDml(List<Object> myObject)
        {
            listObject = myObject;
            listObject.Add(myObject);
            myValidateObject = new ValidateObjects(oCon, listObject[0]);
        }

        /// <summary>
        ///     Ejecuta la culsuta de UPDATE,INSERT,DELETE.
        /// </summary>
        /// <param name="eTipoQuery">Indica el tipo de operación.</param>
        /// <param name="obj">Indica el tipo de operación.</param>
        private void ExecuteQuery(QueryType eTipoQuery)
        {
            try
            {
                if (oCon.Status() == ConnectionState.Closed)
                    oCon.DbOpen();
                oCon.BeginTransaction();

                foreach (var myClsObj in myValidateObject.ColumnObjects)
                {
                    if (oColObject != null)
                    {
                        //Ejecutamos la transacción a la base de datos.
                        oCon.Execute(myValidateObject.GetQuery(eTipoQuery, myClsObj.Value));
                    }
                    else
                    {
                        foreach (Object myObj in listObject)
                        {
                            oCon.Execute(myValidateObject.GetQuery(eTipoQuery, myClsObj.Value, myObj));
                        }
                    }
                }
                oCon.Commit();
            }
            catch (NpgsqlException ex)
            {
                //todo:MEJORA. Quitar las exception y sustituir por una clase de negocio.
                oCon.RollBack();
                throw new ArgumentException(ex.ToString(), ToString());
            }
            finally
            {
                oCon.DbClose();
            }
        }

        /// <summary>
        ///     Devuelve un arraylist obtenidos a través de una consulta SQL.
        /// </summary>
        /// <param name="funDel">Nombre de la función delegado que debe de ejecutar.</param>
        /// <returns>Devuelve un objeto ArrayList con el resultado de la consulta SQL.</returns>
        private ArrayList SelectQuery(DelegadoSelect funDel)
        {
            ArrayList ResultSelect;
            try
            {
                if (oCon.Status() == ConnectionState.Closed)
                    oCon.DbOpen();
                DelegadoSelect MyDel = funDel;
                ResultSelect = MyDel();
                oCon.DbClose();
            }
            catch (NpgsqlException ex)
            {
                throw new ArgumentException(ex.ToString(), ToString());
            }
            finally
            {
                oCon.DbClose();
            }
            return ResultSelect;
        }

        /// <summary>
        ///     Devuelve un arraylist obtenidos a través de una consulta SQL.
        /// </summary>
        /// <returns>Devuelve un objeto ArrayList con el resultado de la consulta SQL.</returns>
        private ArrayList SelectDataSet()
        {
            var ResultSelect = new ArrayList();
            DataSet dtTablaSelect = null;
            foreach (var myClsObj in myValidateObject.ColumnObjects)
            {
                if (oColObject != null)
                {
                    dtTablaSelect =
                        oCon.InitDataAdapter(myValidateObject.GetQuery(QueryType.Select,
                            myClsObj.Value));
                    foreach (DataRow row in dtTablaSelect.Tables[0].Rows)
                    {
                        myClsObj.Value.CreateInstance();
                        foreach (DataColumn col in dtTablaSelect.Tables[0].Columns)
                        {
                            myClsObj.Value[col.ColumnName] = row[col.ColumnName];
                        }
                        ResultSelect.Add(myClsObj.Value.MyObject);
                    }
                }
            }
            return ResultSelect;
        }

        /// <summary>
        ///     Devuelve un arraylist obtenidos a través de una consulta SQL.
        /// </summary>
        /// <returns>Devuelve un objeto ArrayList con el resultado de la consulta SQL.</returns>
        private ArrayList SelectDataReader()
        {
            var ResultSelect = new ArrayList();
            foreach (var myClsObj in myValidateObject.ColumnObjects)
            {
                if (oColObject != null)
                {
                    //Abrimos la conexión con el data Reader para obtener los registros.
                    NpgsqlDataReader reader =
                        oCon.DataReader(myValidateObject.GetQuery(QueryType.Select, myClsObj.Value));
                    //Obtenemos los nombres de la columna y lo almacenamos 
                    DataTable dt = reader.GetSchemaTable();
                    var nameColumns = new List<String>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        nameColumns.Add(reader.GetName(i));
                    }
                    //Si se han obtenidos registros los añadimos al arrayList
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            myClsObj.Value.CreateInstance();
                            foreach (String nameColumn in nameColumns)
                            {
                                myClsObj.Value[nameColumn] = reader[nameColumn];
                            }
                            ResultSelect.Add(myClsObj.Value.MyObject);
                        }
                    }
                }
            }
            return ResultSelect;
        }

        #endregion
    }
}