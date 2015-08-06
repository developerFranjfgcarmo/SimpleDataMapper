using System;
using System.Collections;
using System.Data;
using System.Reflection;

//TODO: SEGUIR MIRANDO LOS COMENTARIOS DE LOS DATOS Y MÉTODOS MIEMBROS DE LA CLASE.seguir mirando
//TODO:MIRAR LA CONSTRUCCIÓN DE LA QUERY.en proceso
//TODO:MIRAR PARA ALMACENAR LA TABLA A LA QUE SE ESTA ACCEDIENDO EN ESTE MOMENTO.listo
//todo:COMPROBAR EL FORMATO DE LOS CAMPOS Y EL TIPO DATO.
//todo:25/04/2009  Revisar solo los campos que pertenezcan a la tabla, el resto de campos de las clases no se deben mirar.
//todo:25/04/2009 , Clase dispose.Listo.
//todo: Controlar que no se haga Rollback, commit  cuando se llama al método Select.
//todo: Controlar a tener una cache donde se carge el esquema de tabla.


//Todo. Controlar errores de unique, contraint, triggers.
using SimpleDataMapper.Connection;

namespace SimpleDataMapper
{
    /// <summary>
    ///     adfsdf
    /// </summary>
    public class ClsStructure
    {
        /// <summary>
        ///     Tipos de propiedades de los campos.
        /// </summary>
        public enum PropertyType
        {
            /// <summary>
            ///     Tipo de dato.
            /// </summary>
            DataType,

            /// <summary>
            ///     Longuitud del campo.
            /// </summary>
            FieldLength,

            /// <summary>
            ///     Indica si permite nulos el campo.
            /// </summary>
            ISNull,

            /// <summary>
            ///     Valor por defecto del campo.
            /// </summary>
            DefaultData,

            /// <summary>
            ///     Indica si el campo es primary key.
            /// </summary>
            PrimaryKey,

            /// <summary>
            ///     Contiene el numero de campos clave de una tabla.
            /// </summary>
            CountPrimarykeys
        };

        /// <summary>
        ///     Nombre de la variable que contiene el nombre de la tabla a realizar la transacción.
        /// </summary>
        private const String NAME_TABLE = "BASETABLE";

        private DataSet dtTablaSelect;

        /// <summary>
        ///     Collección de Columnas de la tabla actual
        /// </summary>
        private Hashtable oColumns;

        /// <summary>
        ///     Almacena la conexión a la base datos.
        /// </summary>
        private ClsConnection oCon;

        /// <summary>
        ///     Contiene la coleción de columnas claves y su correspondiente valor.
        /// </summary>
        private Hashtable oKeysColumns;

        /// <summary>
        ///     Contiene la estrutura de una tabla o de todas las tablas de la base datos.
        /// </summary>
        private Hashtable oSchemaStruct;

        /// <summary>
        ///     Almacena las Primary Keys de la tabla.
        /// </summary>
        private String[] sPrimaryKeys;

        /// <summary>
        ///     Nombre de la tabla actual de trabajo.
        /// </summary>
        private String sTableName;

        /// <summary>
        ///     Constructor de la clase.
        /// </summary>
        /// <param name="oCon">Conexión de la base datos.</param>
        public ClsStructure(ClsConnection oCon)
        {
            this.oCon = oCon;
            LoadSchema();
        }

        /// <summary>
        ///     Constructor de la clase.
        /// </summary>
        /// <param name="oCon">Conexión de la base datos.</param>
        /// <param name="sNameTable">Nombre de la tabla para obtener el esquema.</param>
        public ClsStructure(ClsConnection oCon, String sNameTable)
        {
            this.oCon = oCon;
            LoadSchema(sNameTable);
        }

        /// <summary>
        ///     Collección de Columnas de la tabla actualmente inializada.
        /// </summary>
        private Hashtable OColumns
        {
            get { return oColumns; }
            set { oColumns = value; }
        }

        /// <summary>
        ///     Carga el esquema de la base de datos.
        /// </summary>
        private void LoadSchema()
        {
            Hashtable oColTable;
            String sNameTable = "";
            DataTable dtShema;
            try
            {
                //Abrimos la conexión.
                oCon.DbOpen();
                oSchemaStruct = new Hashtable();
                //Para ver información de las diferentes colecciones de datos devueltas por el método GetSchema:
                // http://msdn2.microsoft.com/es-es/library/ms254969(VS.80).aspx
                // http://msdn2.microsoft.com/es-es/library/ms254501(VS.80).aspx

                //Obtenemos todas las tablas de base datos.
                dtShema = oCon.Connection.GetSchema("Tables", new String[] {null, "public", null, "BASE TABLE"});
                foreach (DataRow rowSchema in dtShema.Rows)
                {
                    oColTable = new Hashtable();
                    foreach (DataColumn columsSchema in dtShema.Columns)
                    {
                        // Console.WriteLine(colSchema.ColumnName + ": " + rowSchema[colSchema].ToString());

                        //Comprobamos si ColumnName es igual table_name, es el nombre de tabla.         
                        switch (columsSchema.ColumnName)
                        {
                            case "table_name":
                                //Almacenamos el nombre de la tabla en una variable para poder almacenarlo en el hashtable.
                                sNameTable = rowSchema[columsSchema].ToString();
                                oColTable = LoadTable(sNameTable);

                                break;
                        }
                    }
                    //Añadimos la tabla, con las propiedades de sus columnas.                                         
                    oSchemaStruct.Add(sNameTable, oColTable);
                    oColTable = null;
                }
            }
            catch (Exception ex)
            {
                ClsTraccer.RunException(ex, "Error al inicializar el esquema de la tabla " + sNameTable,
                    "LoadSchema overloads1");
            }
        }

        /// <summary>
        ///     Carga el esquema de una tabla de la base de datos.
        /// </summary>
        /// <param name="sNameTable"></param>
        private void LoadSchema(String sNameTable)
        {
            try
            {
                oSchemaStruct.Add(sNameTable, LoadTable(sNameTable));
            }
            catch (Exception ex)
            {
                ClsTraccer.RunException(ex, "Error al inicializar el esquema de la tabla " + sNameTable,
                    "LoadSchema overloads2");
            }
        }

        /// <summary>
        ///     Carga la estructura de una tabla.
        /// </summary>
        /// <param name="sNombreTable">Nombre de la tabla.</param>
        /// <returns></returns>
        private Hashtable LoadTable(String sNombreTable)
        {
            var cTableShema = new Hashtable();
            DataTable dtTable;
            Hashtable cRowSchema;
            int iPrimaryKeys = 0;

            dtTable = oCon.Connection.GetSchema("Columns", new String[] {null, null, sNombreTable, null});
            // GetPrimaryKey(sNombreTable);
            //Recorremos todos los registros, para obtener las propiedades de cada una de las columnas de la tabla.
            foreach (DataRow row in dtTable.Rows)
            {
                cRowSchema = new Hashtable();
                //recorre cada propiedad del campo 
                foreach (DataColumn col in dtTable.Columns)
                {
                    //Console.WriteLine(col.ColumnName + ": " + row[col].ToString());
                    switch (col.ColumnName.ToUpper())
                    {
                        case "DATA_TYPE": //Tipo de datos
                            cRowSchema.Add(PropertyType.DataType.ToString(), row[col].ToString());
                            break;

                        case "CHARACTER_MAXIMUM_LENGTH": //Longitud del campo
                            cRowSchema.Add(PropertyType.FieldLength.ToString(),
                                (row[col].ToString() == "") ? "0" : row[col].ToString());
                            break;

                        case "IS_NULLABLE": //Permite valores nulos
                            cRowSchema.Add(PropertyType.ISNull.ToString(), row[col].ToString());
                            break;

                        case "COLUMN_DEFAULT": //Valor por defecto
                            cRowSchema.Add(PropertyType.DefaultData.ToString(),
                                (row[col].ToString() == "") ? "Null" : row[col].ToString());
                            break;
                        case "COLUMN_NAME": //Nombre del campo
                            cRowSchema.Add("NombreCampo", row[col].ToString());
                            if (IsKey(row[col].ToString()))
                            {
                                cRowSchema.Add(PropertyType.PrimaryKey.ToString(), "YES");
                                iPrimaryKeys = +1;
                            }
                            else
                                cRowSchema.Add(PropertyType.PrimaryKey.ToString(), "NO");
                            break;
                    }
                }
                //Añadimos la propidades de las columnas de la tabla.
                cTableShema.Add(cRowSchema["NombreCampo"].ToString(), cRowSchema);
                cRowSchema = null;
            }
            cTableShema.Add(PropertyType.CountPrimarykeys.ToString(), iPrimaryKeys);
            //Inicializamos el array sPrimaryKeys
            sPrimaryKeys = null;
            return cTableShema;
        }


        /// <summary>
        ///     Compruba si el campo es clave primaria
        /// </summary>
        /// <param name="sColumn">Nombre del campo</param>
        /// <returns></returns>
        private bool IsKey(String sColumn)
        {
            if (sPrimaryKeys is Array)
            {
                for (int i = 0; i <= sPrimaryKeys.Length - 1; i++)
                {
                    if (sPrimaryKeys[i].ToUpper() == sColumn.ToUpper())
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Devuelve la propiedad del campo especificada
        /// </summary>
        /// <param name="sNombreCampo">
        ///     Nombre del campo a buscar
        /// </param>
        /// <param name="oPropiedad">
        ///     Propiedad que se va a devolver
        /// </param>
        /// <returns>
        ///     Devuelve el valor de la propiedad
        /// </returns>
        public String GetProperty(String sNombreCampo, PropertyType oPropiedad)
        {
//obtiene el campo a buscar
            var oProperty = (Hashtable) OColumns[sNombreCampo];
            //devuelve el valor de la propiedad especificada del campo 
            return oProperty[oPropiedad.ToString()].ToString();
        }

        /// <summary>
        ///     Devuelve la consulta de actualización, borrado o inserción a la tabla.
        /// </summary>
        /// <param name="oType">Tipo se operación DELETE, UPDATE, INSERT, SELECT</param>
        /// <param name="obj">Colección de valores a actualizar a la tabla.</param>
        private void GetQuery(QueryType oType, Object obj)
        {
            String sFields = "", sValor = "", sQuery = "", sComa = ",";
            FieldInfo[] myFieldInfo;
            Type myType = obj.GetType();
            myFieldInfo = myType.GetFields(); /* (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                            BindingFlags.InvokeMethod | BindingFlags.PutDispProperty | BindingFlags.GetProperty );*/
            //Recorremos la colección de objetos.

            for (int i = 0; i < myFieldInfo.Length; i++)
            {
                //Comprobamos que la lista de elementos no sea el nombre de la tabla.
                // myFieldInfo[i].a
                //Type myTypeProp = myType.GetProperties(;

                if (myFieldInfo[i].Name.ToUpper() != NAME_TABLE)
                {
                    //Dependiento el tipo de operación vamos construyendo la consultas.
                    switch (oType)
                    {
                        case QueryType.UPDATE:
                            //Obtenemos los campos de la base de datos y los valores para esos campos.
                            sFields = myFieldInfo[i].Name.ToString();
                            sValor = GetData(myFieldInfo[i].Name.ToString(), myFieldInfo[i].GetValue(obj));
                            //.GetValue(obj,BindingFlags.GetProperty,null,null,null));
                            //Solo debemos obtener los valores para aquellos campos que no sean claves.
                            if (!oKeysColumns.ContainsKey(sFields))
                                if (!String.IsNullOrEmpty(sQuery)) sQuery += sComa;
                            sQuery += sFields + " = " + sValor;

                            break;
                        case QueryType.INSERT:
                            if (!String.IsNullOrEmpty(sFields)) sFields += sComa;
                            if (!String.IsNullOrEmpty(sValor)) sValor += sComa;
                            //Obtenemos los campos de la base de datos y los valores para esos campos.
                            sFields += myFieldInfo[i].Name.ToString();
                            //obtenemos el valor a actualizar con el formato correspondiente.
                            sValor += GetData(myFieldInfo[i].Name.ToString(), myFieldInfo[i].GetValue(obj));
                            //GetValue(obj,BindingFlags.GetProperty,null,null,null));//

                            break;
                        case QueryType.SELECT:
                            if (!String.IsNullOrEmpty(sFields)) sFields += sComa;
                            //Obtenemos los campos de la base de datos y los valores para esos campos.
                            sFields += myFieldInfo[i].Name.ToString();

                            break;
                    }
                }
            }
            //Contruimos las consultas de transacciones a la base de datos.
            switch (oType)
            {
                case QueryType.DELETE:
                    sQuery = "DELETE FROM " + sTableName + " WHERE " + GetWhere();
                    break;
                case QueryType.UPDATE:
                    sQuery = "UPDATE " + sTableName + " SET " + sQuery + " WHERE " + GetWhere();
                    break;
                case QueryType.INSERT:
                    sQuery = "INSERT INTO " + sTableName + " (" + sFields + ") VALUES (" + sValor + ")";
                    break;
                case QueryType.SELECT:
                    sQuery = "SELECT " + sFields + " FROM " + sTableName + " WHERE " + GetWhere();
                    break;
            }
            //Ejecutamos la transacción a la base de datos.
            if (oType != QueryType.SELECT)
                oCon.Execute(sQuery);
            else
                dtTablaSelect = oCon.InitDataAdapter(sQuery);
        }

        /// <summary>
        ///     Obtenemos el Where para la tabla.
        /// </summary>
        /// <returns></returns>
        private String GetWhere()
        {
            String sWhere = "";
            foreach (DictionaryEntry oDic in oKeysColumns)
            {
                //if (String.IsNullOrEmpty(sWhere)) sWhere = " WHERE ";
                if (!String.IsNullOrEmpty(sWhere)) sWhere += " AND ";
                sWhere += oDic.Key.ToString() + " = " + GetData(oDic.Key.ToString(), oDic.Value.ToString());
            }
            return sWhere;
        }

        /// <summary>
        ///     Función que formatea entre comillas simples (') el valor de un campo
        ///     pasado como parámetro si el tipo de datos es varchar o datetime.
        /// </summary>
        /// <param name="sNombreCampo">
        ///     Nombre del campo cuyo contenido se va a retornar cuando la propiedad Tag del control
        ///     especifica más de un campo separado por ";"
        /// </param>
        /// <param name="sValorCampo">
        ///     Valor que contiene el campo que será formateado
        /// </param>
        /// <returns>
        ///     Devuelve o no entre comillas el valor que contiene el control dependiendo del tipo de
        ///     datos del campo de la tabla al que corresponda.
        /// </returns>
        public String GetData(String sNombreCampo, Object sValorCampo)
        {
            //Formatea el valor según el tipo de datos del campo relacionado 
            if (sValorCampo == null)
            {
                if (GetProperty(sNombreCampo, PropertyType.ISNull).ToString() == "YES")
                    //si permite valores nulos, devuelve formateado el valor que tenga
                    sValorCampo = "NULL";
                else
                    //si NO permite valores nulos, devuelve formateado el valor por defecto
                    sValorCampo = TypeData(sNombreCampo, PropertyType.DefaultData.ToString());
            }
            else
                //devuelve formateado el valor que tenga
                sValorCampo = TypeData(GetProperty(sNombreCampo, PropertyType.DataType), sValorCampo.ToString());


            return sValorCampo.ToString();
        }

        /// <summary>
        ///     Devuelve el valor del campo con su valor correspondiente.
        /// </summary>
        /// <param name="sTipoCampo"></param>
        /// <param name="sValorCampo"></param>
        /// <returns></returns>
        private String TypeData(String sTipoCampo, String sValorCampo)
        {
            String sComilla = "";
            switch (sTipoCampo)
            {
                case "int4":
                case "int8":
                case "int2":
                    if (ClsGeneral.IsNumeric(sValorCampo))
                        //si viene con decimales se los quita
                        sValorCampo = int.Parse(sValorCampo.ToString()).ToString();
                    else if (sValorCampo.ToUpper() == "NULL")
                        //no hace nada (se supone que el campo admite valores NULL)
                        break;
                    else
                        sValorCampo = "0";

                    break;

                case "varchar":
                case "text":
                    if (!String.IsNullOrEmpty(sValorCampo))
                        sComilla = "'";
                    else
                        sValorCampo = "NULL";
                    break;

                case "timestamp":
                case "date":
                case "timetz":
                    if (ClsGeneral.IsDate(sValorCampo))
                    {
                        sComilla = "'";
                        //TODO:dar formato de fecha adecuado si es necesario.
                        sValorCampo = DateTime.Parse(sValorCampo).ToString("yyyy-MM-dd hh:mm:ss");
                    }
                    else
                        sValorCampo = "NULL";

                    break;

                case "bool":
                    if (!String.IsNullOrEmpty(sValorCampo))
                        sValorCampo = "FALSE";
                    break;

                case "float8":
                case "numeric":
                    if (ClsGeneral.IsNumeric(sValorCampo))
                        //convierte la coma en punto
                        sValorCampo = sValorCampo.Replace(",", ".");
                    else if (sValorCampo.ToUpper() == "NULL")
                        //no hace nada (se supone que el campo admite valores NULL)
                        break;
                    else
                        sValorCampo = "0";

                    break;
            }
            return sComilla + sValorCampo + sComilla;
        }

        /// <summary>
        ///     Comprueba si esta definido el datos miembro BASETABLE
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private void CheckTable(Object obj)
        {
            FieldInfo[] myFieldInfo;
            Type myType = obj.GetType();
            myFieldInfo = myType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                           BindingFlags.InvokeMethod);
            for (int i = 0; i < myFieldInfo.Length; i++)
            {
                if (myFieldInfo[i].GetValue(obj) != null && myFieldInfo[i].Name.ToUpper() == NAME_TABLE)

                    if (oSchemaStruct.ContainsKey(myFieldInfo[i].GetValue(obj)))
                    {
                        //Comprobamos que existe la tabla y inicializamos sus columnas.
                        sTableName = (String) myFieldInfo[i].GetValue(obj);
                        OColumns = (Hashtable) oSchemaStruct[myFieldInfo[i].GetValue(obj).ToString()];
                        return;
                    }
                    else
                        ClsTraccer.RunException(
                            "La tabla " + myFieldInfo[i].GetValue(obj) + " no existe en el esquema de la base datos.",
                            "CheckTable");
            }
            //TODO:MIRAR ESTO Y COMPROBAR QUE RECORRE TODOS LOS CAMPOS DE LA CLASE.
            ClsTraccer.RunException(
                "Debe declarar el dato miembro DataTable y inicializarlo con el nombre de la tabla en su clase.",
                "CheckTable");
        }

        /// <summary>
        ///     Comprueba si esta definido los campos clave de la tabla, si no esta definido debe dar un error.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private void CheckKeys(Object obj)
        {
            FieldInfo[] myFieldInfo;
            Type myType = obj.GetType();
            myFieldInfo = myType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                           BindingFlags.InvokeMethod);
            var iKeys = (int) OColumns[PropertyType.CountPrimarykeys.ToString()];
            int iKeysObj = 0;
            oKeysColumns = new Hashtable();

            for (int i = 0; i < myFieldInfo.Length; i++)
            {
                if (myFieldInfo[i].Name.ToUpper() != NAME_TABLE)
                {
                    //Comprobamos el numero de campos claves.
                    if (GetProperty(myFieldInfo[i].Name, PropertyType.PrimaryKey) == "YES")
                    {
                        oKeysColumns.Add(myFieldInfo[i].Name, myFieldInfo[i].GetValue(obj));
                        //Comprobamos que los campos claves no seán nulo, si el campo esta duplicado la base de datos
                        //es la que nos va a devolver el error.
                        if (myFieldInfo[i].GetValue(obj) == null)
                            ClsTraccer.RunException(
                                "El campo  clave " + myFieldInfo[i].Name + "de la tabla " + sTableName +
                                "no puede ser nulo.", "CheckKeys");
                        iKeysObj = +1;
                    }
                }
            }
            //Debe ser iguales el número de claves definidas en la clase que las del esquema de la base de datos.
            if (iKeys == iKeysObj)
                return;
            else
            {
                ClsTraccer.RunException("Primary keys no definida en la clase " + obj.ToString(), "CheckKeys");
                return;
            }
        }

        /// <summary>
        ///     Compruba si están definidos todos los campos de la base de datos
        /// </summary>
        /// <param name="obj"></param>
        private void CheckFields(Object obj)
        {
            bool bExit = false;
            int i = 0;
            FieldInfo[] myFieldInfo;
            Type myType = obj.GetType();
            myFieldInfo = myType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                           BindingFlags.InvokeMethod);

            //Comprobamos que todos los campos de obj existen en el esquema de la tabla.
            /*for (int j = 0; j < myFieldInfo.Length; j++)
            {
                if (myFieldInfo[i].Name.ToUpper() != NAME_TABLE && !this.OColumns.ContainsKey(myFieldInfo[i].Name))
                    ClsTraccer.RunException("El campo " + myFieldInfo[j].Name + " no existe en el esquema de la tabla" + this.sTableName,"CheckFields");
             }*/

            //Recorremos la colección de columnas para comprobar que existen todos los campos.
            foreach (DictionaryEntry oDic in OColumns)
            {
                bExit = false;
                i = 0;
                while (!bExit)
                {
                    if (myFieldInfo[i].Name.ToUpper() != NAME_TABLE)
                    {
                        if (oDic.Key.ToString() == myFieldInfo[i].Name)
                            bExit = true;
                        else if (oDic.Key.ToString() == PropertyType.CountPrimarykeys.ToString())
                            bExit = true;
                        else if (i >= myFieldInfo.Length - 1)
                        {
                            ClsTraccer.RunException(
                                "El campo " + oDic.Key.ToString() + " no existe en esquema de la tabla " + sTableName,
                                "CheckFields");
                        }
                    }
                    i++;
                }
            }
            //return true;
        }

