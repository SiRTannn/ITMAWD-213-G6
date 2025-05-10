using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// This allows scene references to be drag/dropped in the inspector
[System.Serializable]
public class SceneReference
{
    // Scene path used at runtime
    [SerializeField] private string scenePath = string.Empty;

    // ScenePath property - read only at runtime
    public string ScenePath
    {
        get { return scenePath; }
#if UNITY_EDITOR
        set { scenePath = value; }
#endif
    }

#if UNITY_EDITOR
    // Scene asset reference used only in editor
    [SerializeField] private Object sceneAsset;

    // OnValidate runs when inspector values change
    public void OnValidate()
    {
        // If scene asset is assigned, update the path
        if (sceneAsset != null)
        {
            string assetPath = AssetDatabase.GetAssetPath(sceneAsset);
            if (!string.IsNullOrEmpty(assetPath) && assetPath.EndsWith(".unity"))
            {
                scenePath = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            }
        }
    }

    // This enables the custom inspector for this type
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty sceneAssetProperty = property.FindPropertyRelative("sceneAsset");
            SerializedProperty scenePathProperty = property.FindPropertyRelative("scenePath");

            // Draw scene asset field
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.BeginChangeCheck();
            Object value = EditorGUI.ObjectField(position, label, sceneAssetProperty.objectReferenceValue, typeof(SceneAsset), false);
            if (EditorGUI.EndChangeCheck())
            {
                sceneAssetProperty.objectReferenceValue = value;
                if (value == null)
                {
                    scenePathProperty.stringValue = string.Empty;
                }
                else
                {
                    string assetPath = AssetDatabase.GetAssetPath(value);
                    if (!string.IsNullOrEmpty(assetPath) && assetPath.EndsWith(".unity"))
                    {
                        scenePathProperty.stringValue = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                    }
                }
            }

            // Draw scene path field (disabled)
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            GUI.enabled = false;
            EditorGUI.TextField(position, "Scene Path", scenePathProperty.stringValue);
            GUI.enabled = true;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        }
    }
#endif
}