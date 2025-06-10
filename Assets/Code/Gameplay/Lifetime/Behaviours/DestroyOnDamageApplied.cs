using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Lifetime.Behaviours
{
	[RequireComponent(typeof(IDamageApplier))]
	public class DestroyOnDamageApplied : MonoBehaviour
	{
		[SerializeField] private float _delay;
		
		private IDamageApplier _damageApplier;

		private void Awake()
		{
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

        private void HandleDamageApplied(Health _)
        {
            var projectile = GetComponent<Code.Gameplay.Projectiles.Behaviours.Projectile>();

            if (projectile != null)
            {
                float piercingValue = projectile.GetComponent<Stats>().GetStat(StatType.Piercing);

                if (piercingValue <= 0)
                {
                    Destroy(gameObject, _delay);
                }
                else
                {
                    // Do NOT destroy — projectile will handle its own destruction
                }
            }
            else
            {
                // Not a projectile → fallback to default behavior
                Destroy(gameObject, _delay);
            }
        }

    }
}