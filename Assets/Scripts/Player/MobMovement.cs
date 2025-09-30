using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float speed = 2f;
    private int direction = 1;

    void Update()
    {
        transform.position = new Vector2(transform.position.x + direction * speed * Time.deltaTime, transform.position.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player 1" || collision.gameObject.name == "Player 2")
        {
            Destroy(collision.gameObject);
        }
    }
}