using System;
using System.Collections;
using ObjectsPool.Containers.Interfeaces;


namespace ObjectsPool.Containers {
    /// <summary>
    /// Контейнер объектов фиксированного размера 
    /// </summary>
    internal sealed class StaticPoolContainer<T> : IPoolContainer<T> {
        private readonly BitArray _flagArray;
        private readonly T[] _container;

        internal StaticPoolContainer(int size) {
            if (size <= 0) {
                throw new ArgumentOutOfRangeException(nameof(size), $"Can't create PoolCollection of size \'{size}\'");
            }
            _container = new T[size];
            _flagArray = new BitArray(size, false);
        }

        /// <summary>
        /// Заполняет коллекцию новыми элементами
        /// </summary>
        public void Fill(Func<T> createFunction) {
            for (int i = 0; i < _flagArray.Length; i++) {
                if (_flagArray.Get(i)) {
                    continue;
                }
                
                _flagArray.Set(i, true);
                _container[i] = createFunction();
            }
        }
        
        /// <summary>
        /// Пытается получить элемент из коллекции
        /// <para>(Может вернуть default)</para>
        /// </summary>
        public T Get() {
            for (int i = 0; i < _flagArray.Length; i++) {
                if (!_flagArray.Get(i)) {
                    continue;
                }
                
                _flagArray.Set(i, false);
                var element = _container[i];
                _container[i] = default;
                return element;
            }

            return default;
        }

        /// <summary>
        /// <para>Пытается вернуть элемент в коллекцию.</para>
        /// <para>В случае, если элемент удалось закэшировать, возвращает TRUE</para>
        /// <para>Иначе - FALSE</para>
        /// </summary>
        public bool Return(T element) {
            if (_isDisposed) {
                return false;
            }
            
            for (int i = 0; i < _flagArray.Count; i++) {
                if (_flagArray.Get(i)) {
                    continue;
                }
                
                _flagArray.Set(i, true);
                _container[i] = element;
                return true;
            }

            return false;
        }
        
#region DISPOSE
        ~StaticPoolContainer() {
            Dispose(false);
        }
        
        private bool _isDisposed;

        private void Dispose(bool disposing) {
            if (_isDisposed) {
                return;
            }

            _flagArray.SetAll(false);
            for (int i = 0; i < _container.Length; i++) {
                _container[i] = default;
            }
            _flagArray.Length = 0;
            
            _isDisposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
#endregion
    }
}