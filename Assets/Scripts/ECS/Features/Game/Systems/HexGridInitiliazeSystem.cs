using System.Collections.Generic;
using Entitas;
using UnityEngine;

/// <summary>
/// Initialize the hex grid
/// </summary>
public class HexGridInitiliazeSystem : IInitializeSystem
{
    #region private variables
    private readonly Contexts m_contexts;
    private readonly IGameConfig _gameConfig;
    #endregion

    #region constructor
    public HexGridInitiliazeSystem(Contexts contexts, IGameConfig gameConfig) 
    {
        m_contexts = contexts;
        _gameConfig = gameConfig;
    }
    #endregion

    #region public methods
    public void Initialize()
    {
        GameEntity gridEnity = m_contexts.game.CreateEntity();
        gridEnity.AddHexGridSize(_gameConfig.hexGridSize.x, _gameConfig.hexGridSize.y);
        gridEnity.AddHexGridLayout(_gameConfig.hexGridInitLayout);
        gridEnity.AddHexCellRadiusComponet(_gameConfig.hexCellRadius);
    }
    #endregion
}

