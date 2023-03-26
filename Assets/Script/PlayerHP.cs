using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HPBar hpBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hpBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        hpBar.setHealth(currentHealth);
    }
}
