using UnityEngine;

namespace Code.Gameplay.Characters.Enemies.Configs
{
    [CreateAssetMenu(fileName = "DifficultyConfig", menuName = Constants.GameName + "/Configs/Difficulty")]
    public class DifficultyConfig : ScriptableObject
    {
        [Header("Difficulty Scaling Mode")]
        [SerializeField] private bool _scalePerSecond = true;
        [SerializeField] private bool _scalePerLevel = false;

        [Header("Time-based Scaling (when scalePerSecond = true)")]
        [SerializeField] private float _timeIntervalSeconds = 30f;
        [SerializeField] private float _hpIncreasePerInterval = 10f;
        [SerializeField] private float _damageIncreasePerInterval = 5f;

        [Header("Level-based Scaling (when scalePerLevel = true)")]
        [SerializeField] private float _hpIncreasePerLevel = 15f;
        [SerializeField] private float _damageIncreasePerLevel = 8f;

        [Header("Spawn Chances (0-100%)")]
        [SerializeField] [Range(0, 100)] private int _walkerSpawnChance = 70;
        [SerializeField] [Range(0, 100)] private int _skeletonSpawnChance = 20;
        [SerializeField] [Range(0, 100)] private int _bossSpawnChance = 5;

        [Header("Boss Spawn Cooldown")]
        [SerializeField] private float _bossSpawnCooldown = 30f;

        [Header("Spawn Rate")]
        [SerializeField] private AnimationCurve _spawnIntervalCurve = AnimationCurve.Linear(0f, 3f, 300f, 1f);

        [Header("Debug Info")]
        [SerializeField]
        [TextArea(5, 10)]
        private string _designerNotes =
            "DIFFICULTY SCALING MODES:\n\n" +
            "TIME-BASED (scalePerSecond = true):\n" +
            "- Every 'timeIntervalSeconds', enemy HP and Damage increase\n" +
            "- HP increases by 'hpIncreasePerInterval'\n" +
            "- Damage increases by 'damageIncreasePerInterval'\n\n" +
            "LEVEL-BASED (scalePerLevel = true):\n" +
            "- Every time player levels up, enemy HP and Damage increase\n" +
            "- HP increases by 'hpIncreasePerLevel'\n" +
            "- Damage increases by 'damageIncreasePerLevel'\n" +
            "You can enable both modes simultaneously for compound scaling."+
              "- _spawnIntervalCurve Y = how many sec delay to spawn 1 enemy, X = time ,game start at x=0, x=500 mean at 5min of gameplay '\n" +
            "mean at game start enemies spawn each 3 sec, and 5 min enemies spawn each 1 sec.";

        // Properties for external access
        public bool ScalePerSecond => _scalePerSecond;
        public bool ScalePerLevel => _scalePerLevel;
        public float TimeIntervalSeconds => _timeIntervalSeconds;
        public float HpIncreasePerInterval => _hpIncreasePerInterval;
        public float DamageIncreasePerInterval => _damageIncreasePerInterval;
        public float HpIncreasePerLevel => _hpIncreasePerLevel;
        public float DamageIncreasePerLevel => _damageIncreasePerLevel;

        /// <summary>
        /// Calculates HP multiplier based on time and/or level
        /// </summary>
        public float EvaluateHpMultiplier(float elapsedTime, int playerLevel = 1)
        {
            float multiplier = 1f;

            if (_scalePerSecond && _timeIntervalSeconds > 0)
            {
                int timeIntervals = Mathf.FloorToInt(elapsedTime / _timeIntervalSeconds);
                multiplier += (timeIntervals * _hpIncreasePerInterval);
            }

            if (_scalePerLevel && playerLevel > 1)
            {
                int levelsGained = playerLevel - 1;
                multiplier += (levelsGained * _hpIncreasePerLevel);
            }

            return Mathf.Max(1f, multiplier);
        }

        /// <summary>
        /// Calculates damage multiplier based on time and/or level
        /// </summary>
        public float EvaluateDamageMultiplier(float elapsedTime, int playerLevel = 1)
        {
            float multiplier = 1f;

            if (_scalePerSecond && _timeIntervalSeconds > 0)
            {
                int timeIntervals = Mathf.FloorToInt(elapsedTime / _timeIntervalSeconds);
                multiplier += (timeIntervals * _damageIncreasePerInterval);
            }

            if (_scalePerLevel && playerLevel > 1)
            {
                int levelsGained = playerLevel - 1;
                multiplier += (levelsGained * _damageIncreasePerLevel);
            }

            return Mathf.Max(1f, multiplier);
        }

        /// <summary>
        /// Evaluates Walker spawn chance (0-1) based on percentage setting
        /// </summary>
        public float EvaluateWalkerSpawnChance(float elapsedTime)
        {
            return _walkerSpawnChance / 100f;
        }

        /// <summary>
        /// Evaluates Skeleton spawn chance (0-1) based on percentage setting
        /// </summary>
        public float EvaluateSkeletonSpawnChance(float elapsedTime)
        {
            return _skeletonSpawnChance / 100f;
        }

        /// <summary>
        /// Evaluates Boss spawn chance (0-1) based on percentage setting
        /// </summary>
        public float EvaluateBossSpawnChance(float elapsedTime)
        {
            return _bossSpawnChance / 100f;
        }

        /// <summary>
        /// Gets the spawn interval in seconds based on elapsed time
        /// </summary>
        public float EvaluateSpawnInterval(float elapsedTime)
        {
            return Mathf.Max(0.5f, _spawnIntervalCurve.Evaluate(elapsedTime));
        }

        /// <summary>
        /// Gets the boss spawn cooldown duration
        /// </summary>
        public float BossSpawnCooldown => _bossSpawnCooldown;

#if UNITY_EDITOR
        [Header("Runtime Debug (Read-only)")]
        [SerializeField] private float _debugElapsedTime;
        [SerializeField] private int _debugPlayerLevel;
        [SerializeField] private float _debugHpMultiplier;
        [SerializeField] private float _debugDamageMultiplier;
        [SerializeField] private float _debugWalkerChance;
        [SerializeField] private float _debugSkeletonChance;
        [SerializeField] private float _debugBossChance;

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                _debugElapsedTime = Time.timeSinceLevelLoad;
               
                _debugPlayerLevel = 1;
                _debugHpMultiplier = EvaluateHpMultiplier(_debugElapsedTime, _debugPlayerLevel);
                _debugDamageMultiplier = EvaluateDamageMultiplier(_debugElapsedTime, _debugPlayerLevel);
                _debugWalkerChance = _walkerSpawnChance;
                _debugSkeletonChance = _skeletonSpawnChance;
                _debugBossChance = _bossSpawnChance;
            }
        }
#endif
    }
}