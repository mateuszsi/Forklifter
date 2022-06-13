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

		[Header("Wheels")]
		[SerializeField] private WheelController frontLeft;
		[SerializeField] private WheelController frontRight;
		[SerializeField] private WheelController backLeft;
		[SerializeField] private WheelController backRight;

		private float rotation;
		private Vector3 previousPosition;
		private Vector3 currentPosition;

		private void Start()
		{
			currentPosition = forklifterTransform.position;
			forkLifterRigidbody.centerOfMass = centerOfMass.localPosition;
		}

		public void Move(float vertical, float horizontal, float deltaTime)
		{
			if (horizontal > 0.2f || horizontal < -0.2f)
			{
				rotation += turnSpeed * horizontal * deltaTime;
				rotation = Mathf.Clamp(rotation, -maxRotation, maxRotation);
			}
			else
			{
				rotation = Mathf.Lerp(rotation, 0f, deltaTime * backRotation);
			}

			if (vertical > 0.2f || vertical < -0.2f)
			{
				forkLifterRigidbody.angularVelocity = new Vector3(0, rotation * vertical * deltaTime, 0);

				var speed = forklifterSpeed * vertical;
				forkLifterRigidbody.velocity = forklifterTransform.forward * speed;
			}

			RotateWheels();
		}

		public void MoveFork(float dir, float deltaTime)
		{
			var forkPosition = fork.localPosition;

			forkPosition.y += forkSpeed * deltaTime * dir;
			forkPosition.y = Mathf.Clamp(forkPosition.y, minFrokPos, maxFrokPos);

			fork.localPosition = forkPosition;
		}

		private void RotateWheels()
		{
			previousPosition = currentPosition;
			currentPosition = forklifterTransform.position;

			var velocity = currentPosition - previousPosition;
			var direction = Vector3.Dot(forklifterTransform.forward.normalized, velocity.normalized);

			frontLeft.Rotate(velocity, direction);
			backLeft.Rotate(velocity, direction);
			frontRight.Rotate(velocity, direction);
			backRight.Rotate(velocity, direction);

			frontLeft.RotateSteering(rotation);
			frontRight.RotateSteering(rotation);
		}
	}
}