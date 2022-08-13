using ObjectsPool.Containers;
using ObjectsPool.Containers.Interfeaces;
using ObjectsPool.Utils;


namespace ObjectsPool.Systems {
    /// <summary>
    /// Класс управления системой пула-компонентов, в котором пулл ограничен максимальным значением
    /// </summary>
    internal sealed class DynamicPoolSystem<T> : BasePoolSystem<T> where T : class {
        private protected override IPoolContainer<T> Container { get; }

        internal DynamicPoolSystem(int maxSize, PoolSystemActionArgs<T> actionArgs) : base(actionArgs) {
            Container = new DynamicPoolContainer<T>(maxSize);
        }
        
        internal DynamicPoolSystem(int maxSize, int defaultSize, PoolSystemActionArgs<T> actionArgs) : base(actionArgs) {
            Container = new DynamicPoolContainer<T>(maxSize, defaultSize);
        }
    }
}