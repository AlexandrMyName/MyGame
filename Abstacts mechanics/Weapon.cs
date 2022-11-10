using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : IWeapon
{
   public abstract float Damage { get; }

   public abstract string Name { get; }

   public  string Object => "Œ–”∆»≈";
}
