public static class ComponentIds {
    public const int ActiveTurnBased = 0;
    public const int AIMove = 1;
    public const int AIMoveTarget = 2;
    public const int Animation = 3;
    public const int AudioAttackSource = 4;
    public const int Audio = 5;
    public const int AudioDeathSource = 6;
    public const int AudioPickupSource = 7;
    public const int AudioWalkSource = 8;
    public const int Controllable = 9;
    public const int DamageSprite = 10;
    public const int DeleteOnExit = 11;
    public const int Destructible = 12;
    public const int Exit = 13;
    public const int FoodBag = 14;
    public const int Food = 15;
    public const int FoodDamager = 16;
    public const int GameBoardCache = 17;
    public const int GameBoard = 18;
    public const int GameBoardElement = 19;
    public const int GameOver = 20;
    public const int Level = 21;
    public const int LevelTransitionDelay = 22;
    public const int MoveInput = 23;
    public const int NestedView = 24;
    public const int Position = 25;
    public const int Resource = 26;
    public const int SkipTurn = 27;
    public const int SmoothMove = 28;
    public const int SmoothMoveInProgress = 29;
    public const int TurnBased = 30;
    public const int View = 31;

    public const int TotalComponents = 32;

    static readonly string[] components = {
        "ActiveTurnBased",
        "AIMove",
        "AIMoveTarget",
        "Animation",
        "AudioAttackSource",
        "Audio",
        "AudioDeathSource",
        "AudioPickupSource",
        "AudioWalkSource",
        "Controllable",
        "DamageSprite",
        "DeleteOnExit",
        "Destructible",
        "Exit",
        "FoodBag",
        "Food",
        "FoodDamager",
        "GameBoardCache",
        "GameBoard",
        "GameBoardElement",
        "GameOver",
        "Level",
        "LevelTransitionDelay",
        "MoveInput",
        "NestedView",
        "Position",
        "Resource",
        "SkipTurn",
        "SmoothMove",
        "SmoothMoveInProgress",
        "TurnBased",
        "View"
    };

    public static string IdToString(int componentId) {
        return components[componentId];
    }
}

namespace Entitas {
    public partial class Matcher : AllOfMatcher {
        public Matcher(int index) : base(new [] { index }) {
        }

        public override string ToString() {
            return ComponentIds.IdToString(indices[0]);
        }
    }
}