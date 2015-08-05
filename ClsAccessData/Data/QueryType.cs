using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataMapper.Data
{
    /// <summary>
    /// 
    /// </summary>
    public enum QueryType
    {
        /// <summary>
        ///     Transacción de actualización.
        /// </summary>
        Update,

        /// <summary>
        ///     Transacción de inserción.
        /// </summary>
        Insert,

        /// <summary>
        ///     Transacción de borrado.
        /// </summary>
        Delete,

        /// <summary>
        ///     Transacción de Obtención de registro.
        /// </summary>
        Select
    }
}
