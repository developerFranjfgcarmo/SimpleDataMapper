using System;
using ClsAccessData.utilities;

namespace ClsAccessData.Schema
{
    /// <summary>
    ///     Almacena las información de cada columna de la tabla.
    /// </summary>
    internal class ClsColumn : ClsDisposable
    {
        #region "Declaración de campos de las clase."

        private String sDefaultData;

        /// <summary>
        ///     Tipos de propiedades de los campos.
        /// </summary>
        internal enum PropertyType
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

        #endregion

        #region "Declaración de propiedades."

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
            get { return sDefaultData; }
            set
            {
                if (value == "")
                    value = "NULL";
                sDefaultData = value;
            }
        }

        /// <summary>
        ///     Longuitud del Campo.
        /// </summary>
        internal int FieldLenght { get; set; }

        #endregion
    }
}