using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Magnet : MonoBehaviour
{
    [Header("Pole")]
    public bool north;                 // true = Nordpol, false = Südpol

    [Header("Strength")]
    public float power = 1f;           // Magnetsstärke (skalierbar)
    public float forceMultiplier = 50; // globales Tuning

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // WICHTIG: Rigidbody2D muss Dynamic sein, sonst wirken AddForce nicht.
        // Collider2D eines der beiden Objekte muss "Is Trigger" sein, damit OnTriggerStay2D feuert.
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var otherMagnet = other.GetComponent<Magnet>();
        if (otherMagnet == null) return;

        Vector2 diff = (Vector2)(transform.position - otherMagnet.transform.position);
        float distSqr = Mathf.Max(diff.sqrMagnitude, 0.0001f); // Schutz gegen 0
        Vector2 dir = diff.normalized;

        // Stärke ~ (power1 * power2) / r^2
        float strength = (power * otherMagnet.power) / distSqr;
        Vector2 force = dir * strength * forceMultiplier;

        bool likePoles = (north == otherMagnet.north); // gleich = abstoßen, ungleich = anziehen
        rb.AddForce(likePoles ? force : -force, ForceMode2D.Force);
    }
}
