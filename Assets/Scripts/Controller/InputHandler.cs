﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class InputHandler : MonoBehaviour
  {
    float vertical;
    float horizontal;

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
    }

    void Update()
    {
      delta = Time.deltaTime;
      cameraManager.Tick(delta);
    }

    void GetInput()
    {
      vertical = Input.GetAxis("Vertical");
      horizontal = Input.GetAxis("Horizontal");
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
    }
  }

}