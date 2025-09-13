using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int damage)
    {
        currentHealth += damage;
        gameObject.GetComponent<Animator>().SetTrigger("isHit");

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if(currentHealth <= 0)
        {
            GameManager.Instance.Level2Progress(1);
            GameManager.Instance.AddEnemyScore();
            Destroy(gameObject);
        }
    }
}
