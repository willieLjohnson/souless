using UnityEngine;

namespace SA
{
  public class ActionManager : MonoBehaviour
  {
    public List<Action> actionSlots = new List<Action>();

    public void Init()
    {
      if (actionSlots.Count != 0)
        return;

      for (int i = 0; i < 4; i++)
      {
        Action action = new Action();
        action.input = (ActionInput)i;
        actionSlots.Add(action);
      }
    }
    public ActionInput GetAction(StateManager stateManager)
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
}
