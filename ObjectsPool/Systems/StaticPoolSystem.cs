using ObjectsPool.Containers;
using ObjectsPool.Containers.Interfeaces;
using ObjectsPool.Utils;


namespace ObjectsPool.Systems {
    /// <summary>
    /// Класс управления системой пула-компонентов, в котором пулл жестко ограничен (выделена память только на определенное количество элементов)
    /// </summary>
    internal sealed class StaticPoolSystem<T> : BasePoolSystem<T> where T : class {
        private protected override IPoolContainer<T> Container { get; }

        internal StaticPoolSystem(int size, PoolSystemActionArgs<T> actionArgs) : base(actionArgs) {
            Container = new StaticPoolContainer<T>(size);
        }
    }
}