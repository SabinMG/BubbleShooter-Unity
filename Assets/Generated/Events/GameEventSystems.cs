//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventSystemsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class GameEventSystems : Feature {

    public GameEventSystems(Contexts contexts) {
        Add(new BubbleEventSystem(contexts)); // priority: 0
        Add(new DestroyedEventSystem(contexts)); // priority: 0
        Add(new HexCellRadiusComponetEventSystem(contexts)); // priority: 0
        Add(new HexGridLayoutEventSystem(contexts)); // priority: 0
        Add(new HexGridSizeEventSystem(contexts)); // priority: 0
        Add(new PositionEventSystem(contexts)); // priority: 0
    }
}