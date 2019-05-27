using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class EnemyStates : MonoBehaviour
  {
    
    Animator animator;
    EnemyTarget enemyTarget;

    void Start()
    {
      animator = GetComponentInChildren<Animator>();
      enemyTarget = GetComponent<EnemyTarget>();
      enemyTarget.Init(animator);
    }
  }
}
