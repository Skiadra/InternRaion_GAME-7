using UnityEngine;
using UnityEngine.SceneManagement;
using static Movement;

public class PauseMenu : MonoBehaviour
{
    public void continueGame()
    {
        Time.timeScale = 1f;
        move.inControl = true;
        gameObject.SetActive(false);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
