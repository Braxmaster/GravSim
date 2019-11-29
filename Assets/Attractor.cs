using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{

    const float G = 1f;

    public Rigidbody rb;
    public Vector3 position;

    private void FixedUpdate()
    {
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        foreach (Attractor attractor in attractors)
        {
            //Dont attract yourself
            if (attractor != this)
                Attract(attractor);
        }
        position = transform.position;
    }

    void Attract (Attractor objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;

        Vector3 direction = rb.position - rbToAttract.position;

        float distance = direction.magnitude;

        //Caculate the gravitational force
        Vector3 force = direction.normalized * (G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2));

        rbToAttract.AddForce(force);
    }
}
