using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private float force;
    private Vector3 direction;
    private Rigidbody2D rb;
    [SerializeField] private int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == Vector3.zero) return;
        rb.linearVelocity = direction * force;
    }

    public void SetDirection(Vector3 direction, float force)
    {
        this.direction = direction;
        this.force = force;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
            Destroy(gameObject);
        }
    }
}
