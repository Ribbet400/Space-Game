using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    const float G = 667.4f;

    public Rigidbody rb;

    public static List<Gravity> Attractors;

    void OnEnable()
    {
        if (Attractors == null)
        {
            Attractors = new List<Gravity>();
        }
        Attractors.Add(this);
    }
    private void OnDisable()
    {
        Attractors.Remove(this);
    }
    void FixedUpdate()
    {
        foreach (Gravity attractor in Attractors)
        {
            if (attractor != this)
            {
                Attraction(attractor);
            }         
        }
    }

    void Attraction(Gravity objectToAttract)
    {
        Rigidbody rbToAttract = objectToAttract.rb;

        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0)
        {
            return;
        }

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2); // Newton equation
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);

    }
}
