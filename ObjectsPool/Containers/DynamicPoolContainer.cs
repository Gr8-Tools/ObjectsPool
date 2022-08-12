using System;
using System.Collections.Generic;
using ObjectsPool.Containers.Interfeaces;


namespace ObjectsPool.Containers {
    /// <summary>
    /// Контейнер объектов изменяемого (в пределах) размера 
    /// </summary>
    internal sealed class DynamicPoolContainer<T> : IPoolContainer<T>{
        private readonly Queue<T> _container;

        private readonly int _defaultSize;
        private readonly int _maxSize;
        
        internal DynamicPoolContainer(int maxSize, int defaultSize = 0) {
            if (maxSize <= 0) {
                throw new ArgumentOutOfRangeException(nameof(maxSize), $"Can't create PoolCollection of size \'{maxSize}\'");
            }

            _defaultSize = defaultSize != 0 ? defaultSize : maxSize;
            _maxSize = maxSize;
            _container = new Queue<T>(maxSize);
        }

        /// <summary>
        /// Заполняет коллекцию новыми элементами
        /// </summary>
        public void Fill(Func<T> createFunction) {
            while (_container.Count < _defaultSize) {
                _container.Enqueue(createFunction());
            }
        }
        
        /// <summary>
        /// Пытается получить элемент из коллекции
        /// <para>(Может вернуть default)</para>
        /// </summary>
        public bool TryGet(out T element) {
            if (_container.Count == 0) {
                element = default;
                return false;
            }

            element = _container.Dequeue();
            return true;
        }

        /// <summary>
        /// <para>Пытается вернуть элемент в коллекцию.</para>
        /// <para>В случае, если элемент удалось закэшировать, возвращает TRUE</para>
        /// <para>Иначе - FALSE</para>
        /// </summary>
        public bool TryReturn(T element) {
            if (_container.Count >= _maxSize || _isDisposed) {
                return false;
            }
            
            _container.Enqueue(element);
            return true;
        }
        
#region DISPOSE
        ~DynamicPoolContainer() {
            DisposeInternal(false);
        }
        
        private bool _isDisposed;

        private void DisposeInternal(bool disposing) {
            if (_isDisposed) {
                return;
            }
            
            _container.Clear();
            _isDisposed = true;
        }

        public void Dispose() {
            DisposeInternal(true);
            GC.SuppressFinalize(this);
        }
#endregion
    }
}