using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    [Header("Components")]
    public float speed;
    public float jumpForce;
    private float moveInput;

    [Header("Layar Mask")]
    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    [Header("Jump")]
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    [Header("fall physics")]
    public float fallMultiplier;
    public float lowJumpMultiplier;

    private int score = 0;


    private void Awake()
    {
        GameObject collider = GameObject.Find("EagelFlyCollider");     
        Physics2D.IgnoreCollision(collider.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        GameObject collider1 = GameObject.Find("EaglePostionCollider");     
        Physics2D.IgnoreCollision(collider1.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        GameObject collider2 = GameObject.Find("EaglePostionCollider1");     
        Physics2D.IgnoreCollision(collider2.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate(){
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        
        anim.SetBool("run", horizontalInput != 0);
        // anim.SetTrigger("die");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            Jump();
    }

    private void Jump()
    {

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        anim.SetTrigger("jump");
        
        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //cool jump fall
        if (body.velocity.y < 0)
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (body.velocity.y > 0 && Input.GetKey(KeyCode.Space))
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

       //fixed double jump bug
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        //lets player jump
        if (isGrounded == true && Input.GetKeyDown("space") && isJumping == false)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            body.velocity = Vector2.up * jumpForce;
        }

        //makes you jump higher when you hold down space
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                body.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;

            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Trap")
        {
            jumpTimeCounter = jumpTime;
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.tag == "Gem"){
            score += 10;
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "Cherry"){
            GameObject helathManager = GameObject.Find("HealthManagerPlayer");     
            HealthManagerPlayerScript healthManager = helathManager.GetComponent<HealthManagerPlayerScript>();
            healthManager.Heal(100);
            Destroy(other.gameObject);
        }
    
    }

    // private bool isGrounded()
    // {
    //     RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
    //     return raycastHit.collider != null;
    // }
}