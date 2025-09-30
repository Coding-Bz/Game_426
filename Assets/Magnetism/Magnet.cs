using UnityEngine;

public class Magnet : MonoBehaviour
{
    public bool north;
    public float power;
    private Rigidbody2D rigidbody; 

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        // get the other magnet component from the other colliding object
        Magnet magnet = other.GetComponent<Magnet>();
        if (magnet == null) return;

        // calculates force
        Vector3 direction = transform.position - magnet.transform.position;
        direction = direction.normalized;
        Vector3 force = direction / 
                        (Vector3.Distance(transform.position, magnet.transform.position) * 
                         Vector3.Distance(transform.position, magnet.transform.position)) 
                         * power * magnet.power;

        // decides in wich direction the force shoulde be
        if ((north && magnet.north) || (!north && !magnet.north))
        {
            rigidbody.AddForce(force);
        }
        else
        {
            rigidbody.AddForce(-force);
        }
    }
}