using UnityEngine;

namespace Code.Common.Extensions
{
	public static class LayerExtensions
	{
		public static bool Matches(this Collider2D collider, LayerMask layerMask) => ((1 << collider.gameObject.layer) & layerMask) != 0;
		public static bool Matches(this int thisLayer, int otherLayer) => (thisLayer & otherLayer) != 0;
	}
}