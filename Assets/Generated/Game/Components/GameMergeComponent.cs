//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public MergeComponent merge { get { return (MergeComponent)GetComponent(GameComponentsLookup.Merge); } }
    public bool hasMerge { get { return HasComponent(GameComponentsLookup.Merge); } }

    public void AddMerge(bool newCheckForMerge, bool newMergeCompleted) {
        var index = GameComponentsLookup.Merge;
        var component = (MergeComponent)CreateComponent(index, typeof(MergeComponent));
        component.checkForMerge = newCheckForMerge;
        component.mergeCompleted = newMergeCompleted;
        AddComponent(index, component);
    }

    public void ReplaceMerge(bool newCheckForMerge, bool newMergeCompleted) {
        var index = GameComponentsLookup.Merge;
        var component = (MergeComponent)CreateComponent(index, typeof(MergeComponent));
        component.checkForMerge = newCheckForMerge;
        component.mergeCompleted = newMergeCompleted;
        ReplaceComponent(index, component);
    }

    public void RemoveMerge() {
        RemoveComponent(GameComponentsLookup.Merge);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherMerge;

    public static Entitas.IMatcher<GameEntity> Merge {
        get {
            if (_matcherMerge == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Merge);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMerge = matcher;
            }

            return _matcherMerge;
        }
    }
}
