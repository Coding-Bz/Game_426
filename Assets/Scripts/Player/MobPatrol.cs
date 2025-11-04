using UnityEngine;

public class MobPatrol : MonoBehaviour
{
    public float minX;
    public float maxX;
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

    void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (transform.position.x >= maxX)
            direction = -1;
        else if (transform.position.x <= minX)
            direction = 1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(originalScale.x) * direction;
        transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1;
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
            transform.localScale = new Vector3(originalScale.x * direction, originalScale.y, originalScale.z);
        }

        if (collision.gameObject.name == "player1" || collision.gameObject.name == "player2")
        {
            Destroy(collision.gameObject);
        }
    }
}
