using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
[Unique]
[Event(EventTarget.Self)]
public class HexCellRadiusComponet : IComponent
{
    public float value;
}
