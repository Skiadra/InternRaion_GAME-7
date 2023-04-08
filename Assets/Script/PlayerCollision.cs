using UnityEngine.EventSystems;
using UnityEngine;
using static Movement;

public class PlayerCollision : MonoBehaviour
{
    public GameObject interactButton;
    public GameObject menu;
    public GameObject menuButton;
    public GameObject firstButton;
    public GameObject[] list;

    bool interact = false;
    bool isInteracting = false;
    void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.tag == "Object")
        {
            interactButton.SetActive(true);
            interact = true;
        }
    }

    void OnTriggerExit2D(Collider2D collide)
    {
        if (collide.tag == "Object")
        {
            interactButton.SetActive(false);
            interact = false;
        }
    }

    void Update()
    {
        if (interact && Input.GetKeyDown(KeyCode.UpArrow) && !isInteracting && move.inControl)
        {
            interactButton.SetActive(false);
            menu.SetActive(true);
            move.inControl = false;
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(firstButton, new BaseEventData(eventSystem));
            isInteracting = true;
        }
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!list[0].activeSelf)
            {
                menu.SetActive(false);
                move.inControl = true;
                isInteracting = false;
                return;
            }
            for (int i = 0; i < list.Length; i++)
            {
                list[i].SetActive(false);
            }
            menuButton.SetActive(true);
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(firstButton, new BaseEventData(eventSystem));
        }
    }
}
