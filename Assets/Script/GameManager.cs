using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using static Movement;
using static SkillTree;
using UnityEditor.SceneManagement;
using System.Drawing;

public class GameManager : MonoBehaviour
{
    int index;
    public int addSkillPoints;
    [SerializeField] private static bool pointAdded = false;
    public static bool loadStat;
    public static bool newStat;
    public GameObject pauseMenu;
    public GameObject firstPauseButton;
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

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0 && pointAdded == false)
            {skillTree.skillPoint += addSkillPoints; pointAdded = true;}
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
