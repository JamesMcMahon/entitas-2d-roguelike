using Entitas;

public class CoroutineSystem : IExecuteSystem
{
    readonly PoolContext pool;
    readonly IGroup<PoolEntity> coroutinesGroup;

    public CoroutineSystem(Contexts contexts)
    {
        pool = contexts.pool;
        coroutinesGroup = pool.GetGroup(PoolMatcher.Coroutine);
    }

    void IExecuteSystem.Execute()
    {
        foreach (var e in coroutinesGroup.GetEntities())
        {
            var coroutine = e.coroutine.value;
            if (!coroutine.MoveNext())
            {
                e.RemoveCoroutine();
                pool.DestroyEntityIfEmpty(e);
            }
        }
    }
}