        /// <summary>
        ///     Inicializa las variables a nulo.
        /// </summary>
        private void ValueInitial()
        {
            sTableName = "";
            oColumns = null;
            oKeysColumns = null;
            dtTablaSelect = null;
            oCon = null;
            oSchemaStruct = null;
            sPrimaryKeys = null;
        }

        /// <summary>
        ///     Añade un nuevo campo a la tabla.
        /// </summary>
        /// <param name="obj">Objeto que contiene la información de la clase.</param>
        public void Save(Object obj)
        {
            //Verificación de la tabla.
            try
            {
                ExecuteQuery(obj, QueryType.INSERT);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        ///     Actuliza la tabla
        /// </summary>
        /// <param name="obj">Objeto que contiene la información de la clase.</param>
        public void Update(Object obj)
        {
            try
            {
                ExecuteQuery(obj, QueryType.UPDATE);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        ///     Borrar un registro.
        /// </summary>
        /// <param name="obj">Objeto que contiene la información de la clase.</param>
        public void Delete(Object obj)
        {
            try
            {
                ExecuteQuery(obj, QueryType.DELETE);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        ///     Ejecuta la culsuta de UPDATE,INSERT,DELETE.
        /// </summary>
        /// <param name="eTipoQuery">Indica el tipo de operación.</param>
        /// <param name="obj">Indica el tipo de operación.</param>
        private void ExecuteQuery(Object obj, QueryType eTipoQuery)
        {
            CheckTable(obj);

            CheckKeys(obj);

            CheckFields(obj);
            try
            {
                oCon.DbOpen();
                if (eTipoQuery != QueryType.SELECT)
                {
                    oCon.BeginTransaction();
                    GetQuery(eTipoQuery, obj);
                    oCon.Commit();
                }
                else
                    GetQuery(eTipoQuery, obj);
            }
            catch (Exception ex)
            {
                oCon.RollBack();
            }
            finally
            {
                oCon.DbClose();
            }
            //todo: Si la conexión da un error debe ser lanzada excepción. Esto debo mirarlo mejor.

            //this.ValueInitial();
        }


        /// <summary>
        ///     Obtiene los valores de los campos.
        /// </summary>
        /// <param name="obj">Objeto que contiene los datos del registro de la tabla.</param>
        public void GetFields(Object obj)
        {
            FieldInfo[] myFieldInfo;
            Type myType = obj.GetType();
            myFieldInfo = myType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                           BindingFlags.InvokeMethod);
            try
            {
                ExecuteQuery(obj, QueryType.SELECT);
                DataTable dtTables = dtTablaSelect.Tables[0];
                foreach (DataRow drTb in dtTables.Rows)
                {
                    for (int i = 0; i < myFieldInfo.Length; i++)
                    {
                        if (oColumns.ContainsKey(myFieldInfo[i].Name))
                            myFieldInfo[i].SetValue(obj, drTb[myFieldInfo[i].Name]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        ///     Tipos de Query para atacar la base datos.
        /// </summary>
        private enum QueryType
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

        /*/// <summary>
       /// Método de IDisposable para desechar la clase.
       /// </summary>
        public override void prueba()
       {
           ValueInitial();
           // Llamo al método que contiene la lógica
           // para liberar los recursos de esta clase.
           this. Dispose(true);

       }*/
    }
}