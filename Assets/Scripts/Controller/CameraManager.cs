using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class CameraManager : MonoBehaviour
  {
    public bool lockon;
    publicfloat followSpeed = 9;
    public float mouseSpeed = 2;

    public Transform target;

    Transform pivot;
    Transform cameraTransform;

    float turnSmoothing = .1f;
    public float minAngle = -35f;
    public float maxAngle = 35f;

    float smoothX;
    float smoothY;
    float smoothXVelocity;
    float smoothYVelocity;
    public float lookAngle;
    public float tiltAngle;

    public void Init(Transform target)
    {
      this.target = target;

      cameraTransform = CameraManager.main.transform;
      pivot = cameraTransform.parent;
    }

    public void Tick(float delta)
    {
      float horizontal = Input.GetAxis("Mouse X");
      float vertical = Input.GetAxis("Mouse Y");

      float targetSpeed = mouseSpeed;

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
        smoothX = Mathf.SmoothDamp(smoothX, vertical, ref smoothXVelocity, turnSmoothing);
        smoothY = Mathf.SmoothDamp(smoothY, horizontal, ref smoothYVelocity, turnSmoothing);
      }

      else
      {
        smoothX = horizontal;
        smoothY = vertical;
      }

      if (lockon)
      {

      }

      lookAngle += smoothX * targetSpeed;
      transform.rotation = Quaternion.Euler(0, lookAngle, 0);

      tiltAngle -= smoothY * targetSpeed;
      tiltAngle = Mathf.Clam(tiltAngle, minAngle, maxAngle);
      pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
    }

    public static CameraManager singleton;
    void Awake()
    {
      singleton = this;
    }
  }

}
