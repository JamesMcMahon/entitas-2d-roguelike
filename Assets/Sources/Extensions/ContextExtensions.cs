using Entitas;
using ICollectionExtensions;
using System.Collections.Generic;

public static class ContextExtensions
{
    public static void DestroyEntityIfEmpty(this PoolContext context,
                                            PoolEntity entity)
    {
        if (entity.GetComponentIndices().Length == 0)
        {
            context.DestroyEntity(entity);
        }
    }

    public static PoolEntity PlayAudio(this PoolContext context,
                                       BaseAudioComponent source)
    {
        var e = context.CreateEntity();
        e.AddAudio(source.clips, source.randomizePitch);
        return e;
    }

    public static PoolEntity AddToFoodBag(this PoolContext context,
                                          int pointsToAdd)
    {
        int existingPoints = context.foodBag.points;
        context.ReplaceFoodBag(pointsToAdd + existingPoints);
        return context.foodBagEntity;
    }

    public static bool IsGameBoardPositionOpen(this PoolContext context,
                                               PositionComponent position,
                                               out ICollection<PoolEntity> entities)
    {
        return context.IsGameBoardPositionOpen(position.x, position.y, out entities);
    }

    public static bool IsGameBoardPositionOpen(this PoolContext context, int x, int y,
                                               out ICollection<PoolEntity> entities)
    {
        var gameBoard = context.gameBoard;
        bool edge = x == -1 || x == gameBoard.columns || y == -1 || y == gameBoard.rows;
        if (edge)
        {
            entities = null;
            return false;
        }
        entities = context.gameBoardCache.grid[x, y];
        return entities == null || entities.Empty();
    }
}
