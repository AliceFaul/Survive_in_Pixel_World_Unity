using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int score = 10;
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(score);
            GameManager.Instance.Level1Progress(1);
            animator.SetTrigger("Hit");
            Destroy(gameObject, 0.12f);
        }
    }
}
