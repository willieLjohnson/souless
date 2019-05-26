﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class EnemyTarget : MonoBehaviour
  {
    public int index;
    public List<Transform> targets = new List<Transform>();
    public List<HumanBodyBones> humanoidBones = new List<HumanBodyBones>();

    Animator animator;
    
    void Start()
    {
      animator = GetComponent<Animator>();
      if (!animator.isHuman)
        return;

      for (int i = 0; i < humanoidBones.Count; i++)
      {
        targets.Add(animator.GetBoneTransform(humanoidBones[i]));
      }
    }

    public Transform GetTarget()
    {
      if (targets.Count == 0)
        return transform;

      int targetIndex = index;
      if (index < targets.Count - 1)
      {
        index++;
      }
      else
      {
        index = 0;
        targetIndex = 0;
      }

      return targets[targetIndex];
    }
  }
}