using UnityEngine;

namespace Code.Gameplay.UnitStats
{
    public readonly struct StatModifier
    {
        public readonly StatType LinkedStatType;
        public readonly float Value;
        public readonly ModifierMode Mode;

        public StatModifier(StatType linkedStatType, float value, ModifierMode mode)
        {
            LinkedStatType = linkedStatType;
            Value = value;
            Mode = mode;
        }

        public override int GetHashCode()
        {
            return LinkedStatType.GetHashCode() ^ Value.GetHashCode() ^ Mode.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is StatModifier other)
            {
                return LinkedStatType == other.LinkedStatType &&
                       Mathf.Approximately(Value, other.Value) &&
                       Mode == other.Mode;
            }
            return false;
        }

        public static bool operator ==(StatModifier left, StatModifier right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StatModifier left, StatModifier right)
        {
            return !(left == right);
        }
    }

    public enum ModifierMode
    {
        Add,
        Multiply,
        Set
    }
}
