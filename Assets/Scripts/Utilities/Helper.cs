using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class Helper : MonoBehaviour
  {
    [Range(0, 1)]
    public float vertical;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
      animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      animator.SetFloat("vertical", vertical);
      print(animator.GetFloat("vertical"));
    }
  }
}

