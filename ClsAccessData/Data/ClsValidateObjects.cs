/*SELECT nextval('s_idprueba'); 
SELECT currval('s_idprueba'); */
using System;
using System.Collections.Generic;
using SimpleDataMapper.Connection;
using SimpleDataMapper.Controller.ClsMemberClass;
using SimpleDataMapper.Schema;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.Data
{
    internal class ClsValidateObjects : ClsDisposable
    {
        #region Declaración de campos.

        /// <summary>
        ///     Tipos de Query para atacar la base datos.
        /// </summary>
        public enum QueryType
        {
            /// <summary>
            ///     Transacción de actualización.
            /// </summary>
            UPDATE,

            /// <summary>
            ///     Transacción de inserción.
            /// </summary>
            INSERT,

            /// <summary>
            ///     Transacción de borrado.
            /// </summary>
            DELETE,

            /// <summary>
            ///     Transacción de Obtención de registro.
            /// </summary>
            SELECT
        }

        /// <summary>
        ///     Colección de objetos sobre los cuales se vas a realizar las operaciones de DML.
        /// </summary>
        private readonly Dictionary<String, ClsObjectClass> myColObjects;

        /*/// <summary>
        /// Objeto que contiene una la colección de objetos de la misma clase.
        /// </summary>
        private ClsObjectClass myObjects;*/

        /// <summary>
        ///     Colección de tabla a actualizar.
        /// </summary>
        private Dictionary<String, ClsSchema> myColTables;

        /// <summary>
        ///     Almacena la lista de campos de la tabla, los cuales coinciden con la clase. El resto de campos serán tratados por
        ///     la base de datos.
        ///     Estos campos serán posteriormente utilizados para realizar las operaciones de DML.
        /// </summary>
        private Dictionary<String, ClsTable> myValidateTables;

        /// <summary>
        ///     Objeto que contiene la Conexión a la base de datos.
        /// </summary>
        private ClsConnection oConnection;

        #endregion

        #region

        /// <summary>
        ///     Devuelve la Colección de Objetos inicializados.
        /// </summary>
        internal Dictionary<String, ClsObjectClass> MyColObjects
        {
            get { return myColObjects; }
        }

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
        internal ClsValidateObjects(ClsConnection oConnection, params Object[] myColObjects)
        {
            for (int i = 0; i < myColObjects.Length; i++)
            {
                this.oConnection = oConnection;
                this.myColObjects = new Dictionary<string, ClsObjectClass>();
                //Inicializamos las propiedades y los campos del objeto de la clase pasada. Al objeto dictionary le ponemos como clave principal en nombre de la tabla
                var myObjectClass = new ClsObjectClass(myColObjects[i]);
                this.myColObjects.Add(myObjectClass.STableName, myObjectClass);
                var myObjectTable = new ClsSchema(myObjectClass.STableName, this.oConnection);
                myColTables = new Dictionary<string, ClsSchema>();
                myColTables.Add(myObjectClass.STableName, myObjectTable);
                ColummnsValidate();
            }
        }

        //todo:27/01/2012. He creado el método que compara los campos de la clase con las columnas los de la tablas.
        //Cuando vaya ha construir la select, update, insert o delete, Debo recorrer la lista de tablas validadas y 
        //y obtener sus valores.

        private void ColummnsValidate()
        {
            foreach (var myCol in MyColObjects)
            {
                myValidateTables = new Dictionary<String, ClsTable>();
                List<String> myValidatePk = null;
                var myValidateColumns = new List<ClsColumn>();
                int iCountPK = 0;
                ClsSchema oSchema;
                //Cargamos el objeto ClsShema, el cual contiene todas las colecciones de tablas.
                myColTables.TryGetValue(myCol.Key, out oSchema);
                //Inicializamos el objeto ClsTable, el cual contiene las propiedades de la tabla.
                ClsTable oTable = oSchema.GetTable(myCol.Key);
                if (oSchema != null)
                {
                    //Obtenemos las colecciones de PrimaryKey de la tabla
                    myValidatePk = oSchema.GetTable(myCol.Key).ColPrimaryKey;
                    foreach (ClsFieldClass myMember in myCol.Value.OMyMembersClass)
                    {
                        //todo:Mirar si es necesario implementar que valide las primary key de la tabla y del objeto.
                        if (oTable.IsPrimaryKeys(myMember.SField))
                            iCountPK++;
                        //De momento se ha dejado también que se cargen sólo los campos que están en la clase.
                        //Es una totenría tratar campo que quizas no se muestren porque sean de auditoría y que la propia 
                        //base de datos se va encargar de insertar a nulo, con su valor por defecto.
                        if (oTable.ThereIsColumn(myMember.SField))
                        {
                            //Obtiene la columna de la tabla y la añade a la colección.
                            ClsColumn myColumn = oTable.GetColumn(myMember.SField);
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
                    myValidateTables.Add(myCol.Key, new ClsTable(myCol.Key, myValidatePk, myValidateColumns));
            }
            //Una Recorrido la colección de tabla
            myColTables = null;
        }

        private String GetValueObject(ClsColumn oColumn, Object sValue)
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
        private String TypeData(ClsColumn oColumn, String sValue)
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
        private String GetWhere(String sNameTable, ClsObjectClass objCls)
        {
            String sWhere = "";
            foreach (String sPK in myValidateTables[sNameTable].ColPrimaryKey)
            {
                ClsColumn myColumn = myValidateTables[sNameTable].GetColumn(sPK);
                if (!String.IsNullOrEmpty(sWhere)) sWhere += " AND ";
                sWhere += sPK + " = " + GetValueObject(myColumn, objCls[myColumn.NameColumn]);
            }
            return sWhere;
        }


        internal String GetQuery(QueryType eQueryType, ClsObjectClass objCls, Object obj)
        {
            String sFields = "", sValue = "", sComa = ",", sQuery = "", sWhereSelect = "";

            if (obj != null)
                objCls.MyObject = obj;

            foreach (ClsColumn myColumn in myValidateTables[objCls.STableName].ColColums)
            {
                //Recoremos la colección de objetos
                switch (eQueryType)
                {
                    case QueryType.DELETE:
                        break;
                    case QueryType.UPDATE:
                        sFields = myColumn.NameColumn;
                        sValue = GetValueObject(myColumn, objCls[myColumn.NameColumn]);
                        if (!String.IsNullOrEmpty(sQuery))
                            sQuery += sComa;
                        sQuery += sFields + "=" + sValue;
                        break;
                    case QueryType.INSERT:
                        if (!String.IsNullOrEmpty(sFields))
                            sFields += sComa;
                        if (!String.IsNullOrEmpty(sValue))
                            sValue += sComa;
                        sFields += myColumn.NameColumn;
                        sValue += GetValueObject(myColumn, objCls[myColumn.NameColumn]);
                        break;
                    case QueryType.SELECT:
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
                case QueryType.DELETE:
                    sQuery = "DELETE FROM " + objCls.STableName + " WHERE " + GetWhere(objCls.STableName, objCls);
                    break;
                case QueryType.UPDATE:
                    sQuery = "UPDATE  " + objCls.STableName + " SET " + sQuery + " WHERE " +
                             GetWhere(objCls.STableName, objCls);
                    break;
                case QueryType.INSERT:
                    sQuery = "INSERT INTO  " + objCls.STableName + "(" + sFields + ")" + " VALUES (" + sValue + ")";
                    break;
                case QueryType.SELECT:
                    sQuery = "SELECT " + sFields + " FROM " + objCls.STableName + " WHERE " + sWhereSelect;
                    break;
            }

            return sQuery;
        }

        internal String GetQuery(QueryType eQueryType, ClsObjectClass objCls)
        {
            return GetQuery(eQueryType, objCls, null);
        }
    }
}