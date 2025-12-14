using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Transform healthFillRect; // Assign this in the Inspector

    public int maxHealth = 100;
    public int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void PoopNei(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        updateHealthBar();

        // update health bar UI
   }

   public void heal()
   {
        currentHealth = maxHealth;
        updateHealthBar();
   }

   void updateHealthBar(){
        float fraction = Mathf.Clamp01((float)currentHealth / (float)maxHealth) * 0.68f;
        Vector3 newScale = healthFillRect.localScale;
        newScale.x = fraction;
        healthFillRect.localScale = newScale;
 
   }

    void Die()
    {
        Debug.Log("Player has died!");
        // Add death handling logic here (e.g., respawn, game over screen, etc.)
    }
}
