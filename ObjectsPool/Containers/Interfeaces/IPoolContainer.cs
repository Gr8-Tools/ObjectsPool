using System;


namespace ObjectsPool.Containers.Interfeaces {
    /// <summary>
    /// Интерфейс контейнера объектов 
    /// </summary>
    internal interface IPoolContainer<T> : IDisposable {
        /// <summary>
        /// Заполняет коллекцию новыми элементами
        /// </summary>
        void Fill(Func<T> createFunction);
        
        /// <summary>
        /// Пытается получить элемент из коллекции
        /// <para>(Может вернуть default)</para>
        /// </summary>
        bool TryGet(out T element);
        
        /// <summary>
        /// <para>Пытается вернуть элемент в коллекцию.</para>
        /// <para>В случае, если элемент удалось закэшировать, возвращает TRUE</para>
        /// <para>Иначе - FALSE</para>
        /// </summary>
        bool TryReturn(T element);
    }
}