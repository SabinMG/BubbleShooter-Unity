using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace SpaceOrigin.BubblePop
{
    public sealed class ReactiveLogSystem : ReactiveSystem<GameEntity>
    {
       
        private readonly IGroup<GameEntity> _group;
        private readonly List<GameEntity> _buffer;

        public ReactiveLogSystem(Contexts contexts) : base(contexts.game)
        {
          
        }
   
        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Position);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasPosition;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity ge in entities)
            {
                Debug.Log(ge.position.value);
            }
        }
    }
}