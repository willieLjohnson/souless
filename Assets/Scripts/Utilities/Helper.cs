using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class Helper : MonoBehaviour
  {
    [Range(0, 1)]
    // Vertical speed of player.
    public float vertical;

    // Name of animation currently playing.
    public string animationName;
    // Should animation play?
    public bool playAnimation;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
      animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      if (playAnimation)
      {
        vertical = 0;
        animator.CrossFade(animationName, 0.2f);
        playAnimation = false;
      }
      animator.SetFloat("vertical", vertical);
    }
  }
}

