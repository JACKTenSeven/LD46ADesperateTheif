using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyManager : MonoBehaviour
{

    public enum Facing { North, East, South, West };
    public Facing facing;
    public bool Attacking;
    public Animator anim;
    public GameObject laser;
    public LayerMask layerMask;

    public float health=100;

    void FixedUpdate()
    {
        findTarget();

        if (health <= 0)
        {
            //die
            die();
        }

        animationManager();
    }

    private void die()
    {
        //explosion? PE
        GameObject.Destroy(this.gameObject);
    }

    private void fireGun(Facing dir)
    {
        if (!Attacking)
        {
            facing = dir;
            StartCoroutine(gunAnimation());
            Attacking = true;
        }
      
    }

    private void findTarget()
    {
        RaycastHit2D north = Physics2D.Raycast(transform.position, Vector2.up, 10f, layerMask);
        if (north.collider != null)
        {
            if (north.collider.tag == "Player")
            {
                fireGun(Facing.North);
            }
            
        }
        RaycastHit2D east = Physics2D.Raycast(transform.position, Vector2.right, 10f, layerMask);
        if (east.collider != null)
        {
            if (east.collider.tag == "Player")
            {
                fireGun(Facing.East);
            }
        }
        RaycastHit2D south = Physics2D.Raycast(transform.position, Vector2.down, 10f, layerMask);
        if (south.collider != null)
        {
            if (south.collider.tag == "Player")
            {
                fireGun(Facing.South);
            }
        }
        RaycastHit2D west = Physics2D.Raycast(transform.position, Vector2.left, 10f, layerMask);
        if (west.collider != null)
        {
            if (west.collider.tag == "Player")
            {
                fireGun(Facing.West);
            }
        }
    }

    private IEnumerator gunAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("Attacking", true);
        yield return new WaitForSeconds(0.8f);

        GameObject b = Instantiate(laser);
        b.gameObject.transform.position = this.gameObject.transform.position;
        if (facing == Facing.North)
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

        anim.SetBool("Attacking", false);
        Attacking = false;
    }

    private void animationManager()
    {
        if (facing == Facing.North)
        {
            anim.SetInteger("Facing", 0);
        }
        if (facing == Facing.East)
        {
            anim.SetInteger("Facing", 3);
        }
        if (facing == Facing.South)
        {
            anim.SetInteger("Facing", 2);
        }
        if (facing == Facing.West)
        {
            anim.SetInteger("Facing", 1);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            col.gameObject.GetComponent<BulletRemover>().removeBullet();
            // take damage
            health -= 50;
        }
    }
}

