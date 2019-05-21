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
    public float rotateSpeed = 3;

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
      rigidbody.angularDrag = 999;
      rigidbody.drag = 4;
      rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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

      rigidbody.drag = (moveAmount > 0) ? 0 : 4;

      float targetSpeed = moveSpeed;
      if (run)
        targetSpeed = runSpeed;

      rigidbody.velocity = moveDirection * (targetSpeed * moveAmount);

      Vector3 targetDirection = moveDirection;
      targetDirection.y = 0;

      if (targetDirection == Vector3.zero)
        targetDirection = transform.forward;

      Quaternion rotation = Quaternion.LookRotation(targetDirection);
      Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rotation, delta * moveAmount * rotateSpeed);
      transform.rotation = targetRotation;

      HandleMovementAnimations();
    }

    void HandleMovementAnimations()
    {
      animator.SetFloat("vertical", moveAmount, 0.4f, delta);
    }
  }
}