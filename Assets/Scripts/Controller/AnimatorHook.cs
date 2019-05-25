using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class AnimatorHook : MonoBehaviour
  {
    Animator animator;
    StateManager stateManager;
    public float rootMotionMultiplier;
    bool rolling;
    float rollTime; // TODO: Rename
    AnimationCurve rollCurve;

    public void Init(StateManager stateManager)
    {
      this.stateManager = stateManager;
      animator = stateManager.animator;
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
      if (stateManager.canMove)
        return;

      stateManager.rigidBody.drag = 0;

      if (rootMotionMultiplier == 0)
        rootMotionMultiplier = 1;

      if (!rolling)
      {
        Vector3 delta = animator.deltaPosition;
        delta.y = 0;
        Vector3 velocity = (delta * rootMotionMultiplier) / stateManager.delta;
        stateManager.rigidBody.velocity = velocity;
      }
      else
      {
        rollTime += stateManager.delta / 0.6f;
        if (rollTime > 1)
        {
          rollTime = 1;
        }
        float zValue = stateManager.rollCurve.Evaluate(rollTime);
        Vector3 zVelocity = Vector3.forward * zValue;
        Vector3 relative = transform.TransformDirection(zVelocity);
        Vector3 velocity = (relative * rootMotionMultiplier);
        stateManager.rigidBody.velocity = velocity;
      }
    }
  }

}
