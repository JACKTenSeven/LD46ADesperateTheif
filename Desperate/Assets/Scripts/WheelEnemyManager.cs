using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelEnemyManager : MonoBehaviour
{
    public enum Facing { NS, EW };
    public Facing facing;
    public Rigidbody2D rb;
    public LayerMask layerMask;
    public Vector2 chargeLocation;
    public Animator anim;
    public bool Attacking;
    private Vector2 dir;

    public float health=100;

    void FixedUpdate()
    {
        if (!Attacking)
        {
            findTarget();
        }

        if (health <= 0)
        {
            die();
        }
    }

    private void die()
    {
        GameObject.Destroy(this.gameObject);
    }
    private void findTarget()
    {
        Vector2 origin = new Vector2(this.transform.position.x, this.transform.position.y);
        RaycastHit2D hit = Physics2D.CircleCast(origin, 8f, Vector2.zero, 0f, layerMask);
        if (hit.collider != null)
        {
            chargeLocation = new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
            StartCoroutine(chargePlayer());
            Attacking = true;
            
        }
    }
   
    private IEnumerator chargePlayer()
    {
        //which way are we facing??
        dir = chargeLocation - (Vector2)this.transform.position;
        

        if (((Mathf.Abs(dir.x)) > Mathf.Abs(dir.y)))
        {
            facing = Facing.EW;
            anim.SetInteger("Facing", 1);
        }
        else
        {
            facing = Facing.NS;
            anim.SetInteger("Facing", 0);
        }
        dir.Normalize();
        //the player is in that direction
        anim.SetBool("Moving", true);
        yield return new WaitForSeconds(1f);
        //go
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = new Vector2(dir.x * 15, dir.y * 15);
        
        
        yield return new WaitForSeconds(3f);
        if (Attacking)
        {
            rb.velocity = Vector2.zero;
            Attacking = false;
            anim.SetBool("Moving", false);
        }
        else
        {
            StopAllCoroutines();
        }
        
    }

 

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            //stop attacking and find target
            //rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.velocity = Vector2.zero;
            Attacking = false;
            anim.SetBool("Moving", false);
            StopAllCoroutines();
        }
        if (collision.gameObject.tag == "Player")
        {
            
          //  rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(-dir.x * 10f, -dir.y * 10f));
            Attacking = false;
            anim.SetBool("Moving", false);
            collision.gameObject.GetComponent<PlayerManager>().meleeDamagePlayer();
            StopAllCoroutines();
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            health -= 50;
            rb.velocity = Vector2.zero;
            Attacking = false;
            anim.SetBool("Moving", false);
            StopAllCoroutines();
        }
    }

}
