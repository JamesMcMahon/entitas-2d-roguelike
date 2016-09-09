using Entitas;
using ICollectionExtensions;
using System.Collections.Generic;

public static class PoolExtensions
{
    public static void DestroyEntityIfEmpty(this Pool pool, Entity entity)
    {
        if (entity.GetComponentIndices().Length == 0)
        {
            pool.DestroyEntity(entity);
        }
    }

    public static Entity PlayAudio(this Pool pool, AudioComponent source)
    {
        return pool.CreateEntity().AddAudio(source.clips, source.randomizePitch);
    }

    public static Entity AddToFoodBag(this Pool pool, int pointsToAdd)
    {
        int existingPoints = pool.foodBag.points;
        return pool.ReplaceFoodBag(pointsToAdd + existingPoints);
    }

    public static bool IsGameBoardPositionOpen(this Pool pool,
                                               PositionComponent position,
                                               out ICollection<Entity> entities)
    {
        return pool.IsGameBoardPositionOpen(position.x, position.y, out entities);
    }

    public static bool IsGameBoardPositionOpen(this Pool pool, int x, int y,
                                               out ICollection<Entity> entities)
    {
        var gameBoard = pool.gameBoard;
        bool edge = x == -1 || x == gameBoard.columns || y == -1 || y == gameBoard.rows;
        if (edge)
        {
            entities = null;
            return false;
        }
        entities = pool.gameBoardCache.grid[x, y];
        return entities == null || entities.Empty();
    }
}
