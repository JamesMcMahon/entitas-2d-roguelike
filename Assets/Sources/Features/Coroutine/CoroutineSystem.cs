using Entitas;

public class CoroutineSystem : ISetPool, IExecuteSystem
{
    Pool pool;
    Group coroutinesGroup;

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;
        coroutinesGroup = pool.GetGroup(Matcher.Coroutine);
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
