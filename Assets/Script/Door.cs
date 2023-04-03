using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Movement;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private GameObject panel; // reference to the panel GameObject

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isOpen)
        {
            move.saveData();
            SceneManager.LoadScene(sceneToLoad);
        }
        else if (collision.CompareTag("Player") && !isOpen){
            Debug.Log("You need to activate the lever first!");
            panel.SetActive(true); // display the panel
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            panel.SetActive(false); // hide the panel when the player exits the trigger
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
        panel.SetActive(false); // hide the panel when the door is opened
    }
}