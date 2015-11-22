/*SELECT nextval('s_idprueba'); 
SELECT currval('s_idprueba'); */

using System;
using System.Collections.Generic;
using SimpleDataMapper.Connector;
using SimpleDataMapper.Controller.ClsMemberClass;
using SimpleDataMapper.DataBase;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.Data
{
    internal class ValidateObjects : Disposable
    {
        #region [Private Properties]    

        //private readonly Dictionary<String, ClsObjectClass> _colObjects;
        /*/// <summary>
        /// Objeto que contiene una la colección de objetos de la misma clase.
        /// </summary>
        private ClsObjectClass myObjects;*/
        /// <summary>
        ///     Colección de objetos sobre los cuales se vas a realizar las operaciones de DML.
        /// </summary>
        /// <summary>
        ///     Colección de tabla a actualizar.
        /// </summary>
        private Dictionary<String, Schema> _columnsTable;

        /// <summary>
        ///     Objeto que contiene la Conexión a la base de datos.
        /// </summary>
        private Connection _connection;

        /// <summary>
        ///     Almacena la lista de campos de la tabla, los cuales coinciden con la clase. El resto de campos serán tratados por
        ///     la base de datos.
        ///     Estos campos serán posteriormente utilizados para realizar las operaciones de DML.
        /// </summary>
        private Dictionary<String, Table> _validateTables;

        #endregion

        #region

        /// <summary>
        ///     Devuelve la Colección de Objetos inicializados.
        /// </summary>
        internal Dictionary<String, ObjectClass> ColumnObjects { get; set; }

        #endregion

        /// <summary>
        ///     Constructor de la clase. Inicializa y valida los objetos con las tablas pasadas.
        /// </summary>
        /// <param name="oConnection">Objeto Conexión.</param>
        /// <param name="myColObjects">
        ///     Lista de objetos sobre los cuales se van a realizar las instrucciones DML, esto objetos deben ser pasado en el
        ///     orden en el que se
        ///     desea que actualicen a la base de datos.
        /// </param>
        internal ValidateObjects(Connection oConnection, params Object[] myColObjects)
        {
            foreach (var t in myColObjects)
            {
                _connection = oConnection;
                ColumnObjects = new Dictionary<string, ObjectClass>();
                //Inicializamos las propiedades y los campos del objeto de la clase pasada. Al objeto dictionary le ponemos como clave principal en nombre de la tabla
                var myObjectClass = new ObjectClass(t);
                ColumnObjects.Add(myObjectClass.TableName, myObjectClass);
                var myObjectTable = new Schema(myObjectClass.TableName, _connection);
                _columnsTable = new Dictionary<string, Schema> {{myObjectClass.TableName, myObjectTable}};
                ColummnsValidate();
            }
        }

        //todo:27/01/2012. He creado el método que compara los campos de la clase con las columnas los de la tablas.
        //Cuando vaya ha construir la select, update, insert o delete, Debo recorrer la lista de tablas validadas y 
        //y obtener sus valores.

        private void ColummnsValidate()
        {
            foreach (var myCol in ColumnObjects)
            {
                _validateTables = new Dictionary<String, Table>();
                List<String> validatePk = null;
                var validateColumns = new List<Column>();
                var iCountPk = 0;
                Schema oSchema;
                //Cargamos el objeto ClsShema, el cual contiene todas las colecciones de tablas.
                _columnsTable.TryGetValue(myCol.Key, out oSchema);
                //Inicializamos el objeto ClsTable, el cual contiene las propiedades de la tabla.
                if (oSchema != null)
                {
                    var table = oSchema.GetTable(myCol.Key);
                    //Obtenemos las colecciones de PrimaryKey de la tabla
                    validatePk = oSchema.GetTable(myCol.Key).ColPrimaryKey;
                    foreach (var myMember in myCol.Value.MembersOfClass)
                    {
                        //todo:Mirar si es necesario implementar que valide las primary key de la tabla y del objeto.
                        if (table.IsPrimaryKeys(myMember.Field))
                            iCountPk++;
                        //De momento se ha dejado también que se cargen sólo los campos que están en la clase.
                        //Es una totenría tratar campo que quizas no se muestren porque sean de auditoría y que la propia 
                        //base de datos se va encargar de insertar a nulo, con su valor por defecto.
                        if (!table.ThereIsColumn(myMember.Field)) continue;
                        //Obtiene la columna de la tabla y la añade a la colección.
                        var myColumn = table.GetColumn(myMember.Field);
                        if (myColumn != null)
                            validateColumns.Add(myColumn);
                    }
                    if (iCountPk != table.CountPk())
                        //todo:MEJORA. Quitar las exception y sustituir por una clase de negocio.
                        throw new ArgumentException("Los campo primary key de la tabla no están definidas en la clase",
                            ToString());
                }
                //Si la columna existe se añade al objeto tabla.
                _validateTables.Add(myCol.Key, new Table(myCol.Key, validatePk, validateColumns));
            }
            //Una Recorrido la colección de tabla
            _columnsTable = null;
        }

        private string GetValueObject(Column column, Object value)
        {
            if (value == null)
            {
                if (!column.IsNull)
                    value = "NULL";
                else if (!string.IsNullOrWhiteSpace(column.DefaultData))
                    value = column.DefaultData;
                else
                    //todo:MEJORA. Quitar las exception y sustituir por una clase de negocio.
                    throw new ArgumentException("El campo " + column.NameColumn + "No puede ser nulo", ToString());
            }
            else
                value = TypeData(column, value.ToString());
            return value.ToString();
        }

        /// <summary>
        ///     Devuelve el valor del campo con su valor correspondiente formateado.
        /// </summary>
        /// <param name="column">Objeto ClsColumn, contiene toda la información sobre la columna de la tabla</param>
        /// <param name="value">Valor para el campo</param>
        /// <returns>Devuelve el valor formateado.</returns>
        private String TypeData(Column column, String value)
        {
            string sComilla = "";
            switch (column.DataType)
            {
                case "int4":
                case "int8":
                case "int2":
                    if (General.IsNumeric(value))
                        //si viene con decimales se los quita
                        value = int.Parse(value).ToString();
                    else if (value.ToUpper() == "NULL")
                        //no hace nada (se supone que el campo admite valores NULL)
                        break;
                    else
                        value = "0";

                    break;

                case "varchar":
                case "text":
                    if (!String.IsNullOrEmpty(value))
                    {
                        //todo:MEJORA. Quitar las exception y sustituir por una clase de negocio.
                        if (value.Length > column.FieldLenght)
                            throw new ArgumentException(
                                "El valor del campo " + column.NameColumn + " ha superado la longuitud.", ToString());

                        sComilla = "'";
                    }
                    else
                        value = "NULL";
                    break;
                    //timestamp, es como para utilizar un identificador o para marcar la versión de actualización de la Columna.
                    //Si voy a trabajar con fechas mejor utilizar campos Date o DataTime.
                    //Pienso que los campos TimesTamp, se deberían usar para la auditoría.
                case "timestamp":
                case "date":
                case "timetz":
                    if (General.IsDate(value))
                    {
                        sComilla = "'";
                        //ISO 8601. Formato de las fecha utilizado por la base de datos Postgres.
                        value = DateTime.Parse(value).ToString("yyyy-MM-dd hh:mm:ss");
                    }
                    else
                        value = "NULL";

                    break;

                case "bool":
                    value = string.IsNullOrWhiteSpace(value) ? "NULL" : value.ToUpper();
                    break;
                    //Todo. Mirar como se comportan los campos númericos, para devolver una excepcion. En el caso
                    //de exceder de la longuitud máxima.
                case "float8":
                case "numeric":
                    if (General.IsNumeric(value))
                        //convierte la coma en punto
                        value = value.Replace(",", ".");
                    else if (value.ToUpper() == "NULL")
                        //no hace nada (se supone que el campo admite valores NULL)
                        break;
                    else
                        value = "0";

                    break;
            }
            return sComilla + value + sComilla;
        }


        /// <summary>
        ///     Obtenemos el Where para la tabla.
        /// </summary>
        /// <returns>Devuelve un string con la cadena instrucción Where.</returns>
        private string GetWhere(string nameTable, ObjectClass objectOfClass)
        {
            var whereSql = string.Empty;
            foreach (var pk in _validateTables[nameTable].ColPrimaryKey)
            {
                var myColumn = _validateTables[nameTable].GetColumn(pk);
                if (!string.IsNullOrWhiteSpace(whereSql)) whereSql += " AND ";
                whereSql += pk + " = " + GetValueObject(myColumn, objectOfClass[myColumn.NameColumn]);
            }
            return whereSql;
        }

        //todo:22/08/2015 poner los nombre de las tablas y columnas con comillas dobles.
        internal string GetQuery(QueryType queryType, ObjectClass objectOfClass, Object oneObject)
        {
            string fields = string.Empty, value = string.Empty;
            const string point = ",";
            string query = string.Empty, whereSql = string.Empty;
            string doubleQuotes = "\"";
            if (oneObject != null)
                objectOfClass.MyObject = oneObject;

            foreach (var myColumn in _validateTables[objectOfClass.TableName].ColColums)
            {
                //Recoremos la colección de objetos
                switch (queryType)
                {
                    case QueryType.Delete:
                        break;
                    case QueryType.Update:
                        fields = doubleQuotes + myColumn.NameColumn + doubleQuotes;
                        value = GetValueObject(myColumn, objectOfClass[myColumn.NameColumn]);
                        if (!String.IsNullOrEmpty(query))
                            query += point;
                        query +=  fields  + "=" + value;
                        break;
                    case QueryType.Insert:
                        if (!String.IsNullOrEmpty(fields))
                            fields += point;
                        if (!String.IsNullOrEmpty(value))
                            value += point;
                        fields += doubleQuotes + myColumn.NameColumn + doubleQuotes;
                        value += GetValueObject(myColumn, objectOfClass[myColumn.NameColumn]);
                        break;
                    case QueryType.Select:
                        if (!String.IsNullOrEmpty(fields))
                            fields += point;
                        if (objectOfClass[myColumn.NameColumn] != null)
                        {
                            if (!String.IsNullOrEmpty(whereSql))
                                whereSql += " AND ";
                            whereSql += doubleQuotes + myColumn.NameColumn + doubleQuotes + " = " +
                                            GetValueObject(myColumn, objectOfClass[myColumn.NameColumn]);
                        }
                        fields += doubleQuotes + myColumn.NameColumn + doubleQuotes;
                        break;
                }
            }

            switch (queryType)
            {
                case QueryType.Delete:
                    query = "DELETE FROM " + doubleQuotes + objectOfClass.TableName + doubleQuotes + " WHERE " +
                             GetWhere(objectOfClass.TableName, objectOfClass);
                    break;
                case QueryType.Update:
                    query = "UPDATE  " + doubleQuotes + objectOfClass.TableName + doubleQuotes + " SET " + query + " WHERE " +
                             GetWhere(objectOfClass.TableName, objectOfClass);
                    break;
                case QueryType.Insert:
                    query = "INSERT INTO  " + doubleQuotes + objectOfClass.TableName + doubleQuotes + "(" + fields + ")" + " VALUES (" + value +
                             ")";
                    break;
                case QueryType.Select:
                    query = "SELECT " + fields + " FROM " + doubleQuotes + objectOfClass.TableName + doubleQuotes + " WHERE " + whereSql;
                    break;
            }

            return query;
        }

        internal string GetQuery(QueryType queryType, ObjectClass objectOfClass)
        {
            return GetQuery(queryType, objectOfClass, null);
        }
    }
}