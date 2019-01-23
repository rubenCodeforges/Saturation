using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 3f;
    public GameObject groundCheckPoint;
    public LayerMask groundLayer;
    public float groundCheckRadius;

    [HideInInspector] public float initialMass;

    public VirtualJoystick Joystick;

    private float moveHorizontal = 0f;
    private bool isGrounded;
    private bool hasControl = true;
    private Rigidbody2D rb;
    private bool jump = false;

    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public void setControl(bool enabled)
    {
        hasControl = enabled;
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        initialMass = rb.mass;
    }

    private void Update()
    {
        if (hasControl)
        {
            listenForInput();
        }
    }

    void FixedUpdate()
    {
        isGrounded = false;
        GroundCheck();


        if (hasControl)
        {
            rb.velocity = new Vector2(rb.velocity.x + moveHorizontal * speed * Time.deltaTime, rb.velocity.y);
            
        }
    }

    public void OnJumpButton()
    {
        jump = true;
    }
    
    private void listenForInput()
    {
        if (Input.GetAxis("Horizontal") != 0f)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
        }
        else if (Joystick.inputDirection != Vector3.zero)
        {
            moveHorizontal = Joystick.inputDirection.x;
            Debug.Log(Joystick.inputDirection.x);

        }

        if (Input.GetButtonDown("Jump") && isGrounded || jump && isGrounded )
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            jump = false;
        }
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