using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactEffect;

    public float speed = 70f;

    public int damage = 1;
    
    private Transform target;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // checks if the bullet would already hit the target on the next frame
        if (dir.magnitude <= distanceThisFrame)
        {
            DamageTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    void DamageTarget()
    {
        if (target != null)
        {
            target.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        
        GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectInstance, 2f);

        Destroy(gameObject);
    }
}
