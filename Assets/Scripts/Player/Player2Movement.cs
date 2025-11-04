using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2Movement : MonoBehaviour
{
    public float speed;
    private int inputX;
    private int inputY;
    public GameManager gameManager;

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        inputX = 0;
        inputY = 0;

        if (Input.GetKey(KeyCode.RightArrow))
            inputX = 1;
        else if (Input.GetKey(KeyCode.LeftArrow))
            inputX = -1;

        if (Input.GetKey(KeyCode.UpArrow))
            inputY = 1;

        if (inputX != 0)
            transform.localScale = new Vector3(0.7f * inputX, 0.7f, 1);

        transform.position = new Vector2(transform.position.x + inputX * speed * Time.deltaTime, transform.position.y + inputY * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Mob" || 
            collision.gameObject.name == "Lava" || 
            collision.gameObject.name == "Spike")
        {
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("EndFlag"))
        {
           SceneManager.LoadScene("Winning");
        }
    } 
}