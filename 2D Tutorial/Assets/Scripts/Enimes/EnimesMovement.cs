using UnityEngine;

public class EnimesMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private float direction = -1f;
    private Vector3 localState;
    private bool faceRight = false;
    private int hitPoints = 100;
    public int health = 100;
    public HealthManagerEnemyScript healthManager;

    void Start(){
        hitPoints = 100;
        healthManager.SetHealth(hitPoints, health);
    }

    public void TakeDamage(int damage){
        hitPoints -= damage;
        healthManager.SetHealth(hitPoints, health);
        if (hitPoints <= 0){
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        localState = transform.localScale;
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(direction * speed, body.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("FireBall")){
            direction = -direction;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            HealthManagerPlayerScript playerHealthManager = col.gameObject.GetComponentInChildren<HealthManagerPlayerScript>();
            playerHealthManager.TakeDamage(30);
        }
    }

    void LateUpdate(){
        if (direction > 0){
            faceRight = false;
        } else if (direction < 0){
            faceRight = true;
        }

        if ((faceRight && localState.x < 0) || (!faceRight && localState.x > 0)){
            localState.x *= -1;
        }
        
        transform.localScale = localState;
    }
    
}