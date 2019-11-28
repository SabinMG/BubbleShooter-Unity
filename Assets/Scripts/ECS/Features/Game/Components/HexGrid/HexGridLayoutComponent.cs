using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using Entitas.CodeGeneration.Attributes;

public enum HorizontalRowLayout
{
    Odd,
    Even,
}

[Game]
[Unique]
[Event(EventTarget.Self)]
public class HexGridLayoutComponent : IComponent
{
    public HorizontalRowLayout m_layoutType;
}

