using System;

namespace SimpleDataMapper.utilities
{
    internal abstract class Disposable : IDisposable
    {
        private bool _disposing;

        /// <summary>
        ///     Método de IDisposable para desechar la clase.
        /// </summary>
        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        /// <summary>
        ///     Método sobrecargado de Dispose que será el que
        ///     libera los recursos, controla que solo se ejecute
        ///     dicha lógica una vez y evita que el GC tenga que
        ///     llamar al destructor de clase.
        /// </summary>
        /// <param name="dispose"></param>
        protected virtual void Dispose(bool dispose)
        {
            // Si no se esta destruyendo ya…
            if (_disposing) return;
            // La marco como desechada ó desechandose,
            // de forma que no se puede ejecutar este código
            // dos veces.
            _disposing = dispose;

            // Indico al GC que no llame al destructor
            // de esta clase al recolectarla.
            GC.SuppressFinalize(this);

            // … libero los recursos… 
        }

        /// <summary>
        ///     Destructor de clase.
        ///     En caso de que se nos olvide “desechar” la clase,
        ///     el GC llamará al destructor, que tambén ejecuta la lógica
        ///     anterior para liberar los recursos.
        /// </summary>
        ~Disposable()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
    }
}