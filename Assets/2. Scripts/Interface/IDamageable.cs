using System;
using UnityEngine;

public interface IDamageable
{
    public bool     IsDead   { get; }
    public Collider Collider { get; }

    /// <summary>
    /// 대미지를 주는 메서드
    /// </summary>
    /// <param name="attacker">공격을 실행한 대상</param>
    public void     TakeDamage(IAttackable attacker);
    public void     Dead();
}