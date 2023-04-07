using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using static Movement;
using static SkillTree;
using VSCodeEditor;

public class GameManager : MonoBehaviour
{
    int index;
    public static bool loadStat;
    public static bool newStat;
    public GameObject pauseMenu;
    public GameObject firstPauseButton;
    public GameObject guide;
    public void loadLevel()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        index = data.activeSceneIndex;
        loadStat = true;
        newStat = false;
        SceneManager.LoadScene(index);
        Time.timeScale = 1f;
    }

    public void NewStart()
    {
        newStat = true;
        loadStat = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        Time.timeScale = 1f;
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void guideMenuOn()
    {
        guide.SetActive(true);
    }

    public void guideMenuOff()
    {
        guide.SetActive(false);
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
        if (SceneManager.GetActiveScene().buildIndex > 0)
            Debug.Log(skillTree.skillPoint);
    }
}
