using UnityEngine;

namespace Forklifter
{
	public class WheelController : MonoBehaviour
	{
		[SerializeField] private float wheelRadius;
		[SerializeField] private Transform wheel;

		public void Rotate(Vector3 velocity, float direction)
		{
			var actualSpeed = velocity.magnitude;
			var rad = actualSpeed / wheelRadius;

			wheel.Rotate(rad * Mathf.Rad2Deg * direction, 0f, 0f);
		}
	}
}