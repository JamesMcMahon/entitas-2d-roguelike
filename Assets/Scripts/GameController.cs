using Entitas;
using Entitas.VisualDebugging.Unity;
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
        var contexts = Contexts.sharedInstance;
        systems = CreateSystems(contexts);
        systems.Initialize();
    }

    void Update()
    {
        systems.Execute();
    }

    static Systems CreateSystems(Contexts contexts)
    {
        Systems systems;

#if (UNITY_EDITOR)
        systems = new DebugSystems("Editor");
#else
        systems = new Systems();
#endif

        systems
            .Add(new CoroutineSystem(contexts))
            .Add(new GameStartSystem(contexts))
            .Add(new GameOverSystem(contexts))

            .Add(new GameBoardCacheSystem(contexts))
            .Add(new CreateGameBoardSystem(contexts))

            .Add(new TurnSystem(contexts))
            .Add(new InputSystem(contexts))
            .Add(new AIMoveSystem(contexts))

            .Add(new ExitSystem(contexts))
            .Add(new FoodSystem(contexts))
            .Add(new DestructibleSystem(contexts))

        // Render
            .Add(new AnimationSystem(contexts))
            .Add(new DamageSpriteSystem(contexts))
            .Add(new RemoveViewSystem(contexts))
            .Add(new AddViewSystem(contexts))
            .Add(new RenderPositionSystem(contexts))
            .Add(new SmoothMoveSystem(contexts));

        return systems;
    }
}
