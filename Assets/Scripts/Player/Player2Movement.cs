using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float speed;
    private int inputX;
    private int inputY;
    void Update()
    {
        movePlayer();
    }
    void movePlayer()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            inputX = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputX = -1;
        }if (Input.GetKey(KeyCode.UpArrow))
        {
            inputY = 2;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            inputX = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputX = -1;
        }
        else
        {
            inputX = 0;
            inputY = 0;
        }

        transform.position = new Vector2(transform.position.x + inputX * speed * Time.deltaTime, transform.position.y + inputY * speed * Time.deltaTime);
    }
}
