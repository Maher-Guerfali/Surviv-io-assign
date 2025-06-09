namespace Code.Gameplay.UnitStats
{
	public enum StatType
	{
		Unknown = 0,
		MaxHealth = 1,
		MovementSpeed = 2,
		Damage = 3,
		VisionRange = 4,
		RotationSpeed = 5,
		ProjectileSpeed = 6,
		ShootCooldown = 7,
		// XP System stats
		CurrentXP = 8,
		RequiredXP = 9,
		Level = 10,

		// New additions
		HealthPotionEffectiveness=11,   // Health Potions Boost
		Piercing=12,                    // Piercing Projectiles
		Bounces=13,                     // Bouncing Projectiles
		OrbitingProjectiles=14,        // Orbiting Projectiles (as a flag, 0 or 1)
	}
}
