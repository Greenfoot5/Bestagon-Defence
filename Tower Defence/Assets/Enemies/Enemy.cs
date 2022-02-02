﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abstract;
using Abstract.Managers;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Upgrades.Abilities;

namespace Enemies
{
    /// <summary>
    /// The base skeleton for the enemy, holding it's stats and abilities
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        [Header("Stats")]
        public float startSpeed = 2f;
        public float maxHealth = 20f;

        [ReadOnly]
        public float speed;
        [ReadOnly]
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
        
        /// <summary>
        /// Grants abilities and sets current stats to max when spawning the enemy
        /// </summary>
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
        
        /// <summary>
        /// Grants the enemy an ability so they can use it when triggered
        /// </summary>
        /// <param name="ability">The ability to grant</param>
        public void GrantAbility(EnemyAbility ability)
        {
            // TODO - Have better ability checking. Perhaps check if one is a higher tier than another,
            // TODO - or, for an extra challenge, reset the timer on the current one.
            var existingIcon = iconLayout.transform.Find($"{ability.name} Icon");
            if (existingIcon != null)
            {
                return;
            }
            
            // Adds to ability to the lists based on it's trigger(s)
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
            
            // Adds the icon above the enemy's health bar
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
        
        /// <summary>
        /// Removes an ability from the enemy
        /// </summary>
        /// <param name="ability">The ability to remove</param>
        private void RevokeAbility(EnemyAbility ability)
        {
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
            // Check the enemy still has the ability to use
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
    
        /// <summary>
        /// Called whenever the enemy takes damage.
        /// This activates any ability with the OnDamage trigger
        /// </summary>
        /// <param name="amount">The amount of damage to deal</param>
        /// <param name="source">The GameObject that hurt the enemy</param>
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
            
            // Edit the health
            health -= amount;

            leftBar.fillAmount = health / maxHealth;
            rightBar.fillAmount = health / maxHealth;

            if (health <= 0)
            {
                Die(gameObject);
            }
        }
        
        /// <summary>
        /// Called whenever the enemy takes damage
        /// but the source doesn't want OnDamage abilities to activate.
        /// For example, when taking effect damage.
        /// </summary>
        /// <param name="amount">The amount of damage to deal</param>
        public void TakeDamageWithoutAbilities(float amount)
        {
            // Base health stuff
            health -= amount;

            leftBar.fillAmount = health / maxHealth;
            rightBar.fillAmount = health / maxHealth;

            if (health <= 0)
            {
                Die(gameObject);
            }
        }

        /// <summary>
        /// Called when the enemy dies
        /// allows the game to clean up anything when removing the GameObject
        /// </summary>
        /// <param name="source">What killed the enemy</param>
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