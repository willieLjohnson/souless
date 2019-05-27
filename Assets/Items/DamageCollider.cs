using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class DamageCollider : MonoBehaviour
  {
    void OnTriggerEnter(Collider other)
    {
      EnemyStates enemyStates = other.transform.GetComponentInParent<EnemyStates>();

      if (!enemyStates)
        return;

      enemyStates.DoDamage(35);
    }
  }
}
