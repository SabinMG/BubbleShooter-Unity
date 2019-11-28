using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class GameEntityCleanupSystem : ICleanupSystem
{
    #region private variables
    readonly IGroup<GameEntity> _group;
    readonly List<GameEntity> _buffer = new List<GameEntity>();
    #endregion

    #region constructor
    public GameEntityCleanupSystem(Contexts contexts)
    {
        _group = contexts.game.GetGroup(GameMatcher.Destroyed);
    }
    #endregion

    #region public methods
    public void Cleanup()
    {
        foreach (var e in _group.GetEntities(_buffer))
        {
            e.Destroy();  
        }
    }
    #endregion
}
