using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class InputHandler : MonoBehaviour
  {
    float vertical;
    float horizontal;
    bool runInput;
    bool aInput;
    bool xInput;
    bool yInput;

    bool fire1Input;
    bool rInput;
    bool qInput;
    float zInput;

    StateManager stateManager;
    CameraManager cameraManager;

    float delta;

    // Start is called before the first frame update
    void Start()
    {
      stateManager = GetComponent<StateManager>();
      stateManager.Init();

      cameraManager = CameraManager.singleton;
      cameraManager.Init(this.transform);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      delta = Time.fixedDeltaTime;
      GetInput();
      UpdateStates();
      stateManager.FixedTick(delta);
      cameraManager.Tick(delta);
    }

    void Update()
    {
      delta = Time.deltaTime;
      stateManager.Tick(delta);
    }

    void GetInput()
    {
      vertical = Input.GetAxis("Vertical");
      horizontal = Input.GetAxis("Horizontal");
      runInput = Input.GetButton("Run");
      rInput = Input.GetButton("R");
      qInput = Input.GetButton("Q");
    }

    void UpdateStates()
    {
      stateManager.vertical = vertical;
      stateManager.horizontal = horizontal;

      Vector3 moveVertical = stateManager.vertical * cameraManager.transform.forward;
      Vector3 moveHorizontal = stateManager.horizontal * cameraManager.transform.right;
      stateManager.moveDirection = (moveVertical + moveHorizontal).normalized;

      float move = Mathf.Abs(vertical) + Mathf.Abs(horizontal);
      stateManager.moveAmount = Mathf.Clamp01(move);

      if (runInput)
      {
        stateManager.run = (stateManager.moveAmount > 0);
      }
      else
      {
        stateManager.run = false;
      }
    }
  }

}