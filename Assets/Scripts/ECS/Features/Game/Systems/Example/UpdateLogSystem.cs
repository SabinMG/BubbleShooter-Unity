using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace SpaceOrigin.BubblePop
{ 
    public class UpdateLogSystem : IExecuteSystem
    {
        readonly IGroup<GameEntity> m_entities;

        public UpdateLogSystem(Contexts context)
        {
            m_entities = context.game.GetGroup(GameMatcher.Position);
        }

        public void Execute()
        {
            foreach (GameEntity ge in m_entities)
            {
                Debug.Log(ge.position.value);
            }
        }
    }
}
