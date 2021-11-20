using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyState;
using EnemyType;

[System.Serializable]
public class Blackboard
{
    public Vector3 moveToPosition;
    public EnemyState.State currentState;
    public Type enemyType;
    public GameObject player;
    public bool reachedDestination = true;
}
