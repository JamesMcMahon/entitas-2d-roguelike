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
            CreateNewGameBoardCache((GameBoardComponent)component);
        gameBoard.OnEntityUpdated += (group, entity, index, previousComponent, newComponent) =>
            CreateNewGameBoardCache((GameBoardComponent)newComponent);

        var gameBoardElements = pool.GetGroup(Matcher.AllOf(Matcher.GameBoardElement, Matcher.Position));
        gameBoardElements.OnEntityAdded += OnGameBoardElementAdded;
        gameBoardElements.OnEntityUpdated += OnGameBoardElementUpdated;
        gameBoardElements.OnEntityRemoved += OnGameBoardElementRemoved;
    }

    void CreateNewGameBoardCache(GameBoardComponent gameBoard)
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

    void OnGameBoardElementAdded(Group group, Entity entity, int index, IComponent component)
    {
        var grid = pool.gameBoardCache.grid;
        var pos = entity.position;
        grid.Add(pos.x, pos.y, entity);
        pool.ReplaceGameBoardCache(grid);
    }

    void OnGameBoardElementUpdated(Group group, Entity entity, int index, 
                                   IComponent previousComponent, IComponent newComponent)
    {
        var prevPos = (PositionComponent)previousComponent;
        var newPos = (PositionComponent)newComponent;
        var grid = pool.gameBoardCache.grid;
        grid.Remove(prevPos.x, prevPos.y, entity);
        grid.Add(newPos.x, newPos.y, entity);
        pool.ReplaceGameBoardCache(grid);
    }

    void OnGameBoardElementRemoved(Group group, Entity entity, int index, IComponent component)
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
