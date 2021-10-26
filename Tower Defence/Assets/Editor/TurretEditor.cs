using System;
using System.Collections.Generic;
using Turrets;
using Turrets.Upgrades.BulletUpgrades;
using Turrets.Upgrades.TurretUpgrades;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Turret))]
    public class TurretEditor : UnityEditor.Editor
    {
        private bool _showTurretUpgrades;
        private bool _showBulletUpgrades;

        public override void OnInspectorGUI()
        {
            var turret = (Turret) target;

            // Basic stats
            EditorGUILayout.PrefixLabel("Attributes");
            turret.range = EditorGUILayout.FloatField("Range", turret.range);
            turret.turnSpeed = EditorGUILayout.FloatField("Rotation Speed", turret.turnSpeed);

            // Bullet/Laser/Area attack data
            EditorGUILayout.Space();
            AddAttackTypeData(turret);

            // References data
            EditorGUILayout.Space();
            EditorGUILayout.PrefixLabel("References");
            turret.enemyTag = EditorGUILayout.TagField("Tag to Target", turret.enemyTag);
            turret.partToRotate = (Transform) EditorGUILayout.ObjectField("Part to rotate", turret.partToRotate,
                typeof(Transform), true);
            turret.firePoint = (Transform) EditorGUILayout.ObjectField("Fire Point", turret.firePoint,
                typeof(Transform), true);

            // Upgrades
            EditorGUILayout.Space();
            // EditorGUILayout.PrefixLabel("Upgrades");
            AddTurretUpgrades(turret.turretUpgrades, turret);
            AddBulletUpgrades(turret.bulletUpgrades);
        }

        private static void AddAttackTypeData(Turret turret)
        {
            EditorGUILayout.PrefixLabel("Turret Attack");
            turret.attackType = (TurretType) EditorGUILayout.EnumPopup("Attack Type", turret.attackType);

            switch (turret.attackType)
            {
                // Bullet Attack Stats
                case TurretType.Bullet:
                    EditorGUILayout.HelpBox("Fires a bullet prefab every fire rate seconds that homes in on an " +
                                            "enemy", MessageType.None);
                    turret.fireRate = EditorGUILayout.FloatField("Fire Rate", turret.fireRate);
                    turret.bulletPrefab = (GameObject) EditorGUILayout.ObjectField("Bullet Prefab",
                        turret.bulletPrefab, typeof(GameObject), false);
                    break;
                // Laser Attack Stats
                case TurretType.Laser:
                    EditorGUILayout.HelpBox("Fires a laser from the fire point to the enemy.", MessageType.None);
                    turret.damageOverTime = EditorGUILayout.FloatField("Damage every Second", turret.damageOverTime);
                    turret.lineRenderer = (LineRenderer) EditorGUILayout.ObjectField("Line Renderer",
                        turret.lineRenderer, typeof(LineRenderer), true);
                    turret.impactEffect = (ParticleSystem) EditorGUILayout.ObjectField("Impact Effect",
                        turret.impactEffect, typeof(GameObject), false);
                    break;
                // Area Attack Stats
                case TurretType.Area:
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("DO NOT USE! NO SETUP YET", MessageType.Error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddTurretUpgrades(IList<TurretUpgrade> upgrades, Turret turret)
        {
            _showTurretUpgrades = EditorGUILayout.BeginFoldoutHeaderGroup(_showTurretUpgrades, "Turret Upgrades");
            if (!_showTurretUpgrades)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }

            // Add current Upgrades
            var toRemove = new List<int>();
            for (var i = 0; i < upgrades.Count; i++)
            {
                if (upgrades[i] == null)
                {
                    toRemove.Add(i);
                }
                upgrades[i] = (TurretUpgrade) EditorGUILayout.ObjectField(upgrades[i], typeof(TurretUpgrade),
                    false);
            }
            
            // In case we want to add a new upgrade
            var newUpgrade = (TurretUpgrade) EditorGUILayout.ObjectField("Add Upgrade", null,
                typeof(TurretUpgrade), false);
            if (newUpgrade != null)
            {
                upgrades.Add(newUpgrade);
                turret.AddUpgrade(newUpgrade);
            }

            // Remove null upgrades
            toRemove.Reverse();
            foreach (var r in toRemove)
            {
                upgrades.RemoveAt(r);
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        
        private void AddBulletUpgrades(IList<BulletUpgrade> upgrades)
        {
            _showBulletUpgrades = EditorGUILayout.BeginFoldoutHeaderGroup(_showBulletUpgrades, "Bullet Upgrades");
            if (!_showBulletUpgrades)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }

            // Add current Upgrades
            var toRemove = new List<int>();
            for (var i = 0; i < upgrades.Count; i++)
            {
                if (upgrades[i] == null)
                {
                    toRemove.Add(i);
                }
                upgrades[i] = (BulletUpgrade) EditorGUILayout.ObjectField(upgrades[i], typeof(BulletUpgrade),
                    false);
            }
            
            // In case we want to add a new upgrade
            var newUpgrade = (BulletUpgrade) EditorGUILayout.ObjectField("Add Upgrade", null,
                typeof(BulletUpgrade), false);
            if (newUpgrade != null)
            {
                upgrades.Add(newUpgrade);
            }

            // Remove null upgrades
            toRemove.Reverse();
            foreach (var r in toRemove)
            {
                upgrades.RemoveAt(r);
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}