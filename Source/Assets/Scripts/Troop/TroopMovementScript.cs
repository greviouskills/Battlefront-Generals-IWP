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
                        // Calculate the direction towards the target position
                        Vector3 direction = (targetPosition - transform.position).normalized;

                        // Set the velocity of the Rigidbody to move towards the target
                        rb.velocity = direction * movespeed;
                    }
                    else
                    {
                        Debug.Log("destination reached");
                        rb.velocity = Vector3.zero; // Stop the Rigidbody when destination is reached
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
