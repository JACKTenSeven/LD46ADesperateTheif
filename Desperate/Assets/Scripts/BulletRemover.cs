using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRemover : MonoBehaviour
{
    private float timer = 10f;
    void FixedUpdate()
    {
        timer -= 1 * Time.deltaTime;
        if (timer <= 0)
        {
            removeBullet();
        }
    }
    public void removeBullet()
    {
        GameObject.Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
