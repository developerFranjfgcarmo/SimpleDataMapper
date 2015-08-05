/*SELECT nextval('s_idprueba'); 
SELECT currval('s_idprueba'); */
using System;
using System.Collections.Generic;
using SimpleDataMapper.Connection;
using SimpleDataMapper.Controller.ClsMemberClass;
using SimpleDataMapper.DataBase;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.Data
{
    internal class ValidateObjects : Disposable
    {
        #region [Private Properties]    

        /// <summary>
        ///     Colección de objetos sobre los cuales se vas a realizar las operaciones de DML.
        /// </summary>
        //private readonly Dictionary<String, ClsObjectClass> _colObjects;

        /*/// <summary>
        /// Objeto que contiene una la colección de objetos de la misma clase.
        /// </summary>
        private ClsObjectClass myObjects;*/

        /// <summary>
        ///     Colección de tabla a actualizar.
        /// </summary>
        private Dictionary<String, Schema> _colTables;

        /// <summary>
        ///     Almacena la lista de campos de la tabla, los cuales coinciden con la clase. El resto de campos serán tratados por
        ///     la base de datos.
        ///     Estos campos serán posteriormente utilizados para realizar las operaciones de DML.
        /// </summary>
        private Dictionary<String, Table> _validateTables;

        /// <summary>
        ///     Objeto que contiene la Conexión a la base de datos.
        /// </summary>
        private ClsConnection _connection;

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
        internal ValidateObjects(ClsConnection oConnection, params Object[] myColObjects)
        {
            ClsConnection connection1;
            ClsConnection connection;
            foreach (object t in myColObjects)
            {
                this._connection = oConnection;
                this.ColumnObjects = new Dictionary<string, ObjectClass>();
                //Inicializamos las propiedades y los campos del objeto de la clase pasada. Al objeto dictionary le ponemos como clave principal en nombre de la tabla
                var myObjectClass = new ObjectClass(t);
                this.ColumnObjects.Add(myObjectClass.TableName, myObjectClass);
                var myObjectTable = new Schema(myObjectClass.TableName, this._connection);
                _colTables = new Dictionary<string, Schema>();
                _colTables.Add(myObjectClass.TableName, myObjectTable);
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
                List<String> myValidatePk = null;
                var myValidateColumns = new List<Column>();
                int iCountPK = 0;
                Schema oSchema;
                //Cargamos el objeto ClsShema, el cual contiene todas las colecciones de tablas.
                _colTables.TryGetValue(myCol.Key, out oSchema);
                //Inicializamos el objeto ClsTable, el cual contiene las propiedades de la tabla.
                Table oTable = oSchema.GetTable(myCol.Key);
                if (oSchema != null)
                {
                    //Obtenemos las colecciones de PrimaryKey de la tabla
                    myValidatePk = oSchema.GetTable(myCol.Key).ColPrimaryKey;
                    foreach (FieldClass myMember in myCol.Value.MembersOfClass)
                    {
                        //todo:Mirar si es necesario implementar que valide las primary key de la tabla y del objeto.
                        if (oTable.IsPrimaryKeys(myMember.Field))
                            iCountPK++;
                        //De momento se ha dejado también que se cargen sólo los campos que están en la clase.
                        //Es una totenría tratar campo que quizas no se muestren porque sean de auditoría y que la propia 
                        //base de datos se va encargar de insertar a nulo, con su valor por defecto.
                        if (oTable.ThereIsColumn(myMember.Field))
                        {
                            //Obtiene la columna de la tabla y la añade a la colección.
                            Column myColumn = oTable.GetColumn(myMember.Field);
                            if (myColumn != null)
                                myValidateColumns.Add(myColumn);
                        }
                    }
                }
                if (iCountPK != oTable.CountPk())
                    //todo:MEJORA. Quitar las exception y sustituir por una clase de negocio.
                    throw new ArgumentException("Los campo primary key de la tabla no están definidas en la clase",
                        ToString());
                //Si la columna existe se añade al objeto tabla.
                if (myValidateColumns != null)
                    _validateTables.Add(myCol.Key, new Table(myCol.Key, myValidatePk, myValidateColumns));
            }
            //Una Recorrido la colección de tabla
            _colTables = null;
        }

        private String GetValueObject(Column oColumn, Object sValue)
        {
            if (sValue == null)
            {
                if (!oColumn.IsNull)
                    sValue = "NULL";
                else if (!String.IsNullOrEmpty(oColumn.DefaultData))
                    sValue = oColumn.DefaultData;
                else
                    //todo:MEJORA. Quitar las exception y sustituir por una clase de negocio.
                    throw new ArgumentException("El campo " + oColumn.NameColumn + "No puede ser nulo", ToString());
            }
            else
                sValue = TypeData(oColumn, sValue.ToString());
            return sValue.ToString();
        }

        /// <summary>
        ///     Devuelve el valor del campo con su valor correspondiente formateado.
        /// </summary>
        /// <param name="oColumn">Objeto ClsColumn, contiene toda la información sobre la columna de la tabla</param>
        /// <param name="sValue">Valor para el campo</param>
        /// <returns>Devuelve el valor formateado.</returns>
        private String TypeData(Column oColumn, String sValue)
        {
            String sComilla = "";
            switch (oColumn.DataType)
            {
                case "int4":
                case "int8":
                case "int2":
                    if (ClsGeneral.IsNumeric(sValue))
                        //si viene con decimales se los quita
                        sValue = int.Parse(sValue).ToString();
                    else if (sValue.ToUpper() == "NULL")
                        //no hace nada (se supone que el campo admite valores NULL)
                        break;
                    else
                        sValue = "0";

                    break;

                case "varchar":
                case "text":
                    if (!String.IsNullOrEmpty(sValue))
                    {
                        //todo:MEJORA. Quitar las exception y sustituir por una clase de negocio.
                        if (sValue.Length > oColumn.FieldLenght)
                            throw new ArgumentException(
                                "El valor del campo " + oColumn.NameColumn + " ha superado la longuitud.", ToString());

                        sComilla = "'";
                    }
                    else
                        sValue = "NULL";
                    break;
                    //timestamp, es como para utilizar un identificador o para marcar la versión de actualización de la Columna.
                    //Si voy a trabajar con fechas mejor utilizar campos Date o DataTime.
                    //Pienso que los campos TimesTamp, se deberían usar para la auditoría.
                case "timestamp":
                case "date":
                case "timetz":
                    if (ClsGeneral.IsDate(sValue))
                    {
                        sComilla = "'";
                        //ISO 8601. Formato de las fecha utilizado por la base de datos Postgres.
                        sValue = DateTime.Parse(sValue).ToString("yyyy-MM-dd hh:mm:ss");
                    }
                    else
                        sValue = "NULL";

                    break;

                case "bool":
                    if (String.IsNullOrEmpty(sValue))
                        sValue = "NULL";
                    else
                        sValue = sValue.ToUpper();
                    break;
                    //Todo. Mirar como se comportan los campos númericos, para devolver una excepcion. En el caso
                    //de exceder de la longuitud máxima.
                case "float8":
                case "numeric":
                    if (ClsGeneral.IsNumeric(sValue))
                        //convierte la coma en punto
                        sValue = sValue.Replace(",", ".");
                    else if (sValue.ToUpper() == "NULL")
                        //no hace nada (se supone que el campo admite valores NULL)
                        break;
                    else
                        sValue = "0";

                    break;
            }
            return sComilla + sValue + sComilla;
        }


        /// <summary>
        ///     Obtenemos el Where para la tabla.
        /// </summary>
        /// <returns>Devuelve un string con la cadena instrucción Where.</returns>
        private String GetWhere(String sNameTable, ObjectClass objCls)
        {
            String sWhere = "";
            foreach (String sPK in _validateTables[sNameTable].ColPrimaryKey)
            {
                Column myColumn = _validateTables[sNameTable].GetColumn(sPK);
                if (!String.IsNullOrEmpty(sWhere)) sWhere += " AND ";
                sWhere += sPK + " = " + GetValueObject(myColumn, objCls[myColumn.NameColumn]);
            }
            return sWhere;
        }


        internal String GetQuery(QueryType eQueryType, ObjectClass objCls, Object obj)
        {
            String sFields = "", sValue = "", sComa = ",", sQuery = "", sWhereSelect = "";

            if (obj != null)
                objCls.MyObject = obj;

            foreach (Column myColumn in _validateTables[objCls.TableName].ColColums)
            {
                //Recoremos la colección de objetos
                switch (eQueryType)
                {
                    case QueryType.Delete:
                        break;
                    case QueryType.Update:
                        sFields = myColumn.NameColumn;
                        sValue = GetValueObject(myColumn, objCls[myColumn.NameColumn]);
                        if (!String.IsNullOrEmpty(sQuery))
                            sQuery += sComa;
                        sQuery += sFields + "=" + sValue;
                        break;
                    case QueryType.Insert:
                        if (!String.IsNullOrEmpty(sFields))
                            sFields += sComa;
                        if (!String.IsNullOrEmpty(sValue))
                            sValue += sComa;
                        sFields += myColumn.NameColumn;
                        sValue += GetValueObject(myColumn, objCls[myColumn.NameColumn]);
                        break;
                    case QueryType.Select:
                        if (!String.IsNullOrEmpty(sFields))
                            sFields += sComa;
                        if (objCls[myColumn.NameColumn] != null)
                        {
                            if (!String.IsNullOrEmpty(sWhereSelect))
                                sWhereSelect += " AND ";
                            sWhereSelect += myColumn.NameColumn + " = " +
                                            GetValueObject(myColumn, objCls[myColumn.NameColumn]);
                        }
                        sFields += myColumn.NameColumn;
                        break;
                }
            }

            switch (eQueryType)
            {
                case QueryType.Delete:
                    sQuery = "DELETE FROM " + objCls.TableName + " WHERE " + GetWhere(objCls.TableName, objCls);
                    break;
                case QueryType.Update:
                    sQuery = "UPDATE  " + objCls.TableName + " SET " + sQuery + " WHERE " +
                             GetWhere(objCls.TableName, objCls);
                    break;
                case QueryType.Insert:
                    sQuery = "INSERT INTO  " + objCls.TableName + "(" + sFields + ")" + " VALUES (" + sValue + ")";
                    break;
                case QueryType.Select:
                    sQuery = "SELECT " + sFields + " FROM " + objCls.TableName + " WHERE " + sWhereSelect;
                    break;
            }

            return sQuery;
        }

        internal String GetQuery(QueryType eQueryType, ObjectClass objCls)
        {
            return GetQuery(eQueryType, objCls, null);
        }
    }
}