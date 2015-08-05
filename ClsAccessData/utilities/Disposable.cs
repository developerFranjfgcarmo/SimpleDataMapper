using System;

namespace SimpleDataMapper.utilities
{
    internal abstract class Disposable : IDisposable
    {
        private bool _disposing;

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
        /// <param name="dispose"></param>
        protected virtual void Dispose(bool dispose)
        {
            // Si no se esta destruyendo ya�
            if (_disposing) return;
            // La marco como desechada � desechandose,
            // de forma que no se puede ejecutar este c�digo
            // dos veces.
            _disposing = dispose;

            // Indico al GC que no llame al destructor
            // de esta clase al recolectarla.
            GC.SuppressFinalize(this);

            // � libero los recursos� 
        }

        /// <summary>
        ///     Destructor de clase.
        ///     En caso de que se nos olvide �desechar� la clase,
        ///     el GC llamar� al destructor, que tamb�n ejecuta la l�gica
        ///     anterior para liberar los recursos.
        /// </summary>
        ~Disposable()
        {
            // Llamo al m�todo que contiene la l�gica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
    }
}