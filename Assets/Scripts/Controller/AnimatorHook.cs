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
    }
  }

}
