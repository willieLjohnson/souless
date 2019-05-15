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

    // Should animation play?
    public bool playAnimation;

    // Names of oh attack animations.
    public string[] oh_attacks;
    // Names of th attack animations.
    public string[] th_attacks;

    // Is the player in to handed mode.
    public bool twoHanded;
    // Bool to toggle root motion on player.
    public bool enableRootMotion;
    public bool useItem;
    public bool interacting;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
      animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

      enableRootMotion = !animator.GetBool("can_move");
      animator.applyRootMotion = enableRootMotion;

      interacting = animator.GetBool("interacting");

      if (enableRootMotion) return;

      if (useItem)
      {
        animator.Play("use_item");
        useItem = false;
      }

      if (interacting)
      {
        playAnimation = false;
        vertical = Mathf.Clamp(vertical, 0, 0.5f);
      }

      animator.SetBool("two_handed", twoHanded);

      if (playAnimation)
      {
        string targetAnimation;

        if (!twoHanded)
        {
          int r = Random.Range(0, oh_attacks.Length);
          targetAnimation = oh_attacks[r];

          // if (vertical > 0.5f)
          //   targetAnimation = "oh_attack_3";
        }
        else
        {
          int r = Random.Range(0, th_attacks.Length);
          targetAnimation = th_attacks[r];
        }

        if (vertical > 0.5f)
          targetAnimation = "oh_attack_3";

        vertical = 0;

        animator.CrossFade(targetAnimation, 0.2f);
        // animator.SetBool("can_move", false);
        enableRootMotion = true;
        playAnimation = false;
      }
      animator.SetFloat("vertical", vertical);

    }
  }
}

