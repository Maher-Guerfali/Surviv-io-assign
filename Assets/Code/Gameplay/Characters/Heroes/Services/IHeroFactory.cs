using UnityEngine;

namespace Code.Gameplay.Characters.Heroes.Services
{
	public interface IHeroFactory
	{
		Behaviours.Hero CreateHero(Vector3 at, Quaternion rotation);
	}
}