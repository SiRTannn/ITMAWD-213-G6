#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

[System.Serializable]
public class NPCSceneReference
{
    [SerializeField] private string scenePath = string.Empty;
    public string ScenePath => scenePath;

#if UNITY_EDITOR
    [SerializeField] private Object sceneAsset;

    public void OnValidate()
    {
        if (sceneAsset != null)
        {
            string assetPath = AssetDatabase.GetAssetPath(sceneAsset);
            if (!string.IsNullOrEmpty(assetPath) && assetPath.EndsWith(".unity"))
            {
                scenePath = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            }
        }
    }

    [CustomPropertyDrawer(typeof(NPCSceneReference))]
    public class NPCSceneReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty sceneAssetProperty = property.FindPropertyRelative("sceneAsset");
            SerializedProperty scenePathProperty = property.FindPropertyRelative("scenePath");

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