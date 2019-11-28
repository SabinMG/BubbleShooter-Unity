using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace SpaceOrigin.BubblePop
{
    public class GameController 
    {
        readonly Systems _systems;

        public GameController(Contexts contexts, IGameConfig gameConfic)
        {
            _systems = new GameSystems(contexts, gameConfic);
        }

        public void Initialize()
        {
            _systems.Initialize();
        }

        public void Execute()
        {
            _systems.Execute();
            _systems.Cleanup();
        }
    }
}
