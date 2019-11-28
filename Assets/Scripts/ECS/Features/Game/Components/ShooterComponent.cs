using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using Entitas.CodeGeneration.Attributes;

// this compoent will be added when buble shoots
[FlagPrefix("flag")]
[Game]
public class ShooterComponent : IComponent
{
  
   public bool rechedTarget = false;
}
