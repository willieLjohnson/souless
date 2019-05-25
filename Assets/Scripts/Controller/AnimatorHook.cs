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

    public void Init(StateManager stateManager)
    {
      this.stateManager = stateManager;
      animator = stateManager.animator;
    }

    void OnAnimatorMove()
    {
      if (stateManager.canMove)
        return;

      stateManager.rigidBody.drag = 0;

      if (rootMotionMultiplier == 0)
        rootMotionMultiplier = 1;

      Vector3 delta = animator.deltaPosition;
      delta.y = 0;
      Vector3 velocity = (delta * rootMotionMultiplier) / stateManager.delta;
      stateManager.rigidBody.velocity = velocity;
    }
  }

}
