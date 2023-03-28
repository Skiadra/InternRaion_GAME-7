using UnityEngine;
using UnityEngine.EventSystems;
using static SkillTree;

public class Menu : MonoBehaviour
{
    public GameObject skills;
    public GameObject firstSelect;
    public GameObject button;

    public void openSkill()
    {
        skills.SetActive(true);
        skillTree.UpdateSkillUI();
        button.SetActive(false);
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(firstSelect, new BaseEventData(eventSystem));
    }
    public void closeSkill()
    {
        skills.SetActive(false);
        button.SetActive(true);
    }
}
