using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    private int inputX;
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
        MovePlayer();
    }

    void MovePlayer()
    {
        if (wallHandler != null && wallHandler.IsAgainstWall())
        {
            inputX = 0; 
        }
        else
        {
            inputX = 0;
            if (Input.GetKey(KeyCode.D))
            {
                inputX = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                inputX = -1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); 
        }

        rb.linearVelocity = new Vector2(inputX * speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Mob")
        {
            Destroy(gameObject); 
        }
    }
}