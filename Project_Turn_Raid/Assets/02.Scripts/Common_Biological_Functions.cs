using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Common_Biological_Functions : MonoBehaviour, TargetDamage
{
    public float startingHP = 100f; // 시작 체력
    public float currentHP {  get; protected set; } // 현재 체력
    public bool dead {  get; protected set; }

    public event Action onDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        currentHP = startingHP;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        currentHP -= damage;

        if(currentHP <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void RecoveryHP(float newRecoveryHP)
    {
        if(dead)
        {
            return;
        }
        currentHP += newRecoveryHP;
    }

    public virtual void Die()
    {
        if(onDeath != null)
        {
            onDeath();
        }
        dead = true;
    }
}
