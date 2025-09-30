using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float speed = 2f;
    private int direction = 1; 
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1; 
        }
        else if (collision.gameObject.name == "Player 1" || collision.gameObject.name == "Player 2")
        {
            Destroy(collision.gameObject); 
        }
    }
}