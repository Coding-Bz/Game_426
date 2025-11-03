using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float speed = 1f;
    private int direction = 1;
    private Rigidbody2D rb;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
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
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

            transform.localScale = new Vector3(originalScale.x * direction, originalScale.y, originalScale.z);
        }

        if (collision.gameObject.name == "Player 1" || collision.gameObject.name == "Player 2")
        {
            Destroy(collision.gameObject);
        }
    }
}
