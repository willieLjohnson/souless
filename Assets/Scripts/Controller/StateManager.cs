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
    public float toGround = 0.5f;

    [Header("States")]
    public bool onGround;
    public bool run;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody rigidbody;
    [HideInInspector]
    public float delta;
    [HideInInspector]
    public LayerMask ignoreLayers;


    public void Init()
    {
      SetupAnimator();
      rigidbody = GetComponent<Rigidbody>();
      rigidbody.angularDrag = 999;
      rigidbody.drag = 4;
      rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

      gameObject.layer = 8;
      ignoreLayers = ~(1 << 9);
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

    public void Tick(float delta)
    {
      this.delta = delta;
      onGround = OnGround();
    }

    void HandleMovementAnimations()
    {
      animator.SetFloat("vertical", moveAmount, 0.4f, delta);
    }

    public bool OnGround()
    {
      bool rayHit = false;

      Vector3 origin = transform.position + (Vector3.up * toGround);
      Vector3 direction = -Vector3.up;
      float distance = toGround + 0.3f;
      RaycastHit hit;
      if (Physics.Raycast(origin, direction, out hit, distance, ignoreLayers))
      {
        rayHit = true;
        Vector3 targetPosition = hit.point;
        transform.position = targetPosition;
      }

      return rayHit;
    }
  }
}