using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class GameSystems : Feature
{
    public GameSystems(Contexts contexts, IGameConfig gameCongig)
    {
        // hex grid set up
        Add(new HexGridInitiliazeSystem(contexts, gameCongig));
        Add(new HexGridLayoutUpdateSystem(contexts));
        // events system
        Add(new GameEventSystems(contexts));
        // create initial four rows of bubbles
        Add(new CreateInitialBubblesSystem(contexts, gameCongig));
        //adds new bubbles to the view
        Add(new AddBubbleViewSystem(contexts));
        //adds shake system, excutes when bubble hits
        Add(new BubbleShakeSystem(contexts));

        Add(new BubbleMergeSystem(contexts));
        Add(new BubbleFallSystem(contexts));

        Add(new GameEntityCleanupSystem(contexts));
    }
}