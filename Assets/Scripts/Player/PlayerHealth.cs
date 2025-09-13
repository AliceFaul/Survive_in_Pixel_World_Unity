using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        HealthUI.Instance.SetMaxHeart(maxHealth);
    }

    //If health = 1, player current health += 1
    //If health = -1, player current health += -1 <=> current health - 1
    public void ChangeHealth(int health)
    {
        AudioManager.Instance.PlaySFX("Hit");
        currentHealth += health;
        HealthUI.Instance.UpdateHeart(currentHealth);

        if(currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
            AudioManager.Instance.PlaySFX("Dead");
            gameObject.SetActive(false);
        }
    }
}
