using System;
using ObjectsPool.Containers.Interfeaces;
using ObjectsPool.Systems.Interfaces;
using ObjectsPool.Utils;


namespace ObjectsPool.Systems {
    /// <summary>
    /// Базовый класс управления системой пула-компонентов
    /// </summary>
    internal abstract class BasePoolSystem<T>: IPoolSystem<T> where T : class{
        private protected abstract IPoolContainer<T> Container { get; }
        
        private readonly Func<IPoolSystem<T>, T> _createFunction;
        private readonly Action<T> _getAction;
        private readonly Action<T> _releaseAction;
        private readonly Action<T> _destroyAction;
        
        private protected BasePoolSystem(PoolSystemActionArgs<T> actionArgs) {
            _createFunction = poolSystem => {
                var instance = actionArgs.CreateFunction?.Invoke(poolSystem);
                actionArgs.CreateCallback?.Invoke(instance);
                return instance;
            };
            _getAction = actionArgs.GetAction;
            _releaseAction = actionArgs.ReleaseAction;
            _destroyAction = actionArgs.DestroyAction;
        }

        /// <summary>
        /// Заполняет контейнер элементами, вызывая при необходимости функцию <param name="CreateFunction"> Функция создания экземпляра пула</param>
        /// </summary>
        public void FillContainer() {
            Container.Fill(() => _createFunction(this));
        }
        
        /// <summary>
        /// Пытается получить элемент из коллекции
        /// <para>Если не удалось, то генерирует новый из функции <param name="CreateFunction"> Функция создания экземпляра пула</param></para>
        /// </summary>
        public T Get() {
            var element = Container.Get() ??
                          _createFunction?.Invoke(this) ??
                          throw new Exception("Can't create object as there is nothing in Pool and constructor-method is null");

            _getAction?.Invoke(element);
            return element;
        }

        /// <summary>
        /// <para>Пытается вернуть элемент в коллекцию.</para>
        /// <para>Применяет функцию <param name="ReleaseAction">Функция возврата в пул</param>, если она была передана </para>
        /// <para>Если коллекция забита, то пытается приметь функцию разрушения <param name="DestroyAction">Функция удаления объекта</param></para>
        /// </summary>
        public void Return(T element) {
            _releaseAction?.Invoke(element);
            if (!Container.Return(element) || _isDisposed) {
                _destroyAction?.Invoke(element);
            }
        }
        
#region DISPOSE        
        private bool _isDisposed;

        private void Dispose(bool disposing) {
            if (_isDisposed) {
                return;
            }

            T element;
            while ((element = Container.Get()) != default) {
                _destroyAction?.Invoke(element);
            }
            Container.Dispose();
            
            _isDisposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
#endregion
    }
}