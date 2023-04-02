using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealDrop : MonoBehaviour
{
    [SerializeField] private int heal = 50;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")){
            collision.GetComponent<PlayerHP>().receiveHeal(heal);
            Destroy(gameObject); // destroy the game object that this script is attached to
        }
    }
}