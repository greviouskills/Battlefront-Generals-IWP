using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopMovementScript : MonoBehaviour
{
    public List<Vector3> Waypoints = new List<Vector3>();
    public float movespeed;
    public bool canmove = true;
    [SerializeField] private Transform model;
    [SerializeField] private WaterMovement watermover;
    private Rigidbody rb;
    public GameObject Collided;
    public string EndTarget;
    public float fieldOfView = 90.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (watermover != null)
        {
            StartCoroutine(watercheck());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Waypoints.Count > 0)
        {
            if (Waypoints[0] != null)
            {
                Vector3 targetPosition = Waypoints[0];
                model.LookAt(targetPosition);

                if (canmove)
                {
                    float distance = Vector3.Distance(transform.position, targetPosition);
                    // Check if the unit has reached the target position
                    if (distance > 0.5f)
                    {
                        if (Collided != null)
                        {
                            if(Collided.name != EndTarget)
                            {

                                Vector3 directionToTarget = (Collided.transform.position - model.position).normalized;
                                Vector3 forward = model.forward;
                                Vector3 right = model.right;

                                // Check if the target is in front of the object
                                if (Vector3.Dot(forward, directionToTarget) > 0)
                                {

                                    // Check if the target is more on the left or right
                                    if (Vector3.Dot(right, directionToTarget) > 0)
                                    {
                                        // Target is on the right side
                                      

                                        rb.velocity = -right * (movespeed * 1.5f);
                                    }
                                    else
                                    {
                                        // Target is on the left side

                                        rb.velocity = right * (movespeed * 1.5f);
                                    }
                                }
                                else
                                {
                                    Vector3 direction = (targetPosition - transform.position).normalized;
                                    Vector3 Velocity = direction * movespeed;
                                    rb.velocity = Velocity;
                                }
                            
                            }
                            
                            else
                            {
                                Vector3 direction = (targetPosition - transform.position).normalized;
                                Vector3 Velocity = direction * movespeed;
                                rb.velocity = Velocity;
                            }
                        }
                        else
                        {
                            Vector3 direction = (targetPosition - transform.position).normalized;
                            Vector3 Velocity = direction * movespeed;
                            rb.velocity = Velocity;
                        }


                    }
                    else
                    {
                        Debug.Log("destination reached");
                        rb.velocity = Vector3.zero; // Stop the Rigidbody when destination is reached
                        transform.position = new Vector3(Waypoints[0].x, transform.position.y, Waypoints[0].z);
                        Waypoints.RemoveAt(0);
                    }
                }
            }
        }
    }

    private IEnumerator watercheck()
    {
        while (true)
        {
            if (Waypoints.Count > 0 && canmove)
            {
                watermover.CheckWater();
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}
