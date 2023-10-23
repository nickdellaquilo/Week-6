using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    
    private bool isMoving;
    private Rigidbody2D rb;
    private Tilemap tilemap;
    private Animator animator;
    private Vector2 input;

    private bool isOpen;
    
    public PeacockCooldownBar cooldownBar;
    [SerializeField]private float openCoolDown = 5; //once peacock's feathers have been closed, you have to wait 5(?) seconds before being able to open them again
    [SerializeField]private float openTimer; //feathers can only stay open for 3(?) seconds, initally was set to 0

    public HealthBar healthBar;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int health = 100;

    //public PeacockCooldownBar openFeathersTimeLeft;
    //[SerializeField] private float maxOpenTimeLeft = 10f;

    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private Transform bulletParent;


    float TESTtimer = 0;

    public GameObject bulletPrefab;

    Vector2 curPos;

    private void Start()
    {
        animator = GetComponent<Animator>();
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);
        cooldownBar.SetMaxValue(openCoolDown);
        //openFeathersTimeLeft.SetMaxValue(maxOpenTimeLeft);
    }

    private void Update()
    {
        curPos = new Vector2(transform.position.x, transform.position.y);
        
        if (!isOpen)
        {
            openCoolDown += Time.deltaTime;
        } 
        else
        {
            openTimer += Time.deltaTime;
            
            if (openTimer >= 10)
            {
                isOpen = false;
                animator.SetBool("openFeathers", false);
                openTimer = 0;
                openCoolDown = 0;
            }
        }

        animator.SetBool("isMoving", false);

        if (Input.GetKeyDown(KeyCode.Space) && !isOpen && openCoolDown >= 5)
        {
            isOpen = true;
            animator.SetBool("openFeathers", true);
            //openTimer += Time.deltaTime;
            /*
            if (openTimer < 0)
            {
                isOpen = false;
                animator.SetBool("openFeathers", false);
                openTimer = 0f;
                openCoolDown = 0;
            } */

        } 

        cooldownBar.SetCooldownValue(openCoolDown);

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        
        if(input == Vector2.zero)
        {
            animator.SetFloat("Horizontal", input.x);
            animator.SetFloat("Vertical", input.y);
        }
        if (input.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (input.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        transform.Translate(input * Time.deltaTime * moveSpeed);

        /*
        TESTtimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            health -= 10;
            animator.SetBool("openFeathers", true);
        }
        if (TESTtimer >= 2f)
        {
            animator.SetBool("openFeathers", false);
            TESTtimer = 0;
        } 

        if(!isMoving)
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");

            animator.SetFloat("Horizontal", input.x);
            animator.SetFloat("Vertical", input.y);

            if(input.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            if(input.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            if(input != Vector2.zero)
            {
                animator.SetBool("isMoving", true);
                transform.Translate(input * Time.deltaTime * bulletSpeed);

                //Vector2 targetPos = curPos + input;

                //StartCoroutine(Move(targetPos));
            }
        }*/

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletParent.transform.position, Quaternion.identity);

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 shootDirection = (mousePosition - (Vector2)transform.position).normalized;

            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = bulletSpeed * shootDirection; //bulletParent.transform.position; 
        }

        healthBar.SetHealth(health);

    }

    IEnumerator Move(Vector2 targetPos)
    {
        isMoving = true;

        //animator.SetBool("isMoving", true);

        while((targetPos - curPos).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector2.MoveTowards(curPos, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        curPos = targetPos;
        isMoving = false;

        //animator.SetBool("isMoving", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!isOpen)
            {
                health -= 5; // we can adjust this later
            }
            else
            {
                Destroy(collision.gameObject, 0.5f);
            }
        }
    }
}
