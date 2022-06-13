using UnityEngine;

namespace Forklifter
{
	public class InputController : MonoBehaviour
    {
        [SerializeField] private ForklifterController forklifter;

		private void FixedUpdate()
		{
			var vertical = Input.GetAxis("Vertical");
			var horizontal = Input.GetAxis("Horizontal");
			var dir = 0f;
			var deltaTime = Time.fixedDeltaTime;

			if (Input.GetKey(KeyCode.PageDown))
				dir = -1f;
			if (Input.GetKey(KeyCode.PageUp))
				dir = 1f;

			forklifter.Move(vertical, horizontal, deltaTime);
			forklifter.MoveFork(dir, deltaTime);
		}
	}
}