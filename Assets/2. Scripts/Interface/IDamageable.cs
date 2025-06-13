using System;
using UnityEngine;

public interface IDamageable
{
    public bool     IsDead   { get; }
    public Collider Collider { get; }
    public void     TakeDamage(IAttackable attacker);
    public void     Dead();
}