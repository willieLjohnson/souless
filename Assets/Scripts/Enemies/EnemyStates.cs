using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class EnemyStates : MonoBehaviour
  {
    public float health;
    public bool isInvisible;
    public bool canMove;
    public Animator animator;
    EnemyTarget enemyTarget;
    AnimatorHook animatorHook;
    public Rigidbody rigidBody;
    public float delta;
    void Start()
    {
      animator = GetComponentInChildren<Animator>();
      enemyTarget = GetComponent<EnemyTarget>();
      enemyTarget.Init(animator);

      rigidBody = GetComponent<Rigidbody>();

      animatorHook = animator.GetComponent<AnimatorHook>();
      if (animatorHook == null)
        animatorHook = animator.gameObject.AddComponent<AnimatorHook>();
      animatorHook.Init(null, this);
    }

    void Update()
    {
      delta = Time.deltaTime;
      canMove = animator.GetBool("can_move");
      if (isInvisible)
      {
        isInvisible = !canMove;
      }

      if (canMove)
      {
        animator.applyRootMotion = false;
      }
    }

    public void DoDamage(float value)
    {
      if (isInvisible)
        return;

      health -= value;
      isInvisible = true;
      animator.Play("damage_1");
      animator.applyRootMotion = true;
      animator.SetBool("can_move", false);
    }
  }
}
