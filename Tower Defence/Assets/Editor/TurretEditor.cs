using Turrets;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Turret))]
    public class TurretEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Turret turret = (Turret)target;
            
            // Basic stats
            EditorGUILayout.PrefixLabel("Attributes");
            turret.range = EditorGUILayout.FloatField("Range", turret.range);
            turret.turnSpeed = EditorGUILayout.FloatField("Rotation Speed", turret.turnSpeed);

            EditorGUILayout.Space();
            EditorGUILayout.PrefixLabel("Turret Attack");
            turret.attackType = (TurretType) EditorGUILayout.EnumPopup("Attack Type", turret.attackType);

            // Bullet Attack Stats
            if (turret.attackType == TurretType.Bullet)
            {
                EditorGUILayout.HelpBox("Fires a bullet prefab every fire rate seconds that homes in on an " +
                                        "enemy", MessageType.None);
                turret.fireRate = EditorGUILayout.FloatField("Fire Rate", turret.fireRate);
                turret.bulletPrefab = (GameObject) EditorGUILayout.ObjectField("Bullet Prefab",
                    turret.bulletPrefab, typeof(GameObject), false);
            }
            // Laser Attack Stats
            else if (turret.attackType == TurretType.Laser)
            {
                EditorGUILayout.HelpBox("Fires a laser from the fire point to the enemy.", MessageType.None);
                turret.damageOverTime = EditorGUILayout.FloatField("Damage every Second", turret.damageOverTime);
                turret.lineRenderer = (LineRenderer) EditorGUILayout.ObjectField("Line Renderer",
                    turret.lineRenderer, typeof(LineRenderer), true);
                turret.impactEffect = (ParticleSystem) EditorGUILayout.ObjectField("Impact Effect",
                    turret.impactEffect, typeof(GameObject), false);
            }
            // Area Attack Stats
            else if (turret.attackType == TurretType.Area)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("DO NOT USE! NO SETUP YET", MessageType.Error);
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.PrefixLabel("References");
            turret.enemyTag = EditorGUILayout.TagField("Tag to Target", turret.enemyTag);
            turret.partToRotate = (Transform) EditorGUILayout.ObjectField("Part to rotate", turret.partToRotate,
                typeof(Transform), true);
            turret.firePoint = (Transform) EditorGUILayout.ObjectField("Fire Point", turret.firePoint,
                typeof(Transform), true);
        }

        // Custom GUILayout progress bar.
        // void ProgressBar (float value, string label)
        // {
        //     // Get a rect for the progress bar using the same margins as a textfield:
        //     Rect rect = GUILayoutUtility.GetRect (18, 18, "TextField");
        //     EditorGUI.ProgressBar (rect, value, label);
        //     EditorGUILayout.Space ();
        // }
    }
}