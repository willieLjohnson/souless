using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class CameraManager : MonoBehaviour
  {
    public bool lockOn;
    public float followSpeed = 3;
    public float mouseSpeed = 2;

    public Transform target;
    public EnemyTarget lockOnTarget;
    public Transform lockOnTransform;

    [HideInInspector]
    public Transform pivot;
    [HideInInspector]
    public Transform cameraTransform;
    StateManager stateManager;

    float turnSmoothing = .1f;
    public float minAngle = -35f;
    public float maxAngle = 35f;

    float smoothY;
    float smoothX;

    float smoothYVelocity;
    float smoothXVelocity;

    public float lookAngle;
    public float tiltAngle;

    bool moved;

    public void Init(StateManager stateManager)
    {
      this.stateManager = stateManager;
      target = stateManager.transform;

      cameraTransform = Camera.main.transform;
      pivot = cameraTransform.parent;
    }

    public void Tick(float delta)
    {
      float vertical = Input.GetAxis("Mouse Y");
      float horizontal = Input.GetAxis("Mouse X");

      float targetSpeed = mouseSpeed;

      if (lockOnTarget)
      {
        if (!lockOnTransform)
        {
          lockOnTransform = lockOnTarget.GetTarget();
          stateManager.lockOnTransform = lockOnTransform;
        }

        if (Mathf.Abs(horizontal) > 0.6f)
        {
          if (!moved)
          {
            lockOnTransform = lockOnTarget.GetTarget((horizontal > 0));
            stateManager.lockOnTransform = lockOnTransform;
            moved = true;
          }
        }
      }

      if (moved)
      {
        if (Mathf.Abs(horizontal) < 0.6f)
        {
          moved = false;
        }
      }

      FollowTarget(delta);
      HandleRotations(delta, vertical, horizontal, targetSpeed);
    }

    void FollowTarget(float delta)
    {
      float speed = delta * followSpeed;
      Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, delta);
      transform.position = targetPosition;
    }

    void HandleRotations(float delta, float vertical, float horizontal, float targetSpeed)
    {
      if (turnSmoothing > 0)
      {
        smoothY = Mathf.SmoothDamp(smoothY, vertical, ref smoothYVelocity, turnSmoothing);
        smoothX = Mathf.SmoothDamp(smoothX, horizontal, ref smoothXVelocity, turnSmoothing);
      }

      else
      {
        smoothY = vertical;
        smoothX = horizontal;
      }

      tiltAngle -= smoothY * targetSpeed;
      tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
      pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

      if (lockOn && lockOnTarget)
      {
        Vector3 targetDirection = lockOnTransform.position - transform.position;
        targetDirection.Normalize();
        // targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
          targetDirection = transform.forward;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, delta * 9);
        lookAngle = transform.eulerAngles.y;

        return;
      }

      lookAngle += smoothX * targetSpeed;
      transform.rotation = Quaternion.Euler(0, lookAngle, 0);

    }

    public static CameraManager singleton;
    void Awake()
    {
      singleton = this;
    }
  }

}
