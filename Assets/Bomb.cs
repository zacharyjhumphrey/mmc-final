using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float force = -22f;

    float countdown;
    bool hasExploded = false;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded) {
            Explode();
            hasExploded = true;
        }
    }

    void Explode() {
        Debug.Log("BOOOM");
        // Show explostion affect
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        Physics2D.OverlapCircle(transform.position, radius, contactFilter, colliders);

        foreach (Collider2D nearbyObject in colliders) {
            Rigidbody2D nearbyRb = nearbyObject.GetComponent<Rigidbody2D>();
            if (nearbyRb != null) {
                Debug.Log("BOOOM UP");
                nearbyRb.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
            }
        }

        Destroy(gameObject);
    }
}
