using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public float speed = 4;
    public float JumpHeight = 1.2f;
    public Rigidbody rb;

    private float TimeOnGround = 0;


    float distanceToGround;
    bool OnGround;
    Vector3 Groundnormal;
    Vector3 forward;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        //Get all attractors in scene
        Attractor[] attractors = FindObjectsOfType<Attractor>();

        //Movement
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Translate(x, 0, z);

        Vector3 movement = new Vector3(x, 0.0f, z);
        forward = movement;

        //Rotation
        if (Input.GetKey(KeyCode.E))
        {

            transform.Rotate(0, 150 * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {

            transform.Rotate(0, -150 * Time.deltaTime, 0);
        }


        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(OnGround == true)
            {
                rb.AddForce(transform.up * 80000 * JumpHeight);

            }

        }

        //Ground control
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {

            distanceToGround = hit.distance;
            Groundnormal = hit.normal;

            if (distanceToGround <= 1f)
            {
                OnGround = true;
                TimeOnGround += Time.deltaTime;
            }
            else
            {
                OnGround = false;
                TimeOnGround = 0;
            }

        }

        //Always point car down
        var closestPlanet = GetClosestPlanet(attractors).position;

        var heading = closestPlanet - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;


        var goalRotation = Quaternion.FromToRotation(Vector3.up, -direction);

        if (!OnGround)
        {
            transform.rotation = Quaternion.LookRotation(transform.forward, -heading);
        }


        //Keep car steady when on ground
        if (!Input.anyKey && TimeOnGround > 1)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        } else
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }


    Attractor GetClosestPlanet(Attractor[] attractors)
    {   
        Attractor tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Attractor t in attractors)
        {

            if(t.tag != "Player")
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }

            
        }
        return tMin;
    }
    






}
