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
                            if(distance > 1f)
                            {
                                // Calculate the direction to move based on the position of the Collided object
                                Vector3 directionToCollided = Collided.transform.position - transform.position;
                                directionToCollided = directionToCollided.normalized;

                                // Determine the direction to move based on the relative position
                                if (Mathf.Abs(directionToCollided.z) > Mathf.Abs(directionToCollided.x) && directionToCollided.z > 0) // Front collision
                                {
                                    rb.velocity = new Vector3(-movespeed, 0, 0); // Move left
                                }
                                else if (Mathf.Abs(directionToCollided.z) > Mathf.Abs(directionToCollided.x) && directionToCollided.z < 0) // Back collision
                                {
                                    rb.velocity = new Vector3(movespeed, 0, 0); // Move right
                                }
                                else if (Mathf.Abs(directionToCollided.x) > Mathf.Abs(directionToCollided.z) && directionToCollided.x < 0) // Left collision
                                {
                                    rb.velocity = new Vector3(0, 0, -movespeed); // Move backward
                                }
                                else if (Mathf.Abs(directionToCollided.x) > Mathf.Abs(directionToCollided.z) && directionToCollided.x > 0) // Right collision
                                {
                                    rb.velocity = new Vector3(0, 0, movespeed); // Move forward
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
