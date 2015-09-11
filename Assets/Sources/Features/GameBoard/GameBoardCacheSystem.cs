using ArrayExtensions;
using Entitas;
using System.Collections.Generic;

public class GameBoardCacheSystem : ISystem, ISetPool
{
    Pool pool;

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;

        var gameBoard = pool.GetGroup(Matcher.GameBoard);
        gameBoard.OnEntityAdded += (group, entity, index, component) =>
        {
            createNewGameBoardCache((GameBoardComponent)component);
        };
        gameBoard.OnEntityUpdated += (group, entity, index, previousComponent, newComponent) =>
        {
            createNewGameBoardCache((GameBoardComponent)newComponent);
        };

        var gameBoardElements = pool.GetGroup(Matcher.AllOf(Matcher.GameBoardElement, Matcher.Position));
        gameBoardElements.OnEntityAdded += onGameBoardElementAdded;
        gameBoardElements.OnEntityUpdated += onGameBoardElementUpdated;
        gameBoardElements.OnEntityRemoved += onGameBoardElementRemoved;
    }

    void createNewGameBoardCache(GameBoardComponent gameBoard)
    {
        var grid = new ICollection<Entity>[gameBoard.columns, gameBoard.rows];
        var entities = pool.GetEntities(Matcher.AllOf(Matcher.GameBoardElement, Matcher.Position));
        foreach (var e in entities)
        {
            var pos = e.position;
            grid.Add(pos.x, pos.y, e);
        }
        pool.ReplaceGameBoardCache(grid);
    }

    void onGameBoardElementAdded(Group group, Entity entity, int index, IComponent component)
    {
        var grid = pool.gameBoardCache.grid;
        var pos = entity.position;
        grid.Add(pos.x, pos.y, entity);
        pool.ReplaceGameBoardCache(grid);
    }

    void onGameBoardElementUpdated(Group group, Entity entity, int index, 
                                   IComponent previousComponent, IComponent newComponent)
    {
        var prevPos = (PositionComponent)previousComponent;
        var newPos = (PositionComponent)newComponent;
        var grid = pool.gameBoardCache.grid;
        grid.Remove(prevPos.x, prevPos.y, entity);
        grid.Add(newPos.x, newPos.y, entity);
        pool.ReplaceGameBoardCache(grid);
    }

    void onGameBoardElementRemoved(Group group, Entity entity, int index, IComponent component)
    {
        var pos = component as PositionComponent;
        if (pos == null)
        {
            pos = entity.position;
        }
        var grid = pool.gameBoardCache.grid;
        grid.Remove(pos.x, pos.y, entity);
        pool.ReplaceGameBoardCache(grid);
    }
}
