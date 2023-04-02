using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToScene : MonoBehaviour
{
    [SerializeField] private string newLevel;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")){
            SceneManager.LoadScene(newLevel);
        }
    }
}
