using System;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.Controller.ClsMemberClass
{
    /// <summary>
    ///     Esta clase almacena los campos y las propiedades de la clase.
    /// </summary>
    internal class ClsFieldClass : Disposable
    {
        #region Declaración de campos.

        #endregion

        #region Declaración de constructor.

        /// <summary>
        ///     Constructor de la clase.
        /// </summary>
        /// <param name="sField">Campo de la clase</param>
        /// <param name="sProperty">Objeto ClsPorpertyClass que almacenará la propiedad del campo.</param>
        internal ClsFieldClass(String sField, ClsPropertyClass sProperty)
        {
            SField = sField;
            Property = sProperty;
        }

        #endregion

        #region Declaración de propiedades.

        /// <summary>
        ///     Carga o devuelve el valor del campo.
        /// </summary>
        internal string SField { get; set; }

        /// <summary>
        ///     Carga o devuelve la propiedad del campo.
        /// </summary>
        internal ClsPropertyClass Property { get; set; }

        #endregion
    }
}