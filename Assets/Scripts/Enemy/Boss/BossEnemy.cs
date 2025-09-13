using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    [Header("Movement")]
    private Rigidbody2D rb;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Transform player;

    [Space]

    [Header("Health System")]
    [SerializeField] private float maxHealth;
    public float currentHealth;
    [SerializeField] private Image hpBar;

    [Header("Attack System")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedFirstSkill = 15f;
    [SerializeField] private float speedSecondSkill = 10f;
    [SerializeField] private float healThirdSkillValue = 10f;
    [SerializeField] private GameObject miniEnemy;
    private float skillCooldown = 5f;
    private float nextSkillTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        UpdateHPBar();

        if(player == null)
        {
            GameObject Player = GameObject.FindWithTag("Player");
            if(Player != null)
            {
                player = Player.transform;
                Debug.Log("Boss find player");
            }
        }
        else
        {
            Debug.Log("Not have Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextSkillTime)
        {
            UseSkill();
        }

        Moving();
    }

    void Moving()
    {
        if(player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
            Flip();
            Debug.Log("Boss is chasing");
        }
    }

    void Flip()
    {
        if(player != null)
        {
            transform.localScale = new Vector3(player.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }

    public void ChangeHealth(float damage)
    {
        currentHealth += damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHPBar();
        if(currentHealth <= 0)
        {
            GameManager.Instance.AddBossScore();
            Destroy(gameObject);
        }
    }

    void UpdateHPBar()
    {
        if(hpBar != null)
        {
            hpBar.fillAmount = currentHealth / maxHealth;
        }
    }

    #region Boss Skill

    void FirstSkill()
    {
        if(player != null)
        {
            Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            BossBullet enemyBullet = bullet.GetComponent<BossBullet>();
            if(enemyBullet != null)
            {
                AudioManager.Instance.PlaySFX("Range");
                enemyBullet.SetDirection(directionToPlayer, speedFirstSkill);
            }
        }
    }

    void SecondSkill()
    {
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for(int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BossBullet enemyBullet = bullet.GetComponent<BossBullet>();
            if(enemyBullet != null)
            {
                AudioManager.Instance.PlaySFX("Range");
                enemyBullet.SetDirection(bulletDirection, speedSecondSkill);
            }    
        }
    }

    void Heal(float amount)
    {
        ChangeHealth(amount);
    }

    void SpawnMiniEnemy()
    {
        Instantiate(miniEnemy, transform.position, Quaternion.identity);
    }

    void Teleport()
    {
        if(player != null)
        {
            transform.position = player.transform.position;
        }
    }

    void SelectRandomSkill()
    {
        int randomSkill = Random.Range(0, 5);
        switch(randomSkill)
        {
            case 0:
                FirstSkill();
                break;
            case 1:
                SecondSkill();
                break;
            case 2:
                Heal(healThirdSkillValue);
                break;
            case 3:
                SpawnMiniEnemy();
                break;
            case 4:
                Teleport();
                break;
        }            
    }

    void UseSkill()
    {
        nextSkillTime = Time.time + skillCooldown;
        SelectRandomSkill();
    }

    #endregion
}
