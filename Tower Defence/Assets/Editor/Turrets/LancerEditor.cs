﻿using Turrets.Lancer;
using UnityEditor;

namespace Editor.Turrets
{
    [CustomEditor(typeof(Lancer), true)]
    public class LancerEditor : TurretEditor
    {
        // PROPERTIES
        private SerializedProperty _partToRotate;

        private SerializedProperty _firePoint;
        
        private SerializedProperty _bulletPrefab;
        
        protected new void OnEnable()
        {
            base.OnEnable();
            
            _partToRotate = serializedObject.FindProperty("partToRotate");
            _firePoint = serializedObject.FindProperty("firePoint");
            _bulletPrefab = serializedObject.FindProperty("bulletPrefab");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_partToRotate);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Reference", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_firePoint);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Lancer", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_bulletPrefab);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
