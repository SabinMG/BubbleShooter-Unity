using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
[Event(EventTarget.Self)]
public class BubbleComponent : IComponent
{
    public int value; //value of a bubble 8, 16, 24 
}
