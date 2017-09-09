using UnityEngine;

namespace BeatThat
{
	/// <summary>
	/// Extension methods for Unity's core classes, e.g. Component
	/// </summary>
	public static class BoundsUtils
	{

		/// <summary>
		/// For the case where you want to start a bounds and grow it via encapsulating a sequence of additional bounds objects.
		/// When the bounds has *some* positve size, then just calls Bounds::Encapsulate.
		/// However, if the bounds has size of (0,0,0), e.g. if it is just a 'new Bounds()',
		/// then calling this function will cause this bounds to make itself a copy of the passed-in bounds
		/// (as opposed to encapsulating the passed in bounds and the existing (bogus) center point of (0,0,0)
		/// </summary>
		private static void EncapsulateOrInit(ref Bounds b, Bounds toEncap)
		{
			if(Mathf.Approximately(toEncap.size.sqrMagnitude, 0f)) {
				// bounds has no size. conceivablely you could use it's center position and extend bounds to encap that point, but unlikely that would be desired
				return;
			}

			if(Mathf.Approximately(toEncap.size.sqrMagnitude, 0f)) {
				b.center = toEncap.center;
				b.size = toEncap.size;
			}
			else {
				b.Encapsulate(toEncap);
			}
		}

		public static Bounds EstimateBounds(this GameObject go)
		{
			return go.transform.EstimateBounds();
		}

		public static Bounds EstimateBounds(this Component c)
		{
			var b = new Bounds();
			c.Encapsulate(ref b);
			return b;
		}

		public static void Encapsulate(this GameObject go, ref Bounds bounds)
		{
			go.transform.Encapsulate(ref bounds);
		}

		public static void Encapsulate(this Component c, ref Bounds bounds)
		{
			var b = c.GetComponent<IHasBounds>();
			if(b != null) {
				EncapsulateOrInit(ref bounds, b.bounds);
			}

			var col = c.GetComponent<Collider>();
			if(col != null) {
				EncapsulateOrInit(ref bounds, col.bounds);
			}

			var r = c.GetComponent<Renderer>();
			if(r != null) {
				EncapsulateOrInit(ref bounds, r.bounds);
			}

			var pl = c.GetComponent<Light>();
			if(pl != null) {
				EncapsulateOrInit(ref bounds, new Bounds(pl.transform.position, new Vector3(pl.range, pl.range, pl.range)));
			}

			var t = c.transform;

			for(int i = 0; i < t.childCount; i++) {
				Encapsulate(t.GetChild(i), ref bounds);
			}
		}

	}
}
