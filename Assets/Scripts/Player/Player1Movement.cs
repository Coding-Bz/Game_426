using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    public float speed;
    private int inputX;
    private int inputY;

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        inputX = 0;
        inputY = 0;

        if (Input.GetKey(KeyCode.D))
        {
            inputX = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputX = -1;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            inputY = 2;
        }

        transform.position = new Vector2(transform.position.x + inputX * speed * Time.deltaTime, transform.position.y + inputY * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Mob")
        {
            Destroy(gameObject);
        }
    } 
}