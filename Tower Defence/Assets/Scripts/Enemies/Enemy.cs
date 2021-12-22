using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        [Header("Stats")]
        public float startSpeed = 2f;
        [FormerlySerializedAs("startHealth")] public float maxHealth = 20f;

        [HideInInspector]
        public float speed;
        //[HideInInspector]
        public float health;
    
        [Header("Death Stats")]
        public int deathMoney = 10;
        public int deathLives = 1;
        public int endPathMoney = 10;

        [Header("Health Bar")]
        public Image leftBar;
        public Image rightBar;

        public GameObject deathEffect;
        

        private void Awake()
        {
            speed = startSpeed;
            health = maxHealth;
        }
    
        // Called when the enemy takes damage
        public void TakeDamage(float amount, GameObject source)
        {
            health -= amount;

            leftBar.fillAmount = health / maxHealth;
            rightBar.fillAmount = health / maxHealth;

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

        public void FinishPath()
        {
            // Let our other systems know the enemy reached the end
            GameStats.lives -= deathLives;
            WaveSpawner.enemiesAlive--;
            GameStats.money += endPathMoney;
        
            Destroy(gameObject);
        }
    }
}
