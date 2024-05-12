using UnityEngine;
using System.Collections;

public class Eagle : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private float direction = -1f;
    private Vector3 localState;
    private bool faceRight = false;
    Animator m_Animator;
    UnityEngine.AI.NavMeshAgent agent;
    private bool found = false;
    Transform Player;
    Rigidbody2D Player_body;

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
        m_Animator = gameObject.GetComponent<Animator>();
        localState = transform.localScale;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        
    }

    private void FixedUpdate()
    {
        if  (found)
        {
            agent.SetDestination(Player.position);
            if(Vector2.Distance(transform.position, Player.position) < 5)
            { 
                m_Animator.SetBool("Attack 0", true);
            }
          else
           m_Animator.SetBool("Attack 0", false);
        }
        else
        {
         body.velocity = new Vector2(direction * speed, body.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
            {
                HealthManagerPlayerScript playerHealthManager = col.gameObject.GetComponentInChildren<HealthManagerPlayerScript>();
                playerHealthManager.TakeDamage(30);

                agent.isStopped = true;
                
                Player_body = col.gameObject.GetComponent<Rigidbody2D>();
                col.gameObject.GetComponent<PlayerMovement>().enabled = false;

                if (faceRight){
                    body.AddForce(new Vector2(50, 170), ForceMode2D.Impulse);
                    Player_body.AddForce(new Vector2(-20, 0), ForceMode2D.Impulse);
                } else {
                    body.AddForce(new Vector2(-50, 170), ForceMode2D.Impulse);
                    Player_body.AddForce(new Vector2(20, 0), ForceMode2D.Impulse);
                }

                agent.isStopped = false;
                StartCoroutine(StopPlayerFor(0.2f));
                agent.SetDestination(Player.position);
            } else if (col.gameObject.tag == "Wall"){
                direction = -direction;
            }
    }

    private IEnumerator StopPlayerFor(float i){
        yield return new WaitForSeconds (i);
        Player_body.GetComponent<PlayerMovement>().enabled = true;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        
        if (col.gameObject.tag == "Player")
        {
            GameObject collider1 = GameObject.Find("EaglePostionCollider");     
            Physics2D.IgnoreCollision(collider1.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            GameObject collider2 = GameObject.Find("EaglePostionCollider1");     
            Physics2D.IgnoreCollision(collider2.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            Debug.Log($"trigger enter {col.gameObject.tag}");
            Player = col.transform;
            found = true;
          
        }
    }

    void LateUpdate(){
        if (found)
        {
            if (Player.position.x < transform.position.x)
            {
                faceRight = true;
            }
            else
            {
                faceRight = false;
                 
            }

        }

        else
        {
            if (direction > 0){
                faceRight = false;
            } else if (direction < 0){
                faceRight = true;
            }
        }
        
        if ((faceRight && localState.x < 0) || (!faceRight && localState.x > 0)){
            localState.x *= -1;
        }
        
        transform.localScale = localState;
        }
    
    
}