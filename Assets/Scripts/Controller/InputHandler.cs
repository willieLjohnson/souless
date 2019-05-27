using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class InputHandler : MonoBehaviour
  {
    float vertical;
    float horizontal;
    bool attack1Input;
    bool attack2Input;
    bool twoHandedInput;
    bool runInput;
    bool aInput;
    bool useItemInput;

    bool qInput;
    bool lockOnInput;

    float runTimer;
    float rtTimer;
    float ltTimer;

    StateManager stateManager;
    CameraManager cameraManager;

    float delta;

    // Start is called before the first frame update
    void Start()
    {
      stateManager = GetComponent<StateManager>();
      stateManager.Init();

      cameraManager = CameraManager.singleton;
      cameraManager.Init(stateManager);
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
      ResetInput();
    }

    void GetInput()
    {
      vertical = Input.GetAxis("Vertical");
      horizontal = Input.GetAxis("Horizontal");
      runInput = Input.GetButton("Run");

      attack1Input = Input.GetButton("Attack1");
      attack2Input = Input.GetButton("Attack2");
      twoHandedInput = Input.GetButtonUp("TwoHanded");
      lockOnInput = Input.GetButtonUp("LockOn");

      qInput = Input.GetButton("Action2");
      aInput = Input.GetButton("Action1");
      useItemInput = Input.GetButton("UseItem");

      if (runInput)
        runTimer += delta;
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

      if (useItemInput)
        runInput = false;

      if (runInput && runTimer > 0.5f)
      {
        stateManager.run = (stateManager.moveAmount > 0);
      }

      if (!runInput && runTimer > 0 && runTimer < 0.5f)
        stateManager.roll = true;

      stateManager.useItem = useItemInput;
      stateManager.attack1 = attack1Input;
      stateManager.attack2 = attack2Input;
      stateManager.action1 = qInput;
      stateManager.action2 = aInput;

      if (twoHandedInput)
      {
        stateManager.isTwoHanded = !stateManager.isTwoHanded;
        stateManager.HandleTwoHanded();
      }

      if (lockOnInput)
      {
        stateManager.lockOn = !stateManager.lockOn;
        if (!stateManager.lockOnTarget)
          stateManager.lockOn = false;

        cameraManager.lockOnTarget = stateManager.lockOnTarget;
        stateManager.lockOnTransform = cameraManager.lockOnTransform;
        cameraManager.lockOn = stateManager.lockOn;

      }
    }

    void ResetInput()
    {
      if (!runInput)
        runTimer = 0;

      if (stateManager.roll)
        stateManager.roll = false;

      if (stateManager.run)
        stateManager.run = false;
    }
  }

}