using System;
using ObjectsPool.Systems.Interfaces;


namespace ObjectsPool.Utils {
    internal sealed class PoolSystemActionArgs<T> {
        internal Func<IPoolSystem<T>, T> CreateFunction { get; set; }
        internal Action<T>? CreateCallback { get; set; }
        internal Action<T>? GetAction { get; set; }
        internal Action<T>? ReleaseAction { get; set; }
        internal Action<T>? DestroyAction { get; set; }
    }
}