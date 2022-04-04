using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour
{
    public event Action<GameObject> enemyDied;
    public event Action baseDamaged;
    
    public int health;
    public float speed;
    public int coins;

    private Transform targetWaypoint;
    private Vector3 movementDirection;
    
    private bool traveling;
    private int myWaypointIndex = 0;
    private float minPosX;
    private float minPosZ;
    private float maxPosX;
    private float maxPosZ;

    //-----------------------------------------------------------------------------
    void Start()
    {
        targetWaypoint = Waypoints.waypoints[0];
        transform.LookAt(targetWaypoint);
        movementDirection = (targetWaypoint.position - transform.position).normalized;
        traveling = true;

        FindClampRange();
    }

    //-----------------------------------------------------------------------------
    void Update()
    {
        // Debug.Log("MinPos: (" + minPosX + ", " + minPosY + ")");
        // Debug.Log("MaxPos: (" + maxPosX + ", " + maxPosY + ")");

        if (!traveling)
        {
            return;
        }

        EnemyMovement();
    }

    //-----------------------------------------------------------------------------

    void EnemyMovement()
    {
        Vector3 newPosition = transform.position;
        newPosition += movementDirection * speed * Time.deltaTime;

        newPosition = new Vector3(Mathf.Clamp(newPosition.x, minPosX, maxPosX),
            newPosition.y, Mathf.Clamp(newPosition.z, minPosZ, maxPosZ));
        transform.position = newPosition;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 toOther = targetWaypoint.position - transform.position;
        // Debug.Log(Vector3.Dot(forward, toOther));
        if (Vector3.Dot(forward, toOther) <= 0.01f)
        {
            myWaypointIndex++;
            // Debug.Break();
            // Debug.Log("Waypoint reached!");
            if (myWaypointIndex+1 > Waypoints.waypoints.Length)
            {
                // Debug.Log("Destination Reach!");
                if (baseDamaged != null) baseDamaged();
                Destroy(gameObject);
                traveling = false;
                return;
            }
            TargetNextWaypoint();
            FindClampRange();
        }
    }

    private void FindClampRange()
    {
        minPosX = transform.position.x;
        minPosZ = transform.position.z;
        maxPosX = Waypoints.waypoints[myWaypointIndex].position.x;
        maxPosZ = Waypoints.waypoints[myWaypointIndex].position.z;

        if (minPosX > maxPosX)
        {
            (minPosX, maxPosX) = (maxPosX, minPosX);
        }
        
        if (minPosZ > maxPosZ)
        {
            (minPosZ, maxPosZ) = (maxPosZ, minPosZ);
        }
    }
    
    private void TargetNextWaypoint()
    {
        targetWaypoint = Waypoints.waypoints[myWaypointIndex];
        transform.LookAt(targetWaypoint);
        movementDirection = (targetWaypoint.position - transform.position).normalized;
    }

    public void ReduceHealth()
    {
        // Debug.Log("Ouch");
        health -= 1;
        if (health == 0)
        {
            // Debug.Log("Enemy Died!");
            if (enemyDied != null)
            {
                enemyDied(gameObject);
            }
            Destroy(gameObject);
        }
    }
}
