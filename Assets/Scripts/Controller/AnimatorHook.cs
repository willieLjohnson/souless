using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class AnimatorHook : MonoBehaviour
  {
    Animator animator;
    StateManager stateManager;
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
      float multiplier = 1;
      Vector3 delta = animator.deltaPosition;
      delta.y = 0;
      Vector3 velocity = (delta * multiplier) / stateManager.delta;
      stateManager.rigidBody.velocity = velocity;
    }
  }

}
