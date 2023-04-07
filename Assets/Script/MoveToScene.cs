using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Movement;

public class MoveToScene : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadScene()
    {
        move.saveData();
        SceneManager.LoadScene(sceneToLoad);
    }
}