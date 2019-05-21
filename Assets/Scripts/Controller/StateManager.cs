using UnityEngine;

namespace SA
{
  public class StateManager : MonoBehaviour
  {
    [Header("Init")]
    public GameObject activeModel;

    [Header("Inputs")]
    public float vertical;
    public float horizontal;
    public float moveAmount;
    public Vector3 moveDirection;

    [Header("Stats")]
    public float moveSpeed = 2;
    public float runSpeed = 3.5f;

    [Header("States")]
    public bool run;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody rigidbody;
    [HideInInspector]
    public float delta;


    public void Init()
    {
      SetupAnimator();
      rigidbody = GetComponent<Rigidbody>();
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
    public void FixedTick(float delta)
    {
      this.delta = delta;

      float targetSpeed = moveSpeed;
      if (run)
        targetSpeed = runSpeed;

      rigidbody.velocity = moveDirection * targetSpeed;
    }
  }
}