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
    [RequireComponent(typeof(Stats), typeof(Team), typeof(IDamageApplier))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _bounceRadius = 5f;
        [SerializeField] private float _destroyDelay = 0.1f;

        private Stats _stats;
        private Team _team;
        private IMovementDirectionProvider _movementDirectionProvider;
        private IDamageApplier _damageApplier;

        private int _enemiesPierced = 0;
        private int _bounceCount = 0;
        private bool _hasBounced = false;
        private bool _destroyScheduled = false;
        private Coroutine _destroyCoroutine = null;

        private List<GameObject> _hitEnemies = new List<GameObject>();

        private void Awake()
        {
            _stats = GetComponent<Stats>();
            _team = GetComponent<Team>();
            _movementDirectionProvider = GetComponent<IMovementDirectionProvider>();
            _damageApplier = GetComponent<IDamageApplier>();
        }

        private void OnEnable()
        {
            _damageApplier.OnDamageApplied += HandleDamageApplied;
        }

        private void OnDisable()
        {
            _damageApplier.OnDamageApplied -= HandleDamageApplied;
        }

        private void Start()
        {
            // Start destroy timer but keep reference
            _destroyCoroutine = StartCoroutine(DestroyAfterTimeout(6f));
        }

        private IEnumerator DestroyAfterTimeout(float delay)
        {
            yield return new WaitForSeconds(delay);

            // Destroy only if not already destroyed/scheduled
            if (!_destroyScheduled)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Team otherTeam) && otherTeam.Type != _team.Type)
            {
                if (_hitEnemies.Contains(other.gameObject))
                    return;

                _hitEnemies.Add(other.gameObject);

                if (other.TryGetComponent(out Health enemyHealth))
                {
                    float damage = _stats.GetStat(StatType.Damage);
                    enemyHealth.ApplyDamage(damage);
                }

                float bouncingValue = _stats.GetStat(StatType.Bounces);
                float piercingValue = _stats.GetStat(StatType.Piercing);

                // Bouncing logic
                if (bouncingValue >= 1 && !_hasBounced)
                {
                    GameObject nextTarget = FindNextBounceTarget(other.transform.position);

                    if (nextTarget != null)
                    {
                        // Cancel destroy coroutine since we are bouncing
                        if (_destroyCoroutine != null)
                        {
                            StopCoroutine(_destroyCoroutine);
                            _destroyCoroutine = null;
                        }

                        // If piercing is zero, set it to 1 temporarily for bouncing
                        if (piercingValue <= 0)
                        {
                            piercingValue = 1;
                        }

                        // Ensure pierced count at least 1 so it won't destroy prematurely
                        _enemiesPierced = Mathf.Max(_enemiesPierced, 1);

                        BounceToTarget(nextTarget);
                        return; // Bounce, don't destroy
                    }
                    else
                    {
                        // No bounce target found, destroy immediately if piercing is 0 or exceeded
                        if (piercingValue <= 0 || _enemiesPierced >= piercingValue)
                        {
                            Destroy(gameObject);
                            _destroyScheduled = true;
                            return;
                        }
                    }
                }

                // Normal piercing check (if no bounce or already bounced)
                if (ShouldDestroyAfterHit())
                {
                    Destroy(gameObject);
                    _destroyScheduled = true;
                }
            }
        }

        private void HandleDamageApplied(Health _)
        {
            float piercingValue = _stats.GetStat(StatType.Piercing);
            float bouncingValue = _stats.GetStat(StatType.Bounces);

            // Treat piercing as 1 temporarily if bouncing is active but piercing is 0
            if (bouncingValue >= 1 && piercingValue <= 0)
            {
                piercingValue = 1;
            }

            if (piercingValue <= 0 || _enemiesPierced >= piercingValue)
            {
                if (!_destroyScheduled)
                {
                    _destroyScheduled = true;
                    Destroy(gameObject, _destroyDelay);
                }
            }
        }

        private GameObject FindNextBounceTarget(Vector3 currentPosition)
        {
            Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(currentPosition, _bounceRadius);

            GameObject closestEnemy = null;
            float closestDistance = float.MaxValue;

            foreach (var collider in nearbyColliders)
            {
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

        private void BounceToTarget(GameObject target)
        {
            _bounceCount++;
            _hasBounced = true;

            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            if (_movementDirectionProvider != null)
            {
                _movementDirectionProvider.SetDirection(directionToTarget);
            }

            transform.rotation = Quaternion.FromToRotation(Vector3.up, directionToTarget);

            OnBounce();
        }

        protected virtual void OnBounce()
        {
            Debug.Log($"Projectile bounced! Bounce count: {_bounceCount}");
        }

        private bool ShouldDestroyAfterHit()
        {
            _enemiesPierced++;

            float piercingValue = _stats.GetStat(StatType.Piercing);
            float bouncingValue = _stats.GetStat(StatType.Bounces);

            if (bouncingValue >= 1 && piercingValue <= 0)
            {
                piercingValue = 1;
            }

            return piercingValue <= 0 || _enemiesPierced > piercingValue;
        }
    }
}
