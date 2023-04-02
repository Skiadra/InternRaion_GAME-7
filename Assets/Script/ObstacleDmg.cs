using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class ObstacleDmg : MonoBehaviour
// {
//     [SerializeField] private int damage;

//     private void OnTriggerEnter2D(Collider2D collision) {
//         if (collision.tag == "Player"){
//             collision.GetComponent<PlayerHP>().TakeDamage(damage); 
//         }
//     }
// }

public class ObstacleDmg : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float damageInterval = 1f;

    private bool isPlayerInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"){
            collision.GetComponent<PlayerHP>().TakeDamage(damage); 
        }
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            StartCoroutine(DamagePlayerCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            StopCoroutine(DamagePlayerCoroutine());
        }
    }

    private IEnumerator DamagePlayerCoroutine()
    {
        while (isPlayerInside)
        {
            yield return new WaitForSeconds(damageInterval);
            PlayerHP playerHP = FindObjectOfType<PlayerHP>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(damage);
            }
        }
    }
}
