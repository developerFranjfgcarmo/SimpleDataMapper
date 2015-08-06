using System;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.Controller.ClsMemberClass
{
    /// <summary>
    ///     Almacena el nombre de una propiedad de una clase.
    /// </summary>
    internal class PropertyClass : Disposable
    {
       #region [Constructor].

        /// <summary>
        ///     Inicializa el objeto ClsPropertyClass.
        /// </summary>
        /// <param name="sProperty">Nombre de la propiedad.</param>
        /// <param name="bCanRead">Marca si la propiedad se puede leer.</param>
        /// <param name="bCanWrite">Marca si la propiedad se puede sobreescribir.</param>
        internal PropertyClass(String sProperty, Boolean bCanRead, Boolean bCanWrite)
        {
            Property = sProperty;
            CanRead = bCanRead;
            CanWrite = bCanWrite;
        }

        #endregion

        #region [Properties]

        /// <summary>
        ///     Carga o devuelve el nombre de la propiedad.
        /// </summary>
        internal string Property { get; set; }

        /// <summary>
        ///     Indica si la propiedad se puede cargar.
        /// </summary>
        internal Boolean CanWrite{ get; private set; }      

        /// <summary>
        ///     Indica si la propiedad se puede leer.
        /// </summary>
        internal Boolean CanRead { get; private set; }

        #endregion
    }
}