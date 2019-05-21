using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
  float vertical;
  float horizontal;


  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void FixedUpdate()
  {
    GetInput();
  }

  void GetInput()
  {
    vertical = Input.GetAxis("Vertical");
    Horizontal = Input.GetAxis("Horizontal");
  }
}
