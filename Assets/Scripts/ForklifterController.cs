using UnityEngine;

namespace Forklifter
{
	public class ForklifterController : MonoBehaviour
	{
		[SerializeField] private Rigidbody forkLifterRigidbody;
		[SerializeField] private Transform forklifterTransform;
		[SerializeField] private Transform centerOfMass;
		[SerializeField] private float forklifterSpeed;

		[Header("Rotation")]
		[SerializeField] private float maxRotation;
		[SerializeField] private float turnSpeed;
		[SerializeField] private float backRotation;

		[Header("Lift")]
		[SerializeField] private Transform fork;
		[SerializeField] private float minFrokPos;
		[SerializeField] private float maxFrokPos;
		[SerializeField] private float forkSpeed;

		private float rotation;

		private void Start()
		{
			forkLifterRigidbody.centerOfMass = centerOfMass.localPosition;
		}

		private void FixedUpdate()
		{
			var vertical = Input.GetAxis("Vertical");
			var horizontal = Input.GetAxis("Horizontal");
			var dir = 0f;

			if (Input.GetKey(KeyCode.PageDown))
				dir = -1f;
			if (Input.GetKey(KeyCode.PageUp))
				dir = 1f;

			Move(vertical, horizontal);
			MoveFork(dir);
		}

		public void Move(float vertical, float horizontal)
		{
			float time = Time.fixedDeltaTime;

			if (horizontal > 0.2f || horizontal < -0.2f)
			{
				rotation += turnSpeed * horizontal * time;
				rotation = Mathf.Clamp(rotation, -maxRotation, maxRotation);
			}
			else
			{
				rotation = Mathf.Lerp(rotation, 0f, time * backRotation);
			}

			if (vertical > 0.2f || vertical < -0.2f)
			{
				forkLifterRigidbody.angularVelocity = new Vector3(0, rotation * vertical * time, 0);

				var speed = forklifterSpeed * vertical;
				forkLifterRigidbody.velocity = forklifterTransform.forward * speed;
			}
		}

		public void MoveFork(float dir)
		{
			var forkPosition = fork.localPosition;

			forkPosition.y += forkSpeed * Time.fixedDeltaTime * dir;
			forkPosition.y = Mathf.Clamp(forkPosition.y, minFrokPos, maxFrokPos);

			fork.localPosition = forkPosition;
		}
	}
}