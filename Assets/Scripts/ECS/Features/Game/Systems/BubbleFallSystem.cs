using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using DG.Tweening;
using System.Linq;

public class BubbleFallSystem : ReactiveSystem<GameEntity>
{
    #region private variables
    private readonly IGroup<GameEntity> _group;
    private readonly List<GameEntity> _buffer;
    private readonly Contexts _contexts;
    #endregion

    #region constructor
    public BubbleFallSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }
    #endregion

    #region protected methods
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
       => context.CreateCollector(GameMatcher.Merge);

    protected override bool Filter(GameEntity entity) => entity.merge.mergeCompleted; // anytime if this component is tru this will execute

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            List<List<GameEntity>> islands =  FindIslands();
            FallDisconnectedIslands(islands);
        }
    }
    #endregion

    #region private methods
    private List<List<GameEntity>> FindIslands()
    {
        List<GameEntity> allActiveBubbles = GetAllBubblesIntheGrid();
        List<List<GameEntity>> islands = new List<List<GameEntity>>();
 
        while (allActiveBubbles.Count != 0)
        {
            GameEntity topElement = allActiveBubbles[0];
            List<GameEntity> island = GetAllConnectedBubbles(topElement, allActiveBubbles);
            islands.Add(island);
        }
        return islands;
    }
  
    private void FallDisconnectedIslands(List<List<GameEntity>>  islands)
    {
        for (int i = 0; i < islands.Count; i++)
        {
            List<GameEntity> island = islands[i];
            List<GameEntity> islandDis = island.FindAll(s => s.hexCell.coordinate.x.Equals(0));

            if (islandDis.Count != 0) return;
            for (int j = 0; j < island.Count; j++)
            {
                GameEntity fallEntity = island[j];
                if (fallEntity.hasHexCell) fallEntity.RemoveHexCell();
                FallBubble(fallEntity);
            }
        }
    }

    private void FallBubble(GameEntity bubble)
    {
        Vector2 moveVector = bubble.position.value;
        Vector2 fallPosition = new Vector2(moveVector.x + Random.Range(-1.5f,1.5f), -9.8f);
        DOTween.To(() => moveVector, x => moveVector = x, fallPosition, .6f).SetEase(Ease.InQuad).SetOptions(false).OnUpdate(() =>
        {
            if (!bubble.hasPosition) Debug.Log("entity is laready destroyed");
            bubble.ReplacePosition(moveVector);
        }).OnComplete(() =>
        {
            bubble.isDestroyed = true;
        });
    }

    private List<GameEntity> GetAllConnectedBubbles(GameEntity mergeBubble, List<GameEntity> removeItemList)
    {
        List<GameEntity> connectedBubbles = new List<GameEntity>();
        connectedBubbles.Add(mergeBubble);
        removeItemList.Remove(mergeBubble);

        GetAllConnectedBubbles(mergeBubble, connectedBubbles, removeItemList);
        return connectedBubbles;
    }

    private void GetAllConnectedBubbles(GameEntity mergeBubble, List<GameEntity> list, List<GameEntity> removeItemList)
    {
        HexOffsetCoord hexCoord = new HexOffsetCoord(mergeBubble.hexCell.coordinate.x, mergeBubble.hexCell.coordinate.y);
        List<HexOffsetCoord> neighbourCoords = GexGridService.GetAllNeightbourCoords(_contexts, hexCoord);

        for (int i = 0; i < neighbourCoords.Count; i++)
        {
            Vector2Int hexCellCoord = new Vector2Int(neighbourCoords[i].row, neighbourCoords[i].col);
            GameEntity neighbourCell = _contexts.game.GetNeighbourWithCellValue(hexCellCoord);

            if (neighbourCell != null && !list.Contains(neighbourCell))
            {
                list.Add(neighbourCell);
                removeItemList.Remove(neighbourCell);
                GetAllConnectedBubbles(neighbourCell, list, removeItemList);
            }
        }
    }

    private List<GameEntity> GetAllBubblesIntheGrid()
    {
        List<GameEntity> allBubbles = new List<GameEntity>();
        int rows = _contexts.game.hexGridSize.m_rows;
        int columns = _contexts.game.hexGridSize.m_columns;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector2Int hexCellCoord = new Vector2Int(i, j);
                GameEntity bubbleEntity = _contexts.game.GetEntityWithHexCell(hexCellCoord);
                if (bubbleEntity != null) allBubbles.Add(bubbleEntity);
            }
        }
        return allBubbles;
    }
    #endregion
}
