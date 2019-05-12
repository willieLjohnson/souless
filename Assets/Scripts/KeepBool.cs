using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepBool : StateMachineBehaviour
{
  // Name of bool to manage.
  public string boolName;
  // Current status of bool.
  public bool status;
  public bool resetOnExit = true;

  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    animator.SetBool(boolName, status);
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
  {
    if (resetOnExit)
      animator.SetBool(boolName, !status);
  }
}
