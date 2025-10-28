using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float speed = 5f;
    private int direction = 1;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(direction * speed, 0);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1;
            Vector2 newVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
            rb.linearVelocity = newVelocity;

            Vector2 newPosition = transform.position;
            newPosition.x += direction * 0.1f; 
            transform.position = newPosition;
        }

        if (collision.gameObject.name == "Player 1" || collision.gameObject.name == "Player 2")
        {
            Destroy(collision.gameObject);
        }
    }
}
