using System;

namespace ClsAccessData.utilities
{
    internal abstract class ClsDisposable : IDisposable
    {
        private bool disposing;

        /// <summary>
        ///     M�todo de IDisposable para desechar la clase.
        /// </summary>
        public void Dispose()
        {
            // Llamo al m�todo que contiene la l�gica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        /// <summary>
        ///     M�todo sobrecargado de Dispose que ser� el que
        ///     libera los recursos, controla que solo se ejecute
        ///     dicha l�gica una vez y evita que el GC tenga que
        ///     llamar al destructor de clase.
        /// </summary>
        /// <param name=� bool�></param>
        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya�
            if (!disposing)
            {
                // La marco como desechada � desechandose,
                // de forma que no se puede ejecutar este c�digo
                // dos veces.
                disposing = true;

                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);

                // � libero los recursos� 
            }
        }

        /// <summary>
        ///     Destructor de clase.
        ///     En caso de que se nos olvide �desechar� la clase,
        ///     el GC llamar� al destructor, que tamb�n ejecuta la l�gica
        ///     anterior para liberar los recursos.
        /// </summary>
        ~ClsDisposable()
        {
            // Llamo al m�todo que contiene la l�gica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        internal enum Estado
        {
            Delete,
            Update,
            Insert
        };
    }
}