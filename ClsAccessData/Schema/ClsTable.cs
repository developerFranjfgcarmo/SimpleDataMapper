using System;
using System.Collections.Generic;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.Schema
{
    internal class ClsTable : ClsDisposable
    {
        #region Campos

        private readonly List<String> colPrimaryKey;

        #endregion

        #region

        /// <summary>
        ///     Inicialización del objeto sin pasarle parámetros
        /// </summary>
        internal ClsTable() : this(null, null, null)
        {
        }

        /// <summary>
        ///     Inicializa del objeto pasándole parámetros.
        /// </summary>
        /// <param name="sNameTable">Nombre de la tabla.</param>
        /// <param name="colPrimaryKey">Colección de Columnas que son Primary.</param>
        /// <param name="colColums">Colección de objeto ClsColumn.</param>
        internal ClsTable(String sNameTable, List<String> colPrimaryKey, List<ClsColumn> colColums)
        {
            this.SNameTable = sNameTable;
            this.colPrimaryKey = colPrimaryKey;
            this.ColColums = colColums;
        }

        #endregion

        #region Propiedades

        internal string SNameTable { get; set; }

        /// <summary>
        ///     Lista de Columnas de la tabla.
        /// </summary>
        internal List<ClsColumn> ColColums { get; set; }

        /// <summary>
        ///     Lista de campos de la PrimaryKey.
        /// </summary>
        internal List<String> ColPrimaryKey
        {
            get { return colPrimaryKey; }
        }

        #endregion

        #region Métodos públicos.

        /// <summary>
        ///     Comprueba si el campo es Primary key.
        /// </summary>
        /// <param name="sNameColumn">Nombre de la Columna.</param>
        /// <returns>Devuelve un True si el campo es Primary Key y false en caso contrario.</returns>
        internal Boolean IsPrimaryKeys(String sNameColumn)
        {
            if (this != null)
            {
                String oColumn = ColPrimaryKey.Find(delegate(String oFindCol) { return oFindCol == sNameColumn; }
                    );
                if (!String.IsNullOrEmpty(oColumn))
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Obtiene el objeto Columna de la tabla.
        /// </summary>
        /// <param name="sNameColumn">Nombre de la Columna.</param>
        /// <returns>Devuelve un objeto ClsColumn</returns>
        internal ClsColumn GetColumn(String sNameColumn)
        {
            if (this != null)
            {
                ClsColumn oColumn =
                    ColColums.Find(delegate(ClsColumn oFindCol) { return oFindCol.NameColumn == sNameColumn; }
                        );
                if (oColumn != null)
                    return oColumn;
            }
            return null;
        }


        internal Boolean ThereIsColumn(String sNameColumn)
        {
            if (this != null)
            {
                ClsColumn oColumn =
                    ColColums.Find(delegate(ClsColumn oFindCol) { return oFindCol.NameColumn == sNameColumn; }
                        );
                if (oColumn != null)
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Devuelve el número de primary key que tiene la tabla
        /// </summary>
        /// <returns>Devuelve un entero con el número de primary key.</returns>
        internal int CountPk()
        {
            return colPrimaryKey.Count;
        }

        #endregion
    }
}