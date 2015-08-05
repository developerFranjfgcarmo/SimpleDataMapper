using System;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.Controller.ClsMemberClass
{
    /// <summary>
    ///     Almacena el nombre de una propiedad de una clase.
    /// </summary>
    internal class PropertyClass : Disposable
    {
        #region Declaración de campos.

        private readonly Boolean _bCanRead;
        private readonly Boolean _bCanWrite;

        #endregion

        #region Declaración del constructor.

        /// <summary>
        ///     Inicializa el objeto ClsPropertyClass.
        /// </summary>
        /// <param name="sProperty">Nombre de la propiedad.</param>
        /// <param name="bCanRead">Marca si la propiedad se puede leer.</param>
        /// <param name="bCanWrite">Marca si la propiedad se puede sobreescribir.</param>
        internal PropertyClass(String sProperty, Boolean bCanRead, Boolean bCanWrite)
        {
            SProperty = sProperty;
            _bCanRead = bCanRead;
            _bCanWrite = bCanWrite;
        }

        #endregion

        #region Declaración de las propiedades.

        /// <summary>
        ///     Carga o devuelve el nombre de la propiedad.
        /// </summary>
        internal string SProperty { get; set; }

        /// <summary>
        ///     Indica si la propiedad se puede cargar.
        /// </summary>
        internal Boolean BCanWrite
        {
            get { return _bCanWrite; }
        }

        /// <summary>
        ///     Indica si la propiedad se puede leer.
        /// </summary>
        internal Boolean BCanRead
        {
            get { return _bCanRead; }
        }

        #endregion
    }
}