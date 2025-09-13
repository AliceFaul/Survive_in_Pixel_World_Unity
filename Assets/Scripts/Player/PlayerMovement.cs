using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public float moveSpeed = 3f;
    private Vector2 moveInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        if(Input.GetKey(KeyCode.A))
        {
            animator.SetInteger("Direction", 3);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            animator.SetInteger("Direction", 2);
        }
        else if(Input.GetKey(KeyCode.W))
        {
            animator.SetInteger("Direction", 1);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            animator.SetInteger("Direction", 0);
        }
        animator.SetBool("isWalking", rb.linearVelocity.magnitude > 0);

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.Pause();
        }
    }
}
