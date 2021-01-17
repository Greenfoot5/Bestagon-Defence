using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public readonly float StartSpeed = 2f;
    [HideInInspector]
    public float speed;
    public float health = 20;
    public int deathMoney = 10;

    public GameObject deathEffect;

    void Start()
    {
        speed = StartSpeed;
    }
    
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Slow(float slowPercentage)
    {
        speed = StartSpeed * (1f - slowPercentage);
    }

    private void Die()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);

        GameStats.money += deathMoney;
        Destroy(gameObject);
    }
}
