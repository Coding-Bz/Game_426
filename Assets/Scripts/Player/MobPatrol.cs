using UnityEngine;

public class MobPatrol : MonoBehaviour
{
    public float minX;   
    public float maxX;   
    public float speed = 1f;

    private int direction = 1;

    void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (transform.position.x >= maxX)
            direction = -1;
        else if (transform.position.x <= minX)
            direction = 1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}
