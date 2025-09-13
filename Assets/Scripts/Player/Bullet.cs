using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private int damage = 2;
    [SerializeField] private float timeDestroy = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Destroy(gameObject, timeDestroy);
    }

    private void OnEnable()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if(player != null && sr != null)
        {
            Debug.Log("Find Player");
            SpriteRenderer playerSr = player.GetComponent<SpriteRenderer>();
            if(playerSr != null)
            {
                sr.sortingLayerName = playerSr.sortingLayerName;
                gameObject.layer = playerSr.gameObject.layer;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Enemy"))
        {
            AudioManager.Instance.PlaySFX("EnemyHit");
            collision.gameObject.GetComponent<EnemyHealth>().ChangeHealth(-damage);
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Boss"))
        {
            AudioManager.Instance.PlaySFX("EnemyHit");
            collision.gameObject.GetComponent<BossEnemy>().ChangeHealth(-damage);
            Destroy(gameObject);
        }
    }
}
