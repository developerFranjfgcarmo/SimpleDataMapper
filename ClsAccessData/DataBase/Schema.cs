using System;
using System.Collections.Generic;
using System.Data;
using SimpleDataMapper.Connector;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.DataBase
{
    //Todo:25/01/2012. Obtener el esquema y que se quede cargo en memoria
    //Todo:persist in cache the shema.
    internal class Schema : Disposable
    {
        #region [private properties]

        /// <summary>
        ///     Objeto que almacena la conexión de la base de datos.
        /// </summary>
        private readonly Connection _conection;

        /// <summary>
        ///     Nombre de la tabla activa
        /// </summary>
        private String _nameTable;

        /// <summary>
        ///     Colección de listas de tablas.
        /// </summary>
        private List<Table> _tables;

        #endregion

        #region [Constructors]

        /// <summary>
        ///     Inicializa el esquema de la base de datos.
        /// </summary>
        /// <param name="connection">Objeto que contiene la conexión a la base de datos.</param>
        internal Schema(Connection connection) : this("", connection)
        {
        }

        /// <summary>
        ///     Inicializa sólo una tabla de la base de datos.
        /// </summary>
        /// <param name="sNameTable">Nombre de la tabla a inicializar.</param>
        /// <param name="oConection">Objeto que contiene la conexión a la base de datos.</param>
        internal Schema(String sNameTable, Connection oConection)
        {
            _conection = oConection;
            if (_conection.Status() == ConnectionState.Closed)
            {
                _conection.DbOpen();
            }
            _nameTable = sNameTable;
            //Cargamos el esquema o la tabla.
            if (!string.IsNullOrWhiteSpace(_nameTable))
            {
                _tables = new List<Table> {LoadTable(sNameTable)};
            }
            else
            {
              LoadSchema();  
            }
                
            _conection.DbClose();
        }

        #endregion

        #region [Public Methods]

        /// <summary>
        ///     Obtiene el objeto tabla (Columnas, primary key, valores por defecto, etc)
        /// </summary>
        /// <param name="nameTable">Nombre de la tabla.</param>
        /// <returns>Devuelve un Objeto ClsTable</returns>
        internal Table GetTable(String nameTable)
        {
            return _tables.Find(oFindTable => oFindTable.NameTable == _nameTable);
        }

        #endregion

        #region [Private Methods].

        /// <summary>
        ///     Inicializa los campos PrimaryKey de la tabla.
        /// </summary>
        /// <param name="table">Objeto ClsTable que debe contener toda la información de la tabla.</param>
        private void LoadPrimaryKey(Table table)
        {
            try
            {
                //Query que obtiene los campos keys de la tabla especificada
                string sSql = string.Format(@"SELECT  c.attname 
                              FROM pg_index a, pg_class b, pg_attribute c, pg_indexes d, pg_constraint e 
                              WHERE d.indexname = e.conname 
                                AND  a.indrelid = b.oid 
                                AND c.attrelid = b.oid 
                                AND  c.attnum = any(a.indkey) 
                                AND  e.contype = 'p' 
                                AND  a.indrelid = e.conrelid 
                                AND d.tablename    = '{0}' 
                                AND indisprimary", table.NameTable);

                var dtPrimaryKeys = _conection.InitDataAdapter(sSql);
                var dtTables = dtPrimaryKeys.Tables[0];
                //Almacenamos las primaryKey de las tablas.
                foreach (DataRow drKeys in dtTables.Rows)
                {
                    var campo = drKeys["attname"].ToString().Trim();
                    //Busca la columna y le indica que es primaryKey del objeto ClsTable.
                    var oColumn = table.ColColums.Find(oCol => oCol.NameColumn == campo);
                    if (oColumn != null)
                    {
                        oColumn.PrimaryKey = true;
                        table.ColPrimaryKey.Add(oColumn.NameColumn);
                    }
                    else
                    {
                        Traccer.RunException(
                            "Error al obtener las primary keys de la tabla " + table.NameTable + " - " +
                            drKeys["attname"].ToString().Trim(), "GetPrimaryKey");
                    }
                }
            }
            catch (Exception ex)
            {
                Traccer.RunException(ex, "Error al obtener las primary keys de la tabla " + table.NameTable,
                    "GetPrimaryKey");
            }
        }

        /// <summary>
        ///     Carga el esquema de la base de datos.
        /// </summary>
        //todo:Por aquí. Este sólo debe cargar la estructura de la tabla. Para cargar los demás esquemas, se debería hacer desde otra clase que cargaría todas las tablas.
        private void LoadSchema()
        {
            _tables = new List<Table>();
            try
            {
                //Para ver información de las diferentes colecciones de datos devueltas por el método GetSchema:
                // http://msdn2.microsoft.com/es-es/library/ms254969(VS.80).aspx
                // http://msdn2.microsoft.com/es-es/library/ms254501(VS.80).aspx

                //Obtenemos todas las tablas de base datos.             

                //dtShema = this.oConection.DBConnection.GetSchema("Tables", restriction);
                var dtShema = _conection.DbConnection.GetSchema("Tables",
                    new[] {null, "public", null, "BASE TABLE"});
                foreach (DataRow rowSchema in dtShema.Rows)
                {
                    foreach (DataColumn columsSchema in dtShema.Columns)
                    {
                        // Console.WriteLine(colSchema.ColumnName + ": " + rowSchema[colSchema].ToString());

                        //Comprobamos si ColumnName es igual table_name, es el nombre de tabla.         
                        switch (columsSchema.ColumnName)
                        {
                            case "table_name":
                                //Almacenamos el nombre de la tabla en una variable para poder almacenarlo en el hashtable.
                                _nameTable = rowSchema[columsSchema].ToString();
                                _tables.Add(LoadTable(_nameTable));
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Traccer.RunException(ex, "Error al inicializar el esquema de la tabla " + _nameTable,
                    "LoadSchema overloads1");
            }
        }

        /// <summary>
        ///     Carga la estructura de una tabla(Columnas).
        /// </summary>
        /// <param name="nameTable">Nombre de la tabla.</param>
        /// <returns>Devuelve un objeto del tipo ClsTable.</returns>
        private Table LoadTable(String nameTable)
        {
            var table = new Table(_nameTable, new List<string>(), new List<Column>());
            try
            {
                var dtTable = _conection.DbConnection.GetSchema("Columns", new string[] { null, null, nameTable, null });
                //Recorremos todos los registros, para obtener las propiedades de cada una de las columnas de la tabla.
                foreach (DataRow row in dtTable.Rows)
                {
                    //row["IS_NULLABLE"]
                    /*
                     * SELECT *
FROM   information_schema.columns
WHERE  table_name  = 'prueba'
                     */
                    using (var oColumn = new Column())
                    {
                        //recorre cada propiedad del campo 
                        foreach (DataColumn col in dtTable.Columns)
                        {
                            switch (col.ColumnName.ToUpper())
                            {
                                case "DATA_TYPE": //Tipo de datos
                                    oColumn.DataType = row[col].ToString();
                                    break;

                                case "CHARACTER_MAXIMUM_LENGTH": //Longitud del campo
                                    oColumn.FieldLenght = (row[col].ToString() == "")
                                        ? 0
                                        : int.Parse(row[col].ToString());
                                    break;

                                case "IS_NULLABLE": //Permite valores nulos
                                    oColumn.IsNull = (row[col].ToString() == "YES");
                                    break;

                                case "COLUMN_DEFAULT": //Valor por defecto
                                    oColumn.DefaultData = (row[col].ToString() == "") ? "Null" : row[col].ToString();
                                    break;
                                case "COLUMN_NAME": //Nombre del campo
                                    oColumn.NameColumn = row[col].ToString();
                                    break;
                            }
                        }
                        table.ColColums.Add(oColumn);
                    }
                }
                LoadPrimaryKey(table);
            }
            catch (Exception ex)
            {
                Traccer.RunException(ex, "Error al inicializar el esquema de la tabla " + _nameTable,
                    "LoadTable overloads1");
            }
            return table;
        }

        #endregion
    }
}