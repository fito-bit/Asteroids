using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] [Tag] protected string bulletTag;
    [SerializeField] [Tag] protected string laserTag;

    protected void Die()
    {
        
    }
}
