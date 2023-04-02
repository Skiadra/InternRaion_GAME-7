using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private Slider hpBar;

    private void Start()
    {
        currentHealth = maxHealth;
        if (hpBar != null)
        {
            hpBar.maxValue = maxHealth;
            hpBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
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

    private void Die()
    {
        // Restart the game here
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
