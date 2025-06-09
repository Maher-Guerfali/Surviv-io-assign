using UnityEngine;

namespace Code.Gameplay.Projectiles.Behaviours
{
    [RequireComponent(typeof(Collider2D))]
    public class OrbitingProjectile : MonoBehaviour
    {
        private OrbitingProjectileSystem _parentSystem;
        private int _projectileIndex;
        private Collider2D _collider;

        /// <summary>
        /// Initialize this orbiting projectile
        /// </summary>
        public void Initialize(OrbitingProjectileSystem parentSystem, int index)
        {
            _parentSystem = parentSystem;
            _projectileIndex = index;
        }

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();

            // Ensure collider is set as trigger
            if (_collider != null)
            {
                _collider.isTrigger = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Only process collision if we have a parent system
            if (_parentSystem != null)
            {
                _parentSystem.OnProjectileHit(_projectileIndex, other);
            }
        }

        /// <summary>
        /// Gets the projectile index for debugging
        /// </summary>
        public int GetProjectileIndex()
        {
            return _projectileIndex;
        }

        /// <summary>
        /// Gets reference to parent system
        /// </summary>
        public OrbitingProjectileSystem GetParentSystem()
        {
            return _parentSystem;
        }

        // Optional: Add visual feedback when projectile is about to hit
        private void OnTriggerStay2D(Collider2D other)
        {
            // Could add particle effects, glowing, etc. here
        }
    }    }