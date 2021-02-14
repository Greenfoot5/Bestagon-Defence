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
    
    // Called when the enemy takes damage
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }
    
    // Called when we slow the enemy (permanent effect)
    public void Slow(float slowPercentage)
    {
        speed = StartSpeed * (1f - slowPercentage);
    }
    
    // Called when we die
    private void Die()
    {
        // Spawn death effect
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        
        // Grant money and destroy self
        GameStats.money += deathMoney;
        Destroy(gameObject);
    }
}
