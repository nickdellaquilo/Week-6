using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;
    GameObject enemy;
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        rb = GetComponent<Rigidbody2D>();

        Vector2 dir = (enemy.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(dir.x, dir.y);

        Destroy(this.gameObject, 2);
    }
}
