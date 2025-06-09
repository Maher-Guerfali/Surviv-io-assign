using Code.Gameplay.Lifetime.Behaviours;
using Code.Gameplay.Teams.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Code.Gameplay.Movement.Behaviours;
using Code.Gameplay.Characters.Enemies.Behaviours;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Code.Gameplay.Projectiles.Behaviours
{
    [RequireComponent(typeof(Stats), typeof(Team))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _bounceRadius = 5f; // Radius to search for next enemy

        private Stats _stats;
        private Team _team;
        private IMovementDirectionProvider _movementDirectionProvider;
        private int _enemiesPierced = 0;
        private int _bounceCount = 0;
        private bool _hasBounced = false;
        private List<GameObject> _hitEnemies = new List<GameObject>(); // Track hit enemies to avoid re-hitting

        public int EnemiesPierced => _enemiesPierced;
        public bool HasBounced => _hasBounced;

        private void Awake()
        {
            _stats = GetComponent<Stats>();
            _team = GetComponent<Team>();
            _movementDirectionProvider = GetComponent<IMovementDirectionProvider>();
        }

        private void Start()
        {
            StartCoroutine(DestroyAfterTimeout(4f));
        }

        private IEnumerator DestroyAfterTimeout(float delay)
        {
            yield return new WaitForSeconds(delay);

          

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if we hit an enemy (different team)
            if (other.TryGetComponent(out Team otherTeam) && otherTeam.Type != _team.Type)
            {
                // Skip if we already hit this enemy
                if (_hitEnemies.Contains(other.gameObject))
                    return;

                // Add to hit enemies list
                _hitEnemies.Add(other.gameObject);

                // Apply damage to the enemy
                if (other.TryGetComponent(out Health enemyHealth))
                {
                    float damage = _stats.GetStat(StatType.Damage);
                    enemyHealth.ApplyDamage(damage);
                }

                // Check if we should bounce to another enemy
                if (ShouldBounce())
                {
                    GameObject nextTarget = FindNextBounceTarget(other.transform.position);
                    if (nextTarget != null)
                    {
                        BounceToTarget(nextTarget);
                        return; // Don't destroy, we're bouncing
                    }
                }

                // Check if projectile should be destroyed or continue piercing
                if (ShouldDestroyAfterHit())
                {
                    Destroy(gameObject);
                }
            }
        }

        /// <summary>
        /// Determines if projectile should bounce after hitting an enemy
        /// </summary>
        private bool ShouldBounce()
        {
            // Check if we have bouncing stat and haven't bounced yet
            float bouncingValue = _stats.GetStat(StatType.Bounces); // You'll need to add this StatType
            return bouncingValue > 0 && _bounceCount < bouncingValue;
        }

        /// <summary>
        /// Finds the closest enemy within bounce radius to bounce to
        /// </summary>
        private GameObject FindNextBounceTarget(Vector3 currentPosition)
        {
            // Find all colliders within bounce radius
            Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(currentPosition, _bounceRadius);

            GameObject closestEnemy = null;
            float closestDistance = float.MaxValue;

            foreach (var collider in nearbyColliders)
            {
                // Check if it's an enemy we haven't hit yet
                if (collider.TryGetComponent(out Team otherTeam) &&
                    otherTeam.Type != _team.Type &&
                    collider.TryGetComponent(out Enemy enemy) &&
                    !_hitEnemies.Contains(collider.gameObject))
                {
                    float distance = Vector3.Distance(currentPosition, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = collider.gameObject;
                    }
                }
            }

            return closestEnemy;
        }

        /// <summary>
        /// Bounces the projectile towards the target enemy
        /// </summary>
        private void BounceToTarget(GameObject target)
        {
            _bounceCount++;
            _hasBounced = true;

            // Calculate direction to target
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            // Update projectile direction
            if (_movementDirectionProvider != null)
            {
                _movementDirectionProvider.SetDirection(directionToTarget);
            }

            // Rotate projectile to face new direction
            transform.rotation = Quaternion.FromToRotation(Vector3.up, directionToTarget);

            // Optional: Add some visual feedback for bouncing
            OnBounce();
        }

        /// <summary>
        /// Called when projectile bounces - override for visual/audio effects
        /// </summary>
        protected virtual void OnBounce()
        {
            // Add particle effects, sound, etc. here
            Debug.Log($"Projectile bounced! Bounce count: {_bounceCount}");
        }

        /// <summary>
        /// Determines if projectile should be destroyed after hitting an enemy
        /// </summary>
        private bool ShouldDestroyAfterHit()
        {
            _enemiesPierced++;
            float piercingValue = _stats.GetStat(StatType.Piercing);

            // If piercing value is 0 or we've pierced enough enemies, destroy the projectile
            return piercingValue <= 0 || _enemiesPierced > piercingValue;
        }

        /// <summary>
        /// Gets damage value from stats
        /// </summary>
        public float GetDamage()
        {
            return _stats.GetStat(StatType.Damage);
        }

        /// <summary>
        /// Gets bounce count for debugging/UI
        /// </summary>
        public int GetBounceCount()
        {
            return _bounceCount;
        }

    
    }
}