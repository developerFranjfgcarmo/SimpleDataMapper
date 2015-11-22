using System;
using System.Collections.Generic;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.DataBase
{
    /// <summary>
    ///     Almacena las información de cada columna de la tabla.
    /// </summary>
    internal class Column : Disposable
    {
        #region [private properties]     
        private String _defaultData;

        #endregion

        #region [Properties]

        internal List<string> ColumnProperties {
            get
            {
                return new List<string>() { "DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH", "IS_NULLABLE", "COLUMN_DEFAULT", "COLUMN_NAME", "IS_IDENTITY" };
            }
        }

        /// <summary>
        ///     Nombre de la columna.
        /// </summary>
        internal string IsIdentity { get; set; }
        /// <summary>
        ///     Nombre de la columna.
        /// </summary>
        internal string NameColumn { get; set; }

        /// <summary>
        ///     Es nulo el campo.
        /// </summary>
        internal bool IsNull { get; set; }

        /// <summary>
        ///     Es primary key el campo.
        /// </summary>
        internal bool PrimaryKey { get; set; }

        /// <summary>
        ///     Tipo de dato.
        /// </summary>
        internal string DataType { get; set; }

        /// <summary>
        ///     Dato por defecto.
        /// </summary>
        internal String DefaultData
        {
            get { return _defaultData; }
            set
            {
                if (value == "")
                    value = "NULL";
                _defaultData = value;
            }
        }

        /// <summary>
        ///     Longuitud del Campo.
        /// </summary>
        internal int FieldLenght { get; set; }

        #endregion
    }
}