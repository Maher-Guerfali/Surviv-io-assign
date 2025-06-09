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

				if (_baseStats.TryGetValue(statType, out float currentValue))
				{
					float newValue = EditorGUILayout.FloatField(statType.ToString(), currentValue);

					if (!Mathf.Approximately(newValue, currentValue))
					{
						_stats.SetBaseStat(statType, newValue);
						EditorUtility.SetDirty(_stats);
					}
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