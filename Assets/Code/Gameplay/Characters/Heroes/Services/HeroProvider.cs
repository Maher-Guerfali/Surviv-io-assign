using Code.Gameplay.Lifetime.Behaviours;
using Code.Gameplay.UnitStats.Behaviours;

namespace Code.Gameplay.Characters.Heroes.Services
{
	public class HeroProvider : IHeroProvider
	{
		public Behaviours.Hero Hero { get; private set; }
		public Health Health { get; private set; }
		public Stats Stats { get; private set; }
		
		public void SetHero(Behaviours.Hero hero)
		{
			Hero = hero;
			Health = hero.GetComponent<Health>();
			Stats = hero.GetComponent<Stats>();
		}
	}
}