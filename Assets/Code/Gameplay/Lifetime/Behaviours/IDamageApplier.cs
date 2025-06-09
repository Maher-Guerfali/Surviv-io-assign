using System;

namespace Code.Gameplay.Lifetime.Behaviours
{
	public interface IDamageApplier
	{
		event Action<Health> OnDamageApplied;
	}
}