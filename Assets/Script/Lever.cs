using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject door;
    public GameObject panel;

    private bool isOn = false;
    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake() {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOn)
        {
            if (collision.CompareTag("Player"))
            {
                anim.SetTrigger("open");
                panel.SetActive(true);
            }

            isOn = true;
            door.GetComponent<Door>().OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            panel.SetActive(false);
        }
    }
}