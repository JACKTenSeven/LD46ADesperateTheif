using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    //ye
    static public int level=2;
    static public int getLevel()
    {
        return level;
    }

    //Movement
    [Header("Movement")]
    public float speed = 2f;
    public float fullspeed = 2f;
    public float halfspeed = 1f;
    float horizontalMove;
    float verticalMove;
    public enum Facing { North, East, South, West };
    public Facing facing;
    public bool moving;
    public Rigidbody2D rb;
    public Animator animator;
    public float fireTimer;
    public float fireTime = 1.5f;

    //Stats
    [Header("Stats")]
    static public float health=100;
    private float maxHealth;
    static public float medicineLevel=45f;
    static public int money=0;
   static public int getMoney()
    {
        return money;
    }
    static public int ammo=10;
    static public void addAmmo()
    {
        ammo++;
    }

    //UI
    [Header("UI")]
    public Text clocktimeText;
    public Slider healthBar;
    public Text moneyText;
    public Button nextButton;
    public Text ammoText;

    //Other
    [Header("Other")]
    public bool shooting = false;
    public GameObject bullet;

    void Start()
    {
        maxHealth = health;
    }

    void Update()
    {
        animationManager();

        if(Input.GetAxis("Primary Fire") > 0 && fireTimer<=0 && ammo>0)
        {
            
            fireGun();
            ammo--;
            fireTimer = fireTime;

        }

        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        if(horizontalMove==0 && verticalMove == 0)
        {
            moving = false;
        }
        if (horizontalMove != 0 && verticalMove==0)
        {
            speed = fullspeed;
            moving = true;
            if (horizontalMove > 0)
            {
                facing = Facing.East;
            }
            if (horizontalMove < 0)
            {
                facing = Facing.West;
            }
        }
        if (verticalMove != 0 && horizontalMove==0)
        {
            speed = fullspeed;
            moving = true;
            if (verticalMove > 0)
            {
                facing = Facing.North;
            }
            if (verticalMove < 0)
            {
                facing = Facing.South;
            }
        }
        else if(verticalMove!=0 && horizontalMove != 0)
        {
            speed = halfspeed;

            moving = true;
            if (horizontalMove > 0)
            {
                facing = Facing.East;
            }
            if (horizontalMove < 0)
            {
                facing = Facing.West;
            }
        }


    }

    void FixedUpdate()
    {
        if (medicineLevel > 0)
        {
            if (medicineLevel < 30)
            {
                //change sprite color r0 b0 g255
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            medicineLevel -= 1 * Time.deltaTime;
        }
        else
        {
            //player is dead
            playerDie();
        }

        if (health <= 0)
        {
            //player is dead
            playerDie();
        }

        updateUI();
        if (fireTimer > 0)
        {
            fireTimer -= 1 * Time.deltaTime;
        }
        rb.velocity = new Vector2(horizontalMove * speed, verticalMove * speed);
    }

    private void playerDie()
    {
        print("Dies");
        SceneManager.LoadScene(0);
    }

    private void fireGun()
    {
        print("Fire");
        shooting = true;
        StartCoroutine(gunAnimation());

        GameObject b = Instantiate(bullet);
        b.gameObject.transform.position = this.gameObject.transform.position;
        if(facing == Facing.North)
        {
            b.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            b.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 20);
            
        }
        if (facing == Facing.South)
        {
            b.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            b.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            b.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -20);
            
        }
        if (facing == Facing.East)
        {
            b.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);
            
        }
        if (facing == Facing.West)
        {
            b.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            b.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-20, 0);
            
        }

        shooting = false;
    }
    private IEnumerator gunAnimation()
    {
        animator.SetBool("Attacking", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Attacking", false);
        

    }
    private void animationManager()
    {

        animator.SetBool("Moving", moving);
        if(facing == Facing.North)
        {
            animator.SetInteger("Facing", 0);
        }
        if (facing == Facing.East)
        {
            animator.SetInteger("Facing", 1);
        }
        if (facing == Facing.South)
        {
            animator.SetInteger("Facing", 2);
        }
        if (facing == Facing.West)
        {
            animator.SetInteger("Facing", 3);
        }
    }

    private void updateUI()
    {
        moneyText.text = "$" + money;
        healthBar.value = health / maxHealth;
        //clock
        //find minutes
        int minutes = Mathf.FloorToInt(medicineLevel / 60);
        int seconds = Mathf.FloorToInt(medicineLevel % 60);
        clocktimeText.text = minutes + ":" + seconds;

        ammoText.text = "" + ammo;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Med")
        {
            //we picked up a med
           
            //increase the med level and delete the med
            GameObject.Destroy(col.gameObject);
            medicineLevel += 30f;
            //add a partical effect here
        }

        if (col.gameObject.tag == "Money")
        {
            //we picked up a Money

            money += 100;
            GameObject.Destroy(col.gameObject);
           
            //add a partical effect here
        }

        if (col.gameObject.tag == "Laser")
        {
            bulletDamagePlayer();
            GameObject.Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "Exit")
        {
            //exitlevel?
            nextButton.gameObject.SetActive(true);
        }
    }

    public void exitLevel()
    {
        SceneManager.LoadScene(1);
    

        if (level == 3)
        {
            //end of game
            SceneManager.LoadScene(0); 
        }
        level++;
    }
    public void stayLevel()
    {
        nextButton.gameObject.SetActive(false);
    }
    public void bulletDamagePlayer()
    {
        if (medicineLevel < 30)
        {
            //take more damage from melee
            health -= 5f;
        }
        else
        {
            health -= 10f;
        }
    }
    public void meleeDamagePlayer()
    {
        if(medicineLevel < 30)
        {
            //take more damage from melee
            health -= 10f;
        }
        else
        {
            health -= 5f;
        }
    }

}
