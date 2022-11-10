using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon, IWeaponGun
{
    public override float Damage => 60f;

    public override string Name => "ксй";
}
