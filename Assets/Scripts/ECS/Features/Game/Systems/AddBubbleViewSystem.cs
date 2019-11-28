using System.Collections.Generic;
using Entitas;
using UnityEngine;

/// <summary>
/// this system will add new bubble to the view
/// </summary>
public sealed class AddBubbleViewSystem : ReactiveSystem<GameEntity>
{
    #region private variables
    private readonly IGroup<GameEntity> _group;
    private readonly List<GameEntity> _buffer;
    private readonly Contexts _contexts;
    private readonly Transform _parent;
    #endregion

    #region constructor
    public AddBubbleViewSystem(Contexts contexts) : base(contexts.game) // constructor
    {
        _parent = new GameObject("Views").transform;
        _contexts = contexts;
    }
    #endregion

    #region protected methods
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
       => context.CreateCollector(GameMatcher.Asset);

    protected override bool Filter(GameEntity entity) => entity.hasAsset && !entity.hasView;


    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.AddView(InstantiateView(e));
        }
    }
    #endregion

    #region private methods
    private IView InstantiateView(GameEntity entity)
    {
        var prefab = Resources.Load<GameObject>(entity.asset.value);
        var view = Object.Instantiate(prefab, _parent).GetComponent<IView>();

        view.Link(entity);
        return view;
    }
    #endregion
}