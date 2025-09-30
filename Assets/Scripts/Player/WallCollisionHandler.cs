using UnityEngine;

public class WallCollisionHandler : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isAgainstWall = false;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>(); 
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on parent of " + gameObject.name);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isAgainstWall = true;
            rb.linearVelocity = Vector2.zero; 
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isAgainstWall = false;
        }
    }

    public bool IsAgainstWall()
    {
        return isAgainstWall;
    }
}