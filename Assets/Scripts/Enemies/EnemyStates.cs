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
    public bool isDead;

    public Animator animator;
    EnemyTarget enemyTarget;
    AnimatorHook animatorHook;
    public Rigidbody rigidBody;
    public float delta;

    List<Rigidbody> ragdollRigids = new List<Rigidbody>();
    List<Collider> ragdollColliders = new List<Collider>();
    void Start()
    {
      health = 100;
      animator = GetComponentInChildren<Animator>();
      enemyTarget = GetComponent<EnemyTarget>();
      enemyTarget.Init(this);

      rigidBody = GetComponent<Rigidbody>();

      animatorHook = animator.GetComponent<AnimatorHook>();
      if (animatorHook == null)
        animatorHook = animator.gameObject.AddComponent<AnimatorHook>();
      animatorHook.Init(null, this);

      InitRagdoll();
    }

    void InitRagdoll()
    {
      Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();
      for (int i = 0; i < rigs.Length; i++)
      {
        if (rigs[i] == rigidBody)
          continue;

        ragdollRigids.Add(rigs[i]);
        rigs[i].isKinematic = true;

        Collider collider = rigs[i].gameObject.GetComponent<Collider>();
        collider.isTrigger = true;
        ragdollColliders.Add(collider);
      }
    }

    public void EnableRagdoll()
    {
      for (int i = 0; i < ragdollRigids.Count; i++)
      {
        ragdollRigids[i].isKinematic = false;
        ragdollColliders[i].isTrigger = false;
      }

      Collider controllerCollider = rigidBody.gameObject.GetComponent<Collider>();
      controllerCollider.enabled = false;
      rigidBody.isKinematic = true;

      StartCoroutine("CloseAnimator");
    }

    IEnumerator CloseAnimator()
    {
      yield return new WaitForEndOfFrame();
      animator.enabled = false;
      this.enabled = false;
    }

    void Update()
    {
      delta = Time.deltaTime;
      canMove = animator.GetBool("can_move");

      if (health <= 0)
      {
        if (!isDead)
        {
          isDead = true;
          EnableRagdoll();
        }
      }

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
