﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class InventoryManager : MonoBehaviour
  {

    public Weapon rightHandWeapon;
    public Weapon leftHandWeapon;
    public void Init()
    {
      CloseAllDamageColliders();
    }

    public void CloseAllDamageColliders()
    {
      if (rightHandWeapon.weaponHook)
        rightHandWeapon.weaponHook.CloseDamageColliders();

      if (leftHandWeapon.weaponHook)
        leftHandWeapon.weaponHook.CloseDamageColliders();
    }

    public void OpenAllDamageColliders()
    {
      if (rightHandWeapon.weaponHook)
        rightHandWeapon.weaponHook.OpenDamageColliders();

      if (leftHandWeapon.weaponHook)
        leftHandWeapon.weaponHook.OpenDamageColliders();
    }

  }

  [System.Serializable]
  public class Weapon
  {
    public List<Action> actions;
    public List<Action> twoHandedActions;
    public bool leftHandMirror;
    public GameObject weaponModel;
    public WeaponHook weaponHook;
  }
}

