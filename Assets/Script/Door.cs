using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private bool isOpen = false;
    public string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
    }
}