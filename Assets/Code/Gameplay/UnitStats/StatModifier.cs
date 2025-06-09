using UnityEngine;

namespace Code.Gameplay.UnitStats
{
	public readonly struct StatModifier
	{
		public readonly StatType LinkedStatType;
		public readonly float Value;

		public StatModifier(StatType linkedStatType, float value)
		{
			LinkedStatType = linkedStatType;
			Value = value;
		}

		public override int GetHashCode()
		{
			return LinkedStatType.GetHashCode() ^ Value.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			StatModifier other = (StatModifier)obj;
			return LinkedStatType == other.LinkedStatType && Mathf.Approximately(Value, other.Value);
		}

		public static bool operator ==(StatModifier left, StatModifier right)
		{
			return left.LinkedStatType == right.LinkedStatType &&
				   Mathf.Approximately(left.Value, right.Value);
		}

		public static bool operator !=(StatModifier left, StatModifier right)
		{
			return left == right == false;
		}
	}
}