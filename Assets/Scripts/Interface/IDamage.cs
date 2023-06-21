using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public int AttackPow { get; set; }
    public float AttackRate { get; set; }
    public float Range { get; set; }
    public void GiveDamage(int damage,IHealth target);







}
