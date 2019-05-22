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
    public bool fire1, r, q, z;
    public Vector3 moveDirection;

    [Header("Stats")]
    public float moveSpeed = 3.5f;
    public float runSpeed = 5.5f;
    public float rotateSpeed = 9;
    public float toGround = 0.5f;

    [Header("States")]
    public bool onGround;
    public bool run;
    public bool lockOn;
    public bool inAction;
    public bool canMove;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody rigidBody;
    [HideInInspector]
    public float delta;
    [HideInInspector]
    public LayerMask ignoreLayers;

    float _actionDelay;

    public void Init()
    {
      SetupAnimator();
      rigidBody = GetComponent<Rigidbody>();
      rigidBody.angularDrag = 999;
      rigidBody.drag = 4;
      rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

      gameObject.layer = 8;
      ignoreLayers = ~(1 << 9);

      animator.SetBool("onGround", true);
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

      DetectAction();

      if (inAction)
      {
        animator.applyRootMotion = true;

        _actionDelay += delta;
        if (_actionDelay > 0.3f)
        {
          inAction = false;
          _actionDelay = 0;
        }
        else
        {
          return;
        }
      }

      canMove = animator.GetBool("can_move");

      if (!canMove)
        return;

      animator.applyRootMotion = false;

      rigidBody.drag = (moveAmount > 0 || !onGround) ? 0 : 4;

      float targetSpeed = moveSpeed;
      if (run)
        targetSpeed = runSpeed;

      if (onGround)
        rigidBody.velocity = moveDirection * (targetSpeed * moveAmount);

      if (run)
        lockOn = false;

      if (!lockOn)
      {
        Vector3 targetDirection = moveDirection;
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
          targetDirection = transform.forward;

        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rotation, delta * moveAmount * rotateSpeed);
        transform.rotation = targetRotation;

      }

      HandleMovementAnimations();
    }

    public void DetectAction()
    {
      if (!canMove)
        return;

      if (!fire1 && !r && !q && !z)
        return;

      string targetAnimation = null;

      if (fire1)
        targetAnimation = "oh_attack_1";
      if (r)
        targetAnimation = "oh_attack_2";
      if (q)
        targetAnimation = "oh_attack_3";
      if (z)
        targetAnimation = "th_attack_1";

      if (string.IsNullOrEmpty(targetAnimation))
        return;

      canMove = false;
      inAction = true;
      animator.CrossFade(targetAnimation, 0.2f);
    }
    public void Tick(float delta)
    {
      this.delta = delta;
      onGround = OnGround();
      animator.SetBool("onGround", onGround);
    }

    void HandleMovementAnimations()
    {
      animator.SetBool("run", run);
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