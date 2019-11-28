using Entitas;
using DG.Tweening;
using UnityEngine;

public class BubbleView : View
{
    #region public variables
    public SpriteRenderer _sprite;
    public TextMesh _textMesh;
    public BubbleColorsSO _bubbleColorsSO;
    #endregion

    #region public methods
    public override void Link(IEntity entity)
    {
        base.Link(entity);
        var gameEntity = (GameEntity)entity;
        int bubbleValue = gameEntity.bubble.value;
        _textMesh.text = bubbleValue.ToString();
        _sprite.color = _bubbleColorsSO.GetColorFortheValue(bubbleValue);

        transform.DOScale(Vector3.one, Random.Range(0.4f, 0.8f));
    }

    public override void OnPosition(GameEntity entity, Vector2 value)
    {
        transform.position = new Vector3(value.x, value.y, 0);
    }
    #endregion

    #region protected methods
    protected override void Destroy()
    {
        base.Destroy();
    }
    #endregion
}
