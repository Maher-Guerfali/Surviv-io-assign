using UnityEngine;

namespace Code.Gameplay.Identification.Behaviours
{
	public class Id : MonoBehaviour
	{
		[SerializeField] private int _value;
		
		public int Value => _value;

		public void Setup(int id)
		{
			_value = id;
		}
	}
}