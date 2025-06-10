// 6/7/2025 AI-Tag
// This custom inspector was generated with the help of Assistant, a Unity AI product.

using UnityEditor;
using UnityEngine;
using Code.Gameplay.Characters.Enemies.Configs;

[CustomEditor(typeof(DifficultyConfig))]
public class DifficultyConfigEditor : Editor
{
    private SerializedProperty _walkerSpawnChance;
    private SerializedProperty _skeletonSpawnChance;
    private SerializedProperty _bossSpawnChance;

    private Texture2D _walkerIcon;
    private Texture2D _skeletonIcon;
    private Texture2D _bossIcon;

    private void OnEnable()
    {
        _walkerSpawnChance = serializedObject.FindProperty("_walkerSpawnChance");
        _skeletonSpawnChance = serializedObject.FindProperty("_skeletonSpawnChance");
        _bossSpawnChance = serializedObject.FindProperty("_bossSpawnChance");

        LoadIcons();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw default inspector, except spawn chances section
        DrawPropertiesExcluding(serializedObject, "_walkerSpawnChance", "_skeletonSpawnChance", "_bossSpawnChance");

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Spawn Chances", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        DrawSpawnChanceWithIcon(_walkerIcon, "Walker", _walkerSpawnChance);
        DrawSpawnChanceWithIcon(_skeletonIcon, "Skeleton", _skeletonSpawnChance);
        DrawSpawnChanceWithIcon(_bossIcon, "Boss", _bossSpawnChance);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSpawnChanceWithIcon(Texture2D icon, string label, SerializedProperty chanceProp)
    {
        EditorGUILayout.BeginHorizontal();

        if (icon != null)
        {
            GUILayout.Label(icon, GUILayout.Width(64), GUILayout.Height(64));
        }
        else
        {
            EditorGUILayout.HelpBox($"Missing icon for {label}!", MessageType.Warning);
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField($"{label} Spawn Chance", EditorStyles.boldLabel);
        chanceProp.intValue = EditorGUILayout.IntSlider(chanceProp.intValue, 0, 100);

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);
    }

    private void LoadIcons()
    {
        _walkerIcon = LoadIcon("walker_icon.png");
        _skeletonIcon = LoadIcon("skeleton_icon.png");
        _bossIcon = LoadIcon("boss_icon.png");
    }

    private Texture2D LoadIcon(string fileName)
    {
        string path = $"Assets/Editor/Icons/{fileName}";
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

        if (texture == null)
        {
            Debug.LogWarning($"[DifficultyConfigEditor] Could not load icon at path: {path}");
        }

        return texture;
    }
}
