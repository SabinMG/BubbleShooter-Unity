using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using DG.Tweening;

public class BubbleMergeSystem : ReactiveSystem<GameEntity>
{
    #region private varibles
    private readonly IGroup<GameEntity> _group;
    private readonly List<GameEntity> _buffer;
    private readonly Contexts _contexts;
    #endregion

    #region constructor
    public BubbleMergeSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }
    #endregion

    #region protected methods
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
       => context.CreateCollector(GameMatcher.Merge);

    protected override bool Filter(GameEntity entity) => entity.merge.checkForMerge; // anytime if this component is tru this will execute

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            GameEntity mergeBubble = e;
            int bubbleValue = mergeBubble.bubble.value;
            int sum = bubbleValue;
            List<GameEntity> connectedNodes = GetAllConnectedBubbles(mergeBubble);
            if (connectedNodes.Count > 0)
            {
                for (int i = 0; i < connectedNodes.Count -1; i++)
                {
                    sum = sum * 2;
                }

                if (sum >= 2048)
                {
                    for (int i = 0; i < connectedNodes.Count; i++)
                    {
                        connectedNodes[i].isDestroyed = true;
                    }
                }
                else
                {
                    GameEntity possibleMergePoint = GetNextPossibleMergePoint(sum, connectedNodes);
                    if (possibleMergePoint != null)
                    {
                        Merge(sum, possibleMergePoint, connectedNodes);
                    }
                }
            }
        }
    }
    #endregion

    #region private methods
    private List<GameEntity> GetAllConnectedBubbles(GameEntity mergeBubble)
    {
        List<GameEntity> connectedBubbles = new List<GameEntity>();
        GetAllConnectedBubbles(mergeBubble, connectedBubbles);
        return connectedBubbles;
    }

    private void GetAllConnectedBubbles(GameEntity mergeBubble, List<GameEntity> list)
    {
        HexOffsetCoord hexCoord = new HexOffsetCoord(mergeBubble.hexCell.coordinate.x, mergeBubble.hexCell.coordinate.y);
        List<HexOffsetCoord> neighbourCoords = GexGridService.GetAllNeightbourCoords(_contexts, hexCoord);

        for (int i = 0; i < neighbourCoords.Count; i++)
        {
            Vector2Int hexCellCoord = new Vector2Int(neighbourCoords[i].row, neighbourCoords[i].col); 
            GameEntity neighbourCell = _contexts.game.GetNeighbourWithCellValue(hexCellCoord);

            if (neighbourCell != null && mergeBubble.bubble.value == neighbourCell.bubble.value && !list.Contains(neighbourCell))
            {
                list.Add(neighbourCell);
                GetAllConnectedBubbles(neighbourCell, list);
            }
        }
    }

    private GameEntity GetNextPossibleMergePoint(int sumValue, List<GameEntity> connectedNodes)
    {
        GameEntity possiblePoint1 = null; // point where is next merge is possible
        GameEntity possiblePoint2 = null; // when there is no next merge, then chose higher row

        for (int i = 0; i < connectedNodes.Count; i++)
        {
            if (possiblePoint2 == null) possiblePoint2 = connectedNodes[i];
            if (possiblePoint2.hexCell.coordinate.x > connectedNodes[i].hexCell.coordinate.x) possiblePoint2 = connectedNodes[i];

            HexOffsetCoord hexCoord = new HexOffsetCoord(connectedNodes[i].hexCell.coordinate.x, connectedNodes[i].hexCell.coordinate.y);
            List<HexOffsetCoord> neighbourCoords = GexGridService.GetAllNeightbourCoords(_contexts, hexCoord);

            for (int j = 0; j < neighbourCoords.Count; j++)
            {
                Vector2Int hexCellCoord = new Vector2Int(neighbourCoords[j].row, neighbourCoords[j].col);
                GameEntity neighbourCell = _contexts.game.GetNeighbourWithCellValue(hexCellCoord);
                if (neighbourCell != null && neighbourCell.bubble.value == sumValue)
                {
                    if(possiblePoint1 == null) possiblePoint1 = connectedNodes[i];
                    if(possiblePoint1.hexCell.coordinate.x > connectedNodes[i].hexCell.coordinate.x) possiblePoint1 = connectedNodes[i];
                } 
            }
        }
        return possiblePoint1 == null? possiblePoint2: possiblePoint1;       
    }

    private void Merge(int newValue, GameEntity mergePoint, List<GameEntity> mergeBubbles)
    {
        Vector2 mergPosition = mergePoint.position.value;
        Vector2Int hexCellCoord = mergePoint.hexCell.coordinate;
        int mergeCounter = 0;

        for (int i = 0; i < mergeBubbles.Count; i++)
        {
            GameEntity bubble = mergeBubbles[i];
            Vector2 moveVector = bubble.position.value;
            DOTween.To(() => moveVector, x => moveVector = x, mergPosition, .25f).SetEase(Ease.InQuad).SetOptions(false).OnUpdate(() =>
            {
                if (!bubble.hasPosition) Debug.Log("entity is laready destroyed");
                bubble.ReplacePosition(moveVector);
            }).OnComplete(() =>
            {
                bubble.RemoveHexCell();
                bubble.isDestroyed = true;
                mergeCounter++;
                if (mergeCounter == mergeBubbles.Count)
                {
                    GameEntity bubbleEntity = _contexts.game.CreateEntity();
                    bubbleEntity.AddPosition(new Vector2(mergPosition.x, mergPosition.y));
                    bubbleEntity.AddBubble(newValue);
                    bubbleEntity.AddAsset("Bubble");
                    bubbleEntity.AddHexCell(hexCellCoord);
                    bubbleEntity.AddMerge(true, true);
                }
            });
        }
    }

    private void SetCell(Vector2Int hexCellCoord, GameEntity bubbleEntity)
    {
        bubbleEntity.AddHexCell(hexCellCoord);
    }
    #endregion
}
