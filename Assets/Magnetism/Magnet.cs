using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Magnet2D : MonoBehaviour
{
    [Header("Pole")]
    public bool north = true;                 // true = North, false = South

    [Header("Strength & Tuning")]
    [Min(0f)] public float power = 1f;        // Magnet strength
    public float forceMultiplier = 50f;       // Global tuning
    public float minDistance = 0.1f;          // Avoids 1/r^2 explosions
    public float maxForce = 200f;             // Clamp for stability

    [Header("Interaction (Trigger)")]
    public float interactionRadius = 3f;      // Effective range (for gizmos/autoset)
    
    private Rigidbody2D rb;
    private readonly HashSet<Magnet2D> contacts = new();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // RB must be Dynamic for AddForce to work
        if (rb.bodyType != RigidbodyType2D.Dynamic)
        {
            Debug.LogWarning($"{name}: Rigidbody2D must be Dynamic for magnetism to work.");
        }

        // Ensure at least one collider is trigger (this one is ok)
        var col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            Debug.LogWarning($"{name}: Collider2D should be 'Is Trigger' for trigger-based magnetism.");
        }
    }

    // Track who is overlapping; we’ll apply forces in FixedUpdate for consistent timesteps
    void OnTriggerEnter2D(Collider2D other)
    {
        var m = other.GetComponent<Magnet2D>();
        if (m != null && m != this) contacts.Add(m);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var m = other.GetComponent<Magnet2D>();
        if (m != null) contacts.Remove(m);
    }

    void FixedUpdate()
    {
        // Apply pairwise forces once per pair (ID check prevents double-application)
        foreach (var other in contacts)
        {
            if (other == null) continue;
            if (GetInstanceID() > other.GetInstanceID()) continue; // only one side does the pair

            ApplyMagneticForces(this, other);
        }
    }

    private void ApplyMagneticForces(Magnet2D a, Magnet2D b)
    {
        if (a.rb == null || b.rb == null) return;
        if (a.rb.bodyType != RigidbodyType2D.Dynamic && b.rb.bodyType != RigidbodyType2D.Dynamic) return;

        // Direction from a -> b
        Vector2 delta = (Vector2)(b.rb.worldCenterOfMass - a.rb.worldCenterOfMass);
        float dist = Mathf.Max(delta.magnitude, minDistance);
        Vector2 dir = delta / dist;

        // Magnitude ~ (p1 * p2) / r^2
        float magnitude = (a.power * b.power) / (dist * dist);

        // Like poles repel, unlike attract
        bool likePoles = (a.north == b.north);
        if (likePoles) magnitude = -magnitude; // negative flips direction to repel

        Vector2 force = dir * magnitude * a.forceMultiplier;

        // Clamp for stability
        if (force.sqrMagnitude > maxForce * maxForce)
            force = force.normalized * maxForce;

        // Apply equal & opposite forces (only if that body can move)
        if (a.rb.bodyType == RigidbodyType2D.Dynamic) a.rb.AddForce(force, ForceMode2D.Force);
        if (b.rb.bodyType == RigidbodyType2D.Dynamic) b.rb.AddForce(-force, ForceMode2D.Force);
    }

    void OnValidate()
    {
        // Optional quality-of-life: auto-size a CircleCollider2D if present
        var circle = GetComponent<CircleCollider2D>();
        if (circle != null)
        {
            circle.isTrigger = true;
            circle.radius = interactionRadius;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize range and pole
        Gizmos.color = north ? new Color(0.2f, 0.6f, 1f, 0.4f) : new Color(1f, 0.3f, 0.3f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
