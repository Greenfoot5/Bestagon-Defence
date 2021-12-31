using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        //[HideInInspector]
        public float speed;
        [HideInInspector]
        public float health;
    
        [Header("Death Stats")]
        public int deathMoney = 10;
        public int deathLives = 1;
        public int endPathMoney = 10;

        [Header("Health Bar")]
        public Image leftBar;
        public Image rightBar;

        public GameObject deathEffect;
        
        [Header("Abilities")]
        public EnemyAbility[] startingAbilities;
        public GameObject iconLayout;
        
        // Abilities for each trigger
        private readonly List<EnemyAbility> _timerAbilities = new List<EnemyAbility>();
        private readonly List<(EnemyAbility ability, int count)> _hitAbilities = new List<(EnemyAbility, int)>();
        private readonly List<EnemyAbility> _deathAbilities = new List<EnemyAbility>();
        private readonly List<EnemyAbility> _finishAbilities = new List<EnemyAbility>();

        private void Awake()
        {
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
            // TODO - Have better ability checking. Perhaps check if one is a higher tier than another,
            // TODO - or, for an extra challenge, reset the timer on the current one.
            var existingIcon = iconLayout.transform.Find($"{ability.name} Icon");
            if (existingIcon != null)
            {
                return;
            }
            Debug.Log($"Granting Ability: {ability.name}");
            
            if (ability.triggers.Contains(AbilityTrigger.OnTimer))
            {
                _timerAbilities.Add(ability);
                StartCoroutine(TimerAbility(ability));
            }
            if (ability.triggers.Contains(AbilityTrigger.OnDamage))
            {
                _hitAbilities.Add((ability, (int) ability.startCount));
            }
            if (ability.triggers.Contains(AbilityTrigger.OnDeath))
            {
                _deathAbilities.Add(ability);
            }
            if (ability.triggers.Contains(AbilityTrigger.OnEnd))
            {
                _finishAbilities.Add(ability);
            }
            if (ability.triggers.Contains(AbilityTrigger.OnGrant))
            {
                ActivateAbilities(new[] {ability}, null);
                if (ability.startCount > 0)
                {
                    StartCoroutine(GrantedAbilityCounter(ability));
                }
            }
            
            var icon = new GameObject($"{ability.name} Icon");
            icon.AddComponent(typeof(Image));
            icon.GetComponent<Image>().sprite = ability.abilityIcon;
            icon.transform.SetParent(iconLayout.transform);

            // Fix the icon's size
            var iLayoutTransform = (RectTransform)iconLayout.transform;
            var iTransform = (RectTransform)icon.transform;
            var ratio = ability.abilityIcon.rect.height / iLayoutTransform.rect.height;
            iTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ability.abilityIcon.rect.width / ratio);
        }

        private void RevokeAbility(EnemyAbility ability)
        {
            Debug.Log($"Revoked Ability: {ability.name}");
            if (ability.triggers.Contains(AbilityTrigger.OnTimer))
            {
                _timerAbilities.Remove(ability);
            }
            if (ability.triggers.Contains(AbilityTrigger.OnDamage))
            {
                // TODO - Find the counter properly
                _hitAbilities.Remove((ability, 0));
            }
            if (ability.triggers.Contains(AbilityTrigger.OnDeath))
            {
                _deathAbilities.Remove(ability);
            }
            if (ability.triggers.Contains(AbilityTrigger.OnEnd))
            {
                _finishAbilities.Remove(ability);
            }

            var icon = iconLayout.transform.Find($"{ability.name} Icon");
            Destroy(icon.gameObject);
        }

        private IEnumerator GrantedAbilityCounter(EnemyAbility ability)
        {
            yield return new WaitForSeconds(ability.startCount);
            
            RevokeAbility(ability);
            EndCounterAbility(ability, null);
        }
        
        /// <summary>
        /// Activates abilities on a timer
        /// </summary>
        /// <param name="ability">The ability to activate</param>
        /// <returns></returns>
        private IEnumerator TimerAbility(EnemyAbility ability)
        {
            var counter = ability.startCount;
            // Check we still have the ability to use
            while (_timerAbilities.Contains(ability))
            {
                yield return new WaitForSeconds(ability.timer);

                ActivateAbilities(new[] { ability}, null);
                
                // Decrease the counter
                counter -= 1;
                if (counter != 0) continue;
                
                // Remove the ability
                RevokeAbility(ability);
                EndCounterAbility(ability, null);
                yield break;
                
            }
        }
    
        // Called when the enemy takes damage
        public void TakeDamage(float amount, GameObject source)
        {
            var abilities = _hitAbilities.Select(item => item.ability).ToList();
            ActivateAbilities(abilities, source);

            foreach (var t in _hitAbilities)
            {
                // Decrease the counter
                var (ability, count) = t;
                count -= 1;
                if (count != 0) continue;
                
                // Remove the ability
                RevokeAbility(ability);
                EndCounterAbility(ability, source);
            }
            
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

        public void FinishPath()
        {
            ActivateAbilities(_finishAbilities, null);
            
            // Let our other systems know the enemy reached the end
            GameStats.lives -= deathLives;
            WaveSpawner.enemiesAlive--;
            GameStats.money += endPathMoney;
        
            Destroy(gameObject);
        }
        
        private void ActivateAbilities(IEnumerable<EnemyAbility> abilities, GameObject source)
        {
            foreach (var ability in abilities)
            {
                Debug.Log($"Activating Ability: {ability.name}");
                // Spawn ability effect
                var effect = Instantiate(ability.abilityEffect, transform.position, Quaternion.identity);
                Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
                switch (ability.targetingType)
                {
                    case AbilityTarget.Single:
                        // The target may be the damage source or the enemy itself
                        ability.Activate(source);
                        ability.Activate(gameObject);
                        break;
                    case AbilityTarget.Radius:
                        var colliders = Physics2D.OverlapCircleAll(transform.position, ability.range);
                        foreach (var coll in colliders)
                        {
                            if (!coll.CompareTag("Enemy") && !coll.CompareTag("Turret")) continue;
                            
                            ability.Activate(coll.gameObject);
                        }
                        break;
                    case AbilityTarget.All:
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
        
        private void EndCounterAbility(EnemyAbility ability, GameObject source)
        {
            // Spawn ability effect
            var effect = Instantiate(ability.abilityEffect, transform.position, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
            switch (ability.targetingType)
            {
                case AbilityTarget.Single:
                    // The target may be the damage source or the enemy itself
                    ability.OnCounterEnd(source);
                    ability.OnCounterEnd(gameObject);
                    break;
                case AbilityTarget.Radius:
                    var colliders = Physics2D.OverlapCircleAll(transform.position, ability.range);
                    foreach (var coll in colliders)
                    {
                        if (!coll.CompareTag("Enemy") && !coll.CompareTag("Turret")) continue;
                        
                        ability.OnCounterEnd(coll.gameObject);
                    }
                    break;
                case AbilityTarget.All:
                    var turrets = GameObject.FindGameObjectsWithTag("Turret");
                    foreach (var target in turrets)
                    {
                        ability.OnCounterEnd(target);
                    }
                    
                    var enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (var target in enemies)
                    {
                        ability.OnCounterEnd(target);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
