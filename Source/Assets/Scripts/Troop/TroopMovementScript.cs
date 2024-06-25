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
    // Start is called before the first frame update
    void Start()
    {
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
                    if (distance > 0.1f)
                    {
                        // Calculate the direction towards the target position
                        Vector3 direction = (targetPosition - transform.position).normalized;

                        // Calculate the amount to move this frame based on moveSpeed and time since last frame
                        float moveDistance = movespeed * Time.deltaTime;
                        // Make sure the unit doesn't overshoot the target
                        if (moveDistance > distance)
                        {
                            moveDistance = distance;
                        }

                        // Move the unit towards the target position
                        transform.position += direction * moveDistance;
                    }
                    else
                    {
                        Debug.Log("destination reached");
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
            if (Waypoints.Count > 0)
            {
                watermover.CheckWater();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
