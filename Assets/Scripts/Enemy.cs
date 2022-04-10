using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy: MonoBehaviour
{
    // Todo: Use C# Events
    // public event Action<GameObject> enemyDied;
    // public event Action baseDamaged;
    public HealthBar healthBar;
    
    public int health;
    public int startHp;
    public float speed;
    public int value;

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
        
        healthBar.SetMaxHealth(startHp);
        health = startHp;

        FindClampRange();
    }

    //-----------------------------------------------------------------------------
    void Update()
    {
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
        if (Vector3.Dot(forward, toOther) <= 0.01f)
        {
            myWaypointIndex++;
            if (myWaypointIndex+1 > Waypoints.waypoints.Length)
            {
                // removes target Gameobject from List container in WaveSpawner to prevent null exception error
                EndPath();
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

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            // removes target Gameobject from List container in WaveSpawner to prevent null exception error
            WaveSpawner.enemiesInScene.Remove(gameObject);
            Die();
            return;
        }
        
        healthBar.SetHealth(health);
    }

    void Die()
    {
        PlayerStats.Money += value;
        Destroy(gameObject);
        traveling = false;
    }

    void EndPath()
    {
        PlayerStats.Lives--;
        WaveSpawner.enemiesInScene.Remove(gameObject);
        Destroy(gameObject);
        traveling = false;
    }
}
