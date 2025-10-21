using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float speed = 2f;
    private int direction = 1;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(direction * speed, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction = -direction;
            rb.linearVelocity = new Vector2(direction * speed, 0);
        }

        if (collision.gameObject.name == "Player 1" || collision.gameObject.name == "Player 2")
        {
            Destroy(collision.gameObject);
        }
    }
}
