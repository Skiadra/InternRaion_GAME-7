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

    private void Start() {
        currentHealth = maxHealth;
        if (hpBar != null)
        {
            hpBar.maxValue = maxHealth;
            hpBar.value = currentHealth;
        }
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
        // Restart the game here
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
