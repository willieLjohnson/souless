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
    public bool attack1, attack2, q, a;
    public Vector3 moveDirection;
    public bool roll;

    [Header("Stats")]
    public float moveSpeed = 3.5f;
    public float runSpeed = 5.5f;
    public float rotateSpeed = 9f;
    public float toGround = 0.5f;
    public float rollSpeed = 1f;

    [Header("States")]
    public bool onGround;
    public bool run;
    public bool lockOn;
    public bool inAction;
    public bool canMove;
    public bool isTwoHanded;


    [Header("Other")]
    public EnemyTarget lockOnTarget;
    public AnimationCurve rollCurve;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody rigidBody;
    [HideInInspector]
    public AnimatorHook animatorHook;


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

      animatorHook = activeModel.AddComponent<AnimatorHook>();
      animatorHook.Init(this);

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

      // animatorHook.rootMotionMultiplier = 1;
      animatorHook.CloseRoll();
      HandleRolls();

      animator.applyRootMotion = false;

      rigidBody.drag = (moveAmount > 0 || !onGround) ? 0 : 4;

      float targetSpeed = moveSpeed;
      if (run)
        targetSpeed = runSpeed;

      if (onGround)
        rigidBody.velocity = moveDirection * (targetSpeed * moveAmount);

      if (run)
        lockOn = false;

      Vector3 targetDirection = (!lockOn) ? moveDirection : lockOnTarget.transform.position - transform.position;
      targetDirection.y = 0;

      if (targetDirection == Vector3.zero)
        targetDirection = transform.forward;

      Quaternion rotation = Quaternion.LookRotation(targetDirection);
      Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rotation, delta * moveAmount * rotateSpeed);
      transform.rotation = targetRotation;

      animator.SetBool("lockon", lockOn);

      if (!lockOn)
        HandleMovementAnimations();
      else
        HandleLockOnAnimations(moveDirection);
    }

    public void DetectAction()
    {
      if (!canMove)
        return;

      if (!attack1 && !attack2 && !q && !a)
        return;

      string targetAnimation = null;

      if (attack1)
        targetAnimation = "oh_attack_1";
      if (attack2)
        targetAnimation = "oh_attack_2";
      if (q)
        targetAnimation = "oh_attack_3";
      if (a)
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

    void HandleRolls()
    {
      if (!roll)
        return;

      float rollVertical = vertical;
      float rollHorizontal = horizontal;
      rollVertical = (moveAmount > 0.3f) ? 1 : 0;
      rollHorizontal = 0;

      // if (!lockOn)
      // {
      //   rollVertical = (moveAmount > 0.3f) ? 1 : 0;
      //   rollHorizontal = 0;
      // }
      // else
      // {
      //   if (Mathf.Abs(rollVertical) < 0.3f)
      //     rollVertical = 0;
      //   if (Mathf.Abs(rollHorizontal) < 0.3f)
      //     rollHorizontal = 0;
      // }

      if (rollVertical != 0)
      {
        if (moveDirection == Vector3.zero)
          moveDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = targetRotation;
      }

      animatorHook.rootMotionMultiplier = rollSpeed;

      animator.SetFloat("vertical", rollVertical);
      animator.SetFloat("horizontal", rollHorizontal);

      canMove = false;
      inAction = true;
      animator.CrossFade("Rolls", 0.2f);
      animatorHook.InitForRoll();
    }

    void HandleMovementAnimations()
    {
      animator.SetBool("run", run);
      animator.SetFloat("vertical", moveAmount, 0.4f, delta);
    }

    void HandleLockOnAnimations(Vector3 moveDirection)
    {
      Vector3 relativeDirection = transform.InverseTransformDirection(moveDirection);
      float horizontal = relativeDirection.x;
      float vertical = relativeDirection.z;

      animator.SetFloat("vertical", vertical, 0.2f, delta);
      animator.SetFloat("horizontal", horizontal, 0.2f, delta);
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

    public void HandleTwoHanded()
    {
      animator.SetBool("two_handed", isTwoHanded);
    }
  }
}