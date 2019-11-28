using Entitas;
using Entitas.Unity;
using UnityEngine;

public class View : MonoBehaviour, IView, IPositionListener,IDestroyedListener
{
    #region public methods
    public virtual void Link(IEntity entity)
    {
        gameObject.Link(entity);
        var e = (GameEntity)entity;
        e.AddPositionListener(this);
        e.AddDestroyedListener(this);
        var pos = e.position.value;
        transform.position = new Vector3(pos.x, pos.y);
        transform.localScale = new Vector3(0,0,0);
    }

    public virtual void OnPosition(GameEntity entity, Vector2 value)
    {
        transform.position = new Vector3(value.x, value.y);
    }

    // take care of this later
    public virtual void OnDestroyed(GameEntity entity)
    {
        Destroy();
    }
    #endregion

    #region protected methods
    protected virtual void Destroy()
    {
        gameObject.Unlink();
        Destroy(gameObject);
    }
    #endregion
}
