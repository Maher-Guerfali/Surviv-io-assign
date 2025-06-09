using Code.Gameplay.Teams.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using UnityEngine;
using System.Collections;
using Code.Gameplay.Lifetime.Behaviours;

namespace Code.Gameplay.Projectiles.Behaviours
{
    public class OrbitingProjectileSystem : MonoBehaviour
    {
        [Header("Orbit Settings")]
        [SerializeField] private float _orbitRadius = 2f;
        [SerializeField] private float _orbitSpeed = 90f; // degrees per second
        [SerializeField] private float _respawnDelay = 3f;

        [Header("Projectile Setup")]
        [SerializeField] private GameObject _projectilePrefab;

        private OrbitingProjectile[] _orbitingProjectiles = new OrbitingProjectile[3];
        private bool[] _projectileActive = new bool[3] { true, true, true };
        private float _currentAngle = 0f;

        private Stats _ownerStats;
        private Team _ownerTeam;

        private void Awake()
        {
            _ownerStats = GetComponentInParent<Stats>();
            _ownerTeam = GetComponentInParent<Team>();

            CreateOrbitingProjectiles();
        }

        private void Start()
        {
            // Start orbiting
            enabled = true;
        }

        private void Update()
        {
            UpdateOrbitPositions();
        }

        /// <summary>
        /// Creates the 3 orbiting projectiles as child objects
        /// </summary>
        private void CreateOrbitingProjectiles()
        {
            for (int i = 0; i < 3; i++)
            {
                // Create projectile as child
                GameObject projectileObj = Instantiate(_projectilePrefab, transform);
                projectileObj.name = $"OrbitingProjectile_{i + 1}";

                // Get or add OrbitingProjectile component
                OrbitingProjectile orbitingProjectile = projectileObj.GetComponent<OrbitingProjectile>();
                if (orbitingProjectile == null)
                {
                    orbitingProjectile = projectileObj.AddComponent<OrbitingProjectile>();
                }

                // Setup the projectile
                orbitingProjectile.Initialize(this, i);
                _orbitingProjectiles[i] = orbitingProjectile;

                // Set initial position
                UpdateProjectilePosition(i);
            }
        }

        /// <summary>
        /// Updates the positions of all active orbiting projectiles
        /// </summary>
        private void UpdateOrbitPositions()
        {
            // Update orbit angle
            _currentAngle += _orbitSpeed * Time.deltaTime;
            if (_currentAngle >= 360f)
                _currentAngle -= 360f;

            // Update positions for all active projectiles
            for (int i = 0; i < 3; i++)
            {
                if (_projectileActive[i])
                {
                    UpdateProjectilePosition(i);
                }
            }
        }

        /// <summary>
        /// Updates position for a specific projectile
        /// </summary>
        private void UpdateProjectilePosition(int index)
        {
            if (_orbitingProjectiles[index] == null) return;

            // Calculate angle for this projectile (120 degrees apart)
            float projectileAngle = _currentAngle + (index * 120f);
            float radians = projectileAngle * Mathf.Deg2Rad;

            // Calculate position around parent
            Vector3 offset = new Vector3(
                Mathf.Cos(radians) * _orbitRadius,
                Mathf.Sin(radians) * _orbitRadius,
                0f
            );

            _orbitingProjectiles[index].transform.localPosition = offset;

            // Optional: Rotate projectile to face movement direction
            _orbitingProjectiles[index].transform.rotation = Quaternion.FromToRotation(Vector3.up, offset.normalized);
        }

        /// <summary>
        /// Called when one of the orbiting projectiles hits an enemy
        /// </summary>
        public void OnProjectileHit(int projectileIndex, Collider2D hitTarget)
        {
            // Check if target is an enemy
            if (hitTarget.TryGetComponent(out Team targetTeam) && targetTeam.Type != _ownerTeam.Type)
            {
                // Apply damage
                if (hitTarget.TryGetComponent(out Health targetHealth) && _ownerStats != null)
                {
                    float damage = _ownerStats.GetStat(StatType.Damage);
                    targetHealth.ApplyDamage(damage);
                }

                // Disable projectile and start respawn timer
                DisableProjectile(projectileIndex);
                StartCoroutine(RespawnProjectile(projectileIndex));
            }
        }

        /// <summary>
        /// Disables a projectile temporarily
        /// </summary>
        private void DisableProjectile(int index)
        {
            if (index >= 0 && index < 3)
            {
                _projectileActive[index] = false;
                if (_orbitingProjectiles[index] != null)
                {
                    _orbitingProjectiles[index].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Re-enables a projectile after delay
        /// </summary>
        private void EnableProjectile(int index)
        {
            if (index >= 0 && index < 3)
            {
                _projectileActive[index] = true;
                if (_orbitingProjectiles[index] != null)
                {
                    _orbitingProjectiles[index].gameObject.SetActive(true);
                    UpdateProjectilePosition(index); // Update position immediately
                }
            }
        }

        /// <summary>
        /// Coroutine to handle projectile respawn timing
        /// </summary>
        private IEnumerator RespawnProjectile(int index)
        {
            yield return new WaitForSeconds(_respawnDelay);
            EnableProjectile(index);
        }

        /// <summary>
        /// Enables or disables the entire orbiting system
        /// </summary>
        public void SetOrbitingActive(bool active)
        {
            enabled = active;

            for (int i = 0; i < 3; i++)
            {
                if (_orbitingProjectiles[i] != null)
                {
                    _orbitingProjectiles[i].gameObject.SetActive(active);
                    _projectileActive[i] = active;
                }
            }
        }

        /// <summary>
        /// Gets current damage value for orbiting projectiles
        /// </summary>
        public float GetDamage()
        {
            return _ownerStats?.GetStat(StatType.Damage) ?? 0f;
        }

        // Debug visualization in scene view
       
    }
}