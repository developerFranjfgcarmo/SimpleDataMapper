using System;
using System.Collections.Generic;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.DataBase
{
    internal class Table : Disposable
    {
        #region [Constructors]

        /// <summary>
        ///     Inicialización del objeto sin pasarle parámetros
        /// </summary>
        internal Table() : this(null, null, null)
        {
        }

        /// <summary>
        ///     Inicializa del objeto pasándole parámetros.
        /// </summary>
        /// <param name="nameTable">Nombre de la tabla.</param>
        /// <param name="columnPrimaryKey">Colección de Columnas que son Primary.</param>
        /// <param name="listColums">Colección de objeto ClsColumn.</param>
        internal Table(String nameTable, List<String> columnPrimaryKey, List<Column> listColums)
        {
            NameTable = nameTable;
            ColPrimaryKey = columnPrimaryKey;
            ColColums = listColums;
        }

        #endregion

        #region [Properties]

        internal string NameTable { get; set; }

        /// <summary>
        ///     Lista de Columnas de la tabla.
        /// </summary>
        internal List<Column> ColColums { get; set; }

        /// <summary>
        ///     Lista de campos de la PrimaryKey.
        /// </summary>
        internal List<String> ColPrimaryKey { get; private set; }

        #endregion

        #region [Public Methods]

        /// <summary>
        ///     Comprueba si el campo es Primary key.
        /// </summary>
        /// <param name="nameColumn">Nombre de la Columna.</param>
        /// <returns>Devuelve un True si el campo es Primary Key y false en caso contrario.</returns>
        internal Boolean IsPrimaryKeys(String nameColumn)
        {
            var oColumn = ColPrimaryKey.Find(oFindCol => oFindCol.Equals(nameColumn, StringComparison.CurrentCultureIgnoreCase));
            return !String.IsNullOrEmpty(oColumn);
        }

        /// <summary>
        ///     Obtiene el objeto Columna de la tabla.
        /// </summary>
        /// <param name="nameColumn">Nombre de la Columna.</param>
        /// <returns>Devuelve un objeto ClsColumn</returns>
        internal Column GetColumn(String nameColumn)
        {
            var oColumn = ColColums.Find(oFindCol => oFindCol.NameColumn.Equals(nameColumn, StringComparison.CurrentCultureIgnoreCase));
            return oColumn ?? null;
        }


        internal Boolean ThereIsColumn(String nameColumn)
        {
            var oColumn = ColColums.Find(oFindCol => oFindCol.NameColumn.Equals(nameColumn, StringComparison.CurrentCultureIgnoreCase));
            return oColumn != null;
        }

        /// <summary>
        ///     Devuelve el número de primary key que tiene la tabla
        /// </summary>
        /// <returns>Devuelve un entero con el número de primary key.</returns>
        internal int CountPk()
        {
            return ColPrimaryKey.Count;
        }

        #endregion
    }
}