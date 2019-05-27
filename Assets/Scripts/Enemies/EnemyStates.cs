using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class EnemyStates : MonoBehaviour
  {
    public float health;
    public bool isInvisible;
    Animator animator;
    EnemyTarget enemyTarget;

    void Start()
    {
      animator = GetComponentInChildren<Animator>();
      enemyTarget = GetComponent<EnemyTarget>();
      enemyTarget.Init(animator);
    }

    void Update()
    {
      if (isInvisible)
      {
        isInvisible = !animator.GetBool("can_move");
      }
    }

    public void DoDamage(float value)
    {
      if (isInvisible)
        return;

      health -= value;
      isInvisible = true;
      animator.Play("damage_1");
    }
  }
}
