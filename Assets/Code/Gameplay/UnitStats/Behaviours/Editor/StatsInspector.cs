using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Code.Gameplay.UnitStats.Behaviours.Editor
{
	[CustomEditor(typeof(Stats))]
	public class StatsInspector : UnityEditor.Editor
	{
		private Stats _stats;
		private Dictionary<StatType, float> _baseStats;
		private FieldInfo _baseStatsField;

		private void OnEnable()
		{
			_stats = (Stats) target;

			_baseStatsField = typeof(Stats).GetField("_baseStats", BindingFlags.NonPublic | BindingFlags.Instance);
			if (_baseStatsField != null)
			{
				_baseStats = (Dictionary<StatType, float>) _baseStatsField.GetValue(_stats);
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (_baseStats == null || _baseStatsField == null)
			{
				EditorGUILayout.HelpBox("Failed to access _baseStats dictionary via reflection.", MessageType.Error);
				DrawDefaultInspector();
				return;
			}

			EditorGUILayout.LabelField("Base Stats", EditorStyles.boldLabel);

			EditorGUI.BeginChangeCheck();

            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                if (statType == StatType.Unknown) continue;

                if (_baseStats.TryGetValue(statType, out float baseValue))
                {
                    float modifiers = _stats.GetStatModifiersValue(statType);
                    float finalValue = _stats.GetStat(statType);

                    EditorGUILayout.BeginHorizontal();

                    // Base value (editable)
                    float newBaseValue = EditorGUILayout.FloatField(statType.ToString(), baseValue, GUILayout.Width(150));

                    if (!Mathf.Approximately(newBaseValue, baseValue))
                    {
                        _stats.SetBaseStat(statType, newBaseValue);
                        EditorUtility.SetDirty(_stats);
                    }

                    // Modifiers (read-only)
                    EditorGUILayout.LabelField($"Modifiers: {modifiers:+0.##;-0.##;0}", GUILayout.Width(100));

                    // Final Value (read-only)
                    EditorGUILayout.LabelField($"Total: {finalValue:0.##}", GUILayout.Width(100));

                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.LabelField($"{statType}: Not initialized in _baseStats");
                }
            }


            if (EditorGUI.EndChangeCheck())
			{
				_baseStats = (Dictionary<StatType, float>) _baseStatsField.GetValue(_stats);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}