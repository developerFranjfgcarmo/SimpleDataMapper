using System;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.Controller.ClsMemberClass
{
    /// <summary>
    ///     Esta clase almacena los campos y las propiedades de la clase.
    /// </summary>
    internal class FieldClass : Disposable
    {
        #region Declaración de constructor.

        /// <summary>
        ///     Constructor de la clase.
        /// </summary>
        /// <param name="sField">Campo de la clase</param>
        /// <param name="sProperty">Objeto ClsPorpertyClass que almacenará la propiedad del campo.</param>
        internal FieldClass(string sField, PropertyClass sProperty)
        {
            Field = sField;
            Property = sProperty;
        }

        #endregion

        #region Declaración de propiedades.

        /// <summary>
        ///     Carga o devuelve el valor del campo.
        /// </summary>
        internal string Field { get; set; }

        /// <summary>
        ///     Carga o devuelve la propiedad del campo.
        /// </summary>
        internal PropertyClass Property { get; set; }

        #endregion
    }
}