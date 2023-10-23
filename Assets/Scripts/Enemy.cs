using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform player;
    public float lineOfSight = 3f;
    public float speed = 3f;
    bool tooClose = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distFromPlayer > 3)
        {
            tooClose = false;
        }
        if (distFromPlayer < lineOfSight)
        {
            if (!tooClose)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            }
        }
        var dif = player.transform.position - transform.position;
        if(dif.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if(dif.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("oops");
        if (collision.gameObject.CompareTag("Player"))
        {
            tooClose = true;
        }
    }
}
