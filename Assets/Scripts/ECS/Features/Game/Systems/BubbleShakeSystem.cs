using System.Collections.Generic;
using DG.Tweening;
using Entitas;
using UnityEngine;

/// <summary>
/// Shakes system will shakes the neighbour bubbles when the shooter reaches the target
/// </summary>
public class BubbleShakeSystem : ReactiveSystem<GameEntity>
{
    #region private variables
    private readonly IGroup<GameEntity> _group;
    private readonly List<GameEntity> _buffer;
    private readonly Contexts _contexts;
    #endregion

    #region constructor
    public BubbleShakeSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
       => context.CreateCollector(GameMatcher.Shooter);

    protected override bool Filter(GameEntity entity) => entity.hasShooter && entity.shooter.rechedTarget;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            GameEntity shooterBubble = e;
            ShakeAllNeighbours(shooterBubble);
            e.RemoveShooter();
        }
    }
    #endregion

    #region  private methods
    private void ShakeAllNeighbours(GameEntity shootBubble)
    {
        HexOffsetCoord hexCoord = new HexOffsetCoord(shootBubble.hexCell.coordinate.x, shootBubble.hexCell.coordinate.y);
        List<HexOffsetCoord> neighbourCoords = GexGridService.GetAllNeightbourCoords(_contexts, hexCoord);

        for (int i = 0; i < neighbourCoords.Count; i++)
        {
            Vector2Int hexCellCoord = new Vector2Int(neighbourCoords[i].row, neighbourCoords[i].col); // HexOffsetCoord should contain a vectorint: TODO
            GameEntity neighbourCell = _contexts.game.GetNeighbourWithCellValue(hexCellCoord);
            if (neighbourCell != null)
            {
                ShakeThisBubble(shootBubble, neighbourCell);
            }
        }
    }

    private void ShakeThisBubble(GameEntity center, GameEntity target)
    {
        Vector2 direction = target.position.value - center.position.value;

        Vector2 startPosition = target.position.value;
        Vector2 shakeVector = startPosition;
        Vector2 ShakePosition = startPosition + direction*.07f;
        DOTween.To(() => shakeVector, x => shakeVector = x, ShakePosition, .1f).OnUpdate(() =>
        {
            target.ReplacePosition(shakeVector);
        }).OnComplete(() =>
        {
            DOTween.To(() => shakeVector, x => shakeVector = x, startPosition, .1f).OnUpdate(() =>
            {
                target.ReplacePosition(shakeVector);
            }).OnComplete(() =>
            {
            });
        });
    }
    #endregion
}
