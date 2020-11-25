using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform _target;
    public float range = 2.5f;

    public string enemyTag = "Enemy";

    public Transform partToRotate;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (var enemy in enemies)
        {
            float distanceToEnemey = Vector3.Distance(transform.position, enemy.transform.position);
            
            if (distanceToEnemey < shortestDistance)
            {
                shortestDistance = distanceToEnemey;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            _target = nearestEnemy.transform;
        }
        else
        {
            _target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            return;
        }
        
        var aimDir = ((Vector2)_target.position - (Vector2)transform.position).normalized;
        var lookDir = Vector3.Lerp(transform.up, aimDir, 1f);

        transform.rotation *= Quaternion.FromToRotation(transform.up, lookDir);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
