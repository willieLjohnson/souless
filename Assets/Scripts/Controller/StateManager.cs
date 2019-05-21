using UnityEngine;

namespace SA
{
  public class StateManager : MonoBehaviour
  {
    public float vertical;
    public float horizontal;

    public GameObject activeModel;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody rigidbody;
    [HideInInspector]
    public float delta;


    public void Init()
    {
      SetupAnimator();
      rigidbody = GetComponent<rigidbody>();
    }

    void SetupAnimator()
    {
      if (!activeModel)
      {
        animator = GetComponentInChildren<Animator>();
        if (!animator)
        {
          Debug.Log("No model found");
        }
        else
        {
          activeModel = animator.gameObject;
        }
      }

      if (!animator)
        animator = activeModel.GetComponent<Animator>();

      animator.applyRootMotion = false;
    }
    public void Tick(float delta)
    {
      this.delta = delta;
    }
  }
}