using UnityEngine;
using UnityEngine.UI;
using Waves;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float startSpeed = 2f;
    public float startHealth = 20f;
    
    [HideInInspector]
    public float speed;
    //[HideInInspector]
    public float health;
    public int deathMoney = 10;

    [Header("Health Bar")]
    public Image leftBar;
    public Image rightBar;

    public GameObject deathEffect;

    private void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }
    
    // Called when the enemy takes damage
    public void TakeDamage(float amount)
    {
        health -= amount;

        leftBar.fillAmount = health / startHealth;
        rightBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
        }
    }
    
    // Called when we slow the enemy (permanent effect)
    public void Slow(float slowPercentage)
    {
        speed = startSpeed * (1f - slowPercentage);
    }
    
    // Called when we die
    private void Die()
    {
        // Spawn death effect
        var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        
        // Let the wave spawner know the enemy is dead
        WaveSpawner.enemiesAlive--;
        
        // Grant money and destroy self
        GameStats.money += deathMoney;
        Destroy(gameObject);
    }
}
