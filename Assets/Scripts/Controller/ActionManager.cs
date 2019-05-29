using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
  public class ActionManager : MonoBehaviour
  {
    public List<Action> actionSlots = new List<Action>();

    public ItemAction consumableItem;

    StateManager stateManager;

    public void Init(StateManager stateManager)
    {
      this.stateManager = stateManager;

      UpdateActionsOneHanded();
    }

    public void UpdateActionsOneHanded()
    {
      EmptyAllSlots();
      Weapon weapon = stateManager.inventoryManager.rightHandWeapon;

      for (int i = 0; i < weapon.actions.Count; i++)
      {
        Action action = GetAction(weapon.actions[i].input);
        action.targetAnimation = weapon.actions[i].targetAnimation;
      }
    }

    public void UpdateActionsTwoHanded()
    {
      EmptyAllSlots();
      Weapon weapon = stateManager.inventoryManager.rightHandWeapon;

      for (int i = 0; i < weapon.twoHandedActions.Count; i++)
      {
        Action action = GetAction(weapon.twoHandedActions[i].input);
        action.targetAnimation = weapon.twoHandedActions[i].targetAnimation;
      }
    }

    void EmptyAllSlots()
    {
      for (int i = 0; i < 4; i++)
      {
        Action action = GetAction((ActionInput)i);
        action.targetAnimation = null;
      }
    }

    ActionManager()
    {
      for (int i = 0; i < 4; i++)
      {
        Action action = new Action();
        action.input = (ActionInput)i;
        actionSlots.Add(action);
      }
    }

    public Action GetActionSlot(StateManager stateManager)
    {
      ActionInput actionInput = GetActionInput(stateManager);
      return GetAction(actionInput);
    }

    Action GetAction(ActionInput input)
    {
      for (int i = 0; i < actionSlots.Count; i++)
      {
        if (actionSlots[i].input == input)
          return actionSlots[i];
      }

      return null;
    }

    public ActionInput GetActionInput(StateManager stateManager)
    {
      if (stateManager.attack1)
        return ActionInput.attack1;
      if (stateManager.attack2)
        return ActionInput.attack2;
      if (stateManager.action1)
        return ActionInput.action1;
      if (stateManager.action2)
        return ActionInput.action2;

      return ActionInput.attack1;
    }
  }

  public enum ActionInput
  {
    attack1, attack2, action1, action2
  }

  [System.Serializable]
  public class Action
  {
    public ActionInput input;
    public string targetAnimation;
  }

  [System.Serializable]
  public class ItemAction
  {
    public string targetAnimation;
    public string itemID;
  }
}
