using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using static Movement;
using static SkillTree;

public class GameManager : MonoBehaviour
{
    int index;
    public int addSkillPoints;
    public static bool loadStat;
    public static bool newStat;
    public GameObject pauseMenu;
    public GameObject firstPauseButton;
    public void loadLevel()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        index = data.activeSceneIndex;
        loadStat = true;
        SceneManager.LoadSceneAsync(index);
        Time.timeScale = 1f;
    }

    public void NewStart()
    {
        newStat = true;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1);
        Time.timeScale = 1f;
    }

    void Start()
    {
        skillTree.skillPoint += addSkillPoints;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 0f;
            move.inControl = false;
            pauseMenu.SetActive(true);
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(firstPauseButton, new BaseEventData(eventSystem));
        }
        Debug.Log(skillTree.skillPoint);
    }
}
