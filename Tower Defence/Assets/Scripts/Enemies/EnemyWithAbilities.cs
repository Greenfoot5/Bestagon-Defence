using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemies
{
    public class EnemyWithAbilities : Enemy
    {
        [Header("Abilities")]
        public EnemyAbility[] startingAbilities;
        
        // Abilities for each trigger
        private List<EnemyAbility> _timerAbilities = new List<EnemyAbility>();
        private List<EnemyAbility> _hitAbilities = new List<EnemyAbility>();
        private List<EnemyAbility> _deathAbilities = new List<EnemyAbility>();
        private List<EnemyAbility> _finishAbilities = new List<EnemyAbility>();

        private void Start()
        {
            // Starting stats
            speed = startSpeed;
            health = maxHealth;
            
            // Add each starting ability to the correct list
            foreach (var ability in startingAbilities)
            {
                GrantAbility(ability);
            }
        }

        public void GrantAbility(EnemyAbility ability)
        {
            if (ability.triggers.Contains(AbilityTrigger.OnTimer))
            {
                _timerAbilities.Add(ability);
                StartCoroutine(TimerAbility(ability));
            }
            if (ability.triggers.Contains(AbilityTrigger.OnDamage))
            {
                _hitAbilities.Add(ability);
            }
            if (ability.triggers.Contains(AbilityTrigger.OnDeath))
            {
                _deathAbilities.Add(ability);
            }
            if (ability.triggers.Contains(AbilityTrigger.OnEnd))
            {
                _finishAbilities.Add(ability);
            }
        }
        
        /// <summary>
        /// Activates abilities on a timer
        /// </summary>
        /// <param name="ability">The ability to activate</param>
        /// <returns></returns>
        public IEnumerator TimerAbility(EnemyAbility ability)
        {
            // Check we still have the ability to use
            if (!_timerAbilities.Contains(ability))
            {
                yield break;
            }
            
            yield return new WaitForSeconds(ability.timer);
            
            ActivateAbilities(new [] { ability }, null);
        }
        
        // Called when the enemy takes damage
        public new void TakeDamage(float amount, GameObject source)
        {
            ActivateAbilities(_hitAbilities, source);
            
            // Base health stuff
            health -= amount;

            leftBar.fillAmount = health / maxHealth;
            rightBar.fillAmount = health / maxHealth;

            if (health <= 0)
            {
                Die(gameObject);
            }
        }
        
        // Called when we die
        private void Die(GameObject source)
        {
            ActivateAbilities(_deathAbilities, source);
            
            // Spawn death effect
            var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        
            // Let the wave spawner know the enemy is dead
            WaveSpawner.enemiesAlive--;
        
            // Grant money and destroy self
            GameStats.money += deathMoney;
            Destroy(gameObject);
        }
        
        public new void FinishPath()
        {
            ActivateAbilities(_finishAbilities, null);
            
            // Let our other systems know the enemy reached the end
            GameStats.lives -= deathLives;
            WaveSpawner.enemiesAlive--;
            GameStats.money += endPathMoney;
        
            Destroy(gameObject);
        }

        public void ActivateAbilities(IEnumerable<EnemyAbility> abilities, GameObject source)
        {
            foreach (var ability in abilities)
            {
                Debug.Log("Activating ability" + ability);
                switch (ability.targetingType)
                {
                    case AbilityTarget.Single:
                        // The target may be the damage source or the enemy itself
                        ability.Activate(source);
                        ability.Activate(gameObject);
                        Debug.Log("Casting ability");
                        break;
                    case AbilityTarget.Radius:
                        Debug.Log("RANGE!");
                        var colliders = Physics2D.OverlapCircleAll(transform.position, ability.range);
                        foreach (var coll in colliders)
                        {
                            if (coll.CompareTag("Enemy") || coll.CompareTag("Turret"))
                            {
                                Debug.Log(coll.gameObject.name);
                                ability.Activate(coll.gameObject);
                            }
                        }
                        break;
                    case AbilityTarget.All:
                        Debug.Log("everything....");
                        var turrets = GameObject.FindGameObjectsWithTag("Turret");
                        foreach (var target in turrets)
                        {
                            ability.Activate(target);
                        }
                        
                        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
                        foreach (var target in enemies)
                        {
                            ability.Activate(target);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
