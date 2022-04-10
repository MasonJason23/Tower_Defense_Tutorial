using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Unity Set-up Fields")]
    public Transform partToRotateRef;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public GameObject spawnEffect;
    
    [Header("Attributes")]
    public float range = 15f;
    public float turnSpeed = 10f;
    
    public float fireRate = 2f;
    private float fireDelay = 0f;
    
    private Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnInstance = (GameObject) Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(spawnInstance, 3f);
        
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.2f);
    }

    void UpdateTarget()
    {
        // Debug.Log("Turret - Enemies Alive: " + WaveSpawner.enemiesInScene.Count);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        
        foreach (GameObject enemy in WaveSpawner.enemiesInScene)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        // target lock-on
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotateRef.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotateRef.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        
        if (fireDelay <= 0f)
        {
            Shoot();
            fireDelay = 1f / fireRate;
        }

        fireDelay -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bulletGameObject = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
