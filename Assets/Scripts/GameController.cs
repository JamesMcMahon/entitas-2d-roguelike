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
            .Add(pool.CreateSystem<GameStartSystem>())
            .Add(pool.CreateSystem<GameOverSystem>())

            .Add(pool.CreateSystem<GameBoardCacheSystem>())
            .Add(pool.CreateSystem<CreateGameBoardSystem>())

            .Add(pool.CreateSystem<TurnSystem>())
            .Add(pool.CreateSystem<InputSystem>())
            .Add(pool.CreateSystem<AIMoveSystem>())

            .Add(pool.CreateSystem<ExitSystem>())
            .Add(pool.CreateSystem<FoodSystem>())
            .Add(pool.CreateSystem<DestructibleSystem>())

            // Render
            .Add(pool.CreateSystem<AnimationSystem>())
            .Add(pool.CreateSystem<DamageSpriteSystem>())
            .Add(pool.CreateSystem<RemoveViewSystem>())
            .Add(pool.CreateSystem<AddViewSystem>())
            .Add(pool.CreateSystem<RenderPositionSystem>())
            .Add(pool.CreateSystem<SmoothMoveSystem>());

        return systems;
    }
}
