using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class AnimatorHook : MonoBehaviour
  {
    Animator animator;
    StateManager stateManager;
    EnemyStates enemyStates;
    Rigidbody rigidBody;


    public float rootMotionMultiplier;
    bool rolling;
    float rollTime;
    float delta;
    AnimationCurve rollCurve;

    public void Init(StateManager stateManager, EnemyStates enemyStates)
    {
      this.stateManager = stateManager;
      this.enemyStates = enemyStates;
      if (stateManager != null)
      {
        animator = stateManager.animator;
        rigidBody = stateManager.rigidBody;
        rollCurve = stateManager.rollCurve;
        delta = stateManager.delta;
      }

      if (enemyStates != null)
      {
        animator = enemyStates.animator;
        rigidBody = enemyStates.rigidBody;
        delta = enemyStates.delta;
      }
    }

    public void InitForRoll()
    {
      rolling = true;
      rollTime = 0;
    }

    public void CloseRoll()
    {
      if (!rolling)
        return;

      rootMotionMultiplier = 1;
      rollTime = 0;
      rolling = false;
    }

    void OnAnimatorMove()
    {
      if (stateManager == null && enemyStates == null)
        return;

      if (rigidBody == null)
        return;

      if (stateManager != null)
      {
        if (stateManager.canMove)
          return;

        delta = stateManager.delta;
      }

      if (enemyStates != null)
      {
        if (enemyStates.canMove)
          return;

        delta = enemyStates.delta;
      }

      rigidBody.drag = 0;

      if (rootMotionMultiplier == 0)
        rootMotionMultiplier = 1;



      if (!rolling)
      {
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = (deltaPosition * rootMotionMultiplier) / delta;
        rigidBody.velocity = velocity;
      }
      else
      {
        rollTime += delta / 0.6f;
        if (rollTime > 1)
        {
          rollTime = 1;
        }

        if (stateManager == null)
          return;

        float zValue = rollCurve.Evaluate(rollTime);
        Vector3 zVelocity = Vector3.forward * zValue;
        Vector3 relative = transform.TransformDirection(zVelocity);
        Vector3 velocity = (relative * rootMotionMultiplier);
        rigidBody.velocity = velocity;
      }
    }

    public void OpenDamageColliders()
    {
      if (stateManager == null)
        return;
      stateManager.inventoryManager.currentWeapon.weaponHook.OpenDamageColliders();
    }

    public void CloseDamageColliders()
    {
      if (stateManager == null)
        return;
      stateManager.inventoryManager.currentWeapon.weaponHook.CloseDamageColliders();
    }
  }

}
