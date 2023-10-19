using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    
    private bool isMoving;
    private Rigidbody2D rb;
    private Tilemap tilemap;
    private Vector2 input;
    Vector2 curPos;

    private void Update()
    {
        curPos = new Vector2(transform.position.x, transform.position.y);

        if(!isMoving)
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");

            if(input != Vector2.zero)
            {
                Vector2 targetPos = curPos + input;

                StartCoroutine(Move(targetPos));
            }
        }
    }

    IEnumerator Move(Vector2 targetPos)
    {
        isMoving = true;

        while((targetPos - curPos).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector2.MoveTowards(curPos, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        curPos = targetPos;
        isMoving = false;
    }
}
