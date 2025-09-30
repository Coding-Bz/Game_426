using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float speed = 2f;
    private int direction = 1; 
    private Rigidbody2D rb;
    private WallCollisionHandler wallHandler;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallHandler = transform.Find("WallCollisionHandler")?.GetComponent<WallCollisionHandler>();
        if (wallHandler == null)
        {
            Debug.LogError("WallCollisionHandler not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (wallHandler != null && wallHandler.IsAgainstWall())
        {
            direction *= -1; 
        }
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player 1" || collision.gameObject.name == "Player 2")
        {
            Destroy(collision.gameObject); 
        }
    }
}