using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private Slider hpBar;

    private bool isHealing = false;

    // Starting position of the player
    private Vector3 startingPosition;

    // Y position threshold for falling off the cliff
    private float fallThreshold = -50f;

    private void Start() {
        currentHealth = maxHealth;
        if (hpBar != null)
        {
            hpBar.maxValue = maxHealth;
            hpBar.value = currentHealth;
        }

        // Store the starting position of the player
        startingPosition = transform.position;
    }

    public void TakeDamage(int damage)
    {
        if (!isHealing) // check if player is currently being healed
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }

            if (hpBar != null)
            {
                hpBar.value = currentHealth;
            }
        }
    }

    public void receiveHeal(int heal){
        if (currentHealth < maxHealth) // check if player is not already at full health
        {
            currentHealth += heal;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            if (hpBar != null)
            {
                hpBar.value = currentHealth;
            }

            StartCoroutine(HealingEffect()); // start healing effect
        }
    }

    private IEnumerator HealingEffect()
    {
        isHealing = true;
        yield return new WaitForSeconds(1f); // adjust duration of healing effect as needed
        isHealing = false;
    }

    private void Die()
    {
        // Reset the player's position to the starting position
        transform.position = startingPosition;

        // Restart the game here
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        // Check if the player has fallen off the cliff
        if (transform.position.y < fallThreshold)
        {
            Die();
        }
    }
}
