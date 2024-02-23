using NUnit.Framework.Internal.Commands;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyBase : IDamagable
{
    [SerializeField] protected int maxHealth;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int damage)
    { 
        currentHealth -= damage;

        if (currentHealth <= 0)
        { 
            
        }
    }

    private void Death()
    {
        // deletes game object
    }
}
