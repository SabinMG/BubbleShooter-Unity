using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

// not it should be recctive sytem, update when the rows are move up and down

public class HexGridLayoutUpdateSystem : IExecuteSystem
{
    #region private variables
    readonly IGroup<GameEntity> m_entities;
    #endregion

    #region constructor
    public HexGridLayoutUpdateSystem(Contexts context)
    {
        m_entities = context.game.GetGroup(GameMatcher.HexGridLayout);
    }
    #endregion

    #region public methods
    public void Execute()
    {
        foreach (GameEntity ge in m_entities)
        {
           // ge.hexGridLayout.m_layoutType = HorizontalRowLayout.Even; 
            //Debug.Log(ge.hexGridLayout.m_layoutType);
        }
    }
    #endregion
}
