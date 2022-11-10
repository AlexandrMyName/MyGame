using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public  Weapon weapon;

    public Player (string name,float health,float maxHealth, Weapon weapon) : base( name,  health,  maxHealth) {  this.weapon = weapon; }

}
