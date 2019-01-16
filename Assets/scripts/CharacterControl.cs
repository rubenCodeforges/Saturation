using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 3f;
    public Rigidbody2D rb;
    public float maxVelocity = 500f;
    public GameObject groundCheckPoint;
    public LayerMask groundLayer;
    public float groundCheckRadius;

    [HideInInspector]
    public float initialMass;
    
    private float moveHorizontal = 0f;
    private bool isGrounded;

    private void Start()
    {
        initialMass = rb.mass;
    }

    private void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        isGrounded = false;
        GroundCheck();
        rb.velocity = new Vector2(rb.velocity.x + moveHorizontal * speed * Time.deltaTime, rb.velocity.y);
    }

    public bool getIsGrounded()
    {
        return isGrounded;
    }

    private void GroundCheck()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(groundCheckPoint.transform.position, groundCheckRadius, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                isGrounded = true;
        }
    }
}