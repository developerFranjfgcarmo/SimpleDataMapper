using System;
using System.Collections.Generic;
using System.Data;
using ClsAccessData.Connection;
using ClsAccessData.utilities;

namespace ClsAccessData.Schema
{
    //Todo:25/01/2012. Obtener el esquema y que se quede cargo en memoria
    internal class ClsSchema : ClsDisposable
    {
        #region Declaración de campos

        /// <summary>
        ///     Objeto que almacena la conexión de la base de datos.
        /// </summary>
        private readonly ClsConnection oConection;

        /// <summary>
        ///     Colección de listas de tablas.
        /// </summary>
        private List<ClsTable> oTables;

        /// <summary>
        ///     Nombre de la tabla activa
        /// </summary>
        private String sNameTable;

        #endregion

        #region Declaración de contructores.

        /// <summary>
        ///     Inicializa el esquema de la base de datos.
        /// </summary>
        /// <param name="oConection">Objeto que contiene la conexión a la base de datos.</param>
        internal ClsSchema(ClsConnection oConection) : this("", oConection)
        {
        }

        /// <summary>
        ///     Inicializa sólo una tabla de la base de datos.
        /// </summary>
        /// <param name="sNameTable">Nombre de la tabla a inicializar.</param>
        /// <param name="oConection">Objeto que contiene la conexión a la base de datos.</param>
        internal ClsSchema(String sNameTable, ClsConnection oConection)
        {
            this.oConection = oConection;
            if (this.oConection.Status() == ConnectionState.Closed)
            {
                this.oConection.DbOpen();
            }
            this.sNameTable = sNameTable;
            //Cargamos el esquema o la tabla.
            if (!String.IsNullOrEmpty(this.sNameTable))
            {
                oTables = new List<ClsTable>();
                oTables.Add(LoadTable(sNameTable));
            }
            else
                LoadSchema();
            this.oConection.DbClose();
        }

        #endregion

        #region Métodos públicos.

        /// <summary>
        ///     Obtiene el objeto tabla (Columnas, primary key, valores por defecto, etc)
        /// </summary>
        /// <param name="sNameTable">Nombre de la tabla.</param>
        /// <returns>Devuelve un Objeto ClsTable</returns>
        internal ClsTable GetTable(String sNameTable)
        {
            return oTables.Find(delegate(ClsTable oFindTable) { return oFindTable.SNameTable == this.sNameTable; }
                );
        }

        #endregion

        #region Métodos privados.

        /// <summary>
        ///     Inicializa los campos PrimaryKey de la tabla.
        /// </summary>
        /// <param name="oTable">Objeto ClsTable que debe contener toda la información de la tabla.</param>
        private void LoadPrimaryKey(ClsTable oTable)
        {
            try
            {
                //Query que obtiene los campos keys de la tabla especificada
                String sSql = "SELECT  c.attname " +
                              "FROM pg_index a, pg_class b, pg_attribute c, pg_indexes d, pg_constraint e " +
                              "WHERE d.indexname = e.conname " +
                              "AND  a.indrelid = b.oid " +
                              "AND  c.attrelid = b.oid " +
                              "AND  c.attnum = any(a.indkey) " +
                              "AND  e.contype = 'p' " +
                              "AND  a.indrelid = e.conrelid " +
                              "AND d.tablename    = '" + oTable.SNameTable + "'  " +
                              "AND indisprimary";

                String sCampo;

                DataSet dtPrimaryKeys = oConection.InitDataAdapter(sSql);
                DataTable dtTables = dtPrimaryKeys.Tables[0];
                //Almacenamos las primaryKey de las tablas.
                foreach (DataRow drKeys in dtTables.Rows)
                {
                    sCampo = drKeys["attname"].ToString().Trim();
                    //Busca la columna y le indica que es primaryKey del objeto ClsTable.
                    ClsColumn oColumn =
                        oTable.ColColums.Find(delegate(ClsColumn oCol) { return oCol.NameColumn == sCampo; }
                            );
                    if (oColumn != null)
                    {
                        oColumn.PrimaryKey = true;
                        oTable.ColPrimaryKey.Add(oColumn.NameColumn);
                    }
                    else
                    {
                        ClsTraccer.RunException(
                            "Error al obtener las primary keys de la tabla " + oTable.SNameTable + " - " +
                            drKeys["attname"].ToString().Trim(), "GetPrimaryKey");
                    }
                }
            }
            catch (Exception ex)
            {
                ClsTraccer.RunException(ex, "Error al obtener las primary keys de la tabla " + oTable.SNameTable,
                    "GetPrimaryKey");
            }
        }

        /// <summary>
        ///     Carga el esquema de la base de datos.
        /// </summary>
        //todo:Por aquí. Este sólo debe cargar la estructura de la tabla. Para cargar los demás esquemas, se debería hacer desde otra clase que cargaría todas las tablas.
        private void LoadSchema()
        {
            String[] restriction;

            oTables = new List<ClsTable>();
            try
            {
                DataTable dtShema;
                //Para ver información de las diferentes colecciones de datos devueltas por el método GetSchema:
                // http://msdn2.microsoft.com/es-es/library/ms254969(VS.80).aspx
                // http://msdn2.microsoft.com/es-es/library/ms254501(VS.80).aspx

                //Obtenemos todas las tablas de base datos.             
                restriction = new[] {null, "public", null, "BASE TABLE"};

                //dtShema = this.oConection.DBConnection.GetSchema("Tables", restriction);
                dtShema = oConection.DbConnection.GetSchema("Tables", new[] {null, "public", null, "BASE TABLE"});
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
                                sNameTable = rowSchema[columsSchema].ToString();
                                oTables.Add(LoadTable(sNameTable));
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClsTraccer.RunException(ex, "Error al inicializar el esquema de la tabla " + sNameTable,
                    "LoadSchema overloads1");
            }
        }

        /// <summary>
        ///     Carga la estructura de una tabla(Columnas).
        /// </summary>
        /// <param name="sNombreTable">Nombre de la tabla.</param>
        /// <returns>Devuelve un objeto del tipo ClsTable.</returns>
        private ClsTable LoadTable(String sNombreTable)
        {
            var oTable = new ClsTable(sNameTable, new List<string>(), new List<ClsColumn>());
            DataTable dtTable;
            try
            {
                dtTable = oConection.DbConnection.GetSchema("Columns", new[] {null, null, sNombreTable, null});
                //Recorremos todos los registros, para obtener las propiedades de cada una de las columnas de la tabla.
                foreach (DataRow row in dtTable.Rows)
                {
                    using (var oColumn = new ClsColumn())
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
                        oTable.ColColums.Add(oColumn);
                    }
                }
                LoadPrimaryKey(oTable);
            }
            catch (Exception ex)
            {
                ClsTraccer.RunException(ex, "Error al inicializar el esquema de la tabla " + sNameTable,
                    "LoadTable overloads1");
            }
            return oTable;
        }

        #endregion
    }
}