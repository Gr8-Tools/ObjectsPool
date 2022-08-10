using System;


namespace ObjectsPool.Systems.Interfaces {
    /// <summary>
    /// Интерфейс управления системой пула-компонентов
    /// </summary>
    internal interface IPoolSystem<T> : IDisposable {
        /// <summary>
        /// Заполняет контейнер элементами, вызывая при необходимости функцию <param name="CreateFunction"> Функция создания экземпляра пула</param>
        /// </summary>
        void FillContainer();
        
        /// <summary>
        /// Пытается получить элемент из коллекции
        /// <para>Если не удалось, то генерирует новый из функции <param name="CreateFunction"> Функция создания экземпляра пула</param></para>
        /// </summary>
        T Get();
        
        /// <summary>
        /// <para>Пытается вернуть элемент в коллекцию.</para>
        /// <para>Применяет функцию <param name="ReleaseAction">Функция возврата в пул</param>, если она была передана </para>
        /// <para>Если коллекция забита, то пытается приметь функцию разрушения <param name="DestroyAction">Функция удаления объекта</param></para>
        /// </summary>
        void Return(T element);
    }
}