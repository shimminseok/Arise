using System;
using UnityEngine;

public interface IDamageable
{
    public bool      IsDead    { get; }
    public Transform Transform { get; }
    public void      TakeDamage(IAttackable attacker);
    public void      Daed();
}