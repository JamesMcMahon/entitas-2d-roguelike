using Entitas;
using Entitas.Unity.VisualDebugging;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool useSeed = false;
    public int randomSeed = 42;

    Systems systems;

    void Start()
    {
        if (useSeed)
        {
            Random.seed = randomSeed;
        }

        Debug.Log("Starting GameController using seed " + Random.seed);
        systems = CreateSystems(Pools.pool);
        systems.Initialize();
    }

    void Update()
    {
        systems.Execute();
    }

    static Systems CreateSystems(Pool pool)
    {
        Systems systems;

#if (UNITY_EDITOR)
        systems = new DebugSystems();
#else
        systems = new Systems();
#endif

        systems
            .Add(pool.CreateGameStartSystem())
            .Add(pool.CreateGameOverSystem())

            .Add(pool.CreateGameBoardCacheSystem())
            .Add(pool.CreateCreateGameBoardSystem())

            .Add(pool.CreateTurnSystem())
            .Add(pool.CreateInputSystem())
            .Add(pool.CreateAIMoveSystem())

            .Add(pool.CreateExitSystem())
            .Add(pool.CreateFoodSystem())
            .Add(pool.CreateDestructibleSystem())

            // Render
            .Add(pool.CreateAnimationSystem())
            .Add(pool.CreateDamageSpriteSystem())
            .Add(pool.CreateRemoveViewSystem())
            .Add(pool.CreateAddViewSystem())
            .Add(pool.CreateRenderPositionSystem())
            .Add(pool.CreateSmoothMoveSystem());

        return systems;
    }
}
