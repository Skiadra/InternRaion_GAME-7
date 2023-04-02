using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject door;
    public GameObject panel;
    public Sprite leverOnSprite;

    private bool isOn = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOn)
        {
            if (collision.CompareTag("Player"))
            {
                panel.SetActive(true);
            }

            isOn = true;
            spriteRenderer.sprite = leverOnSprite;
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