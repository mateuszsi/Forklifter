using UnityEngine;

namespace Forklifter
{
	public class ForklifterController : MonoBehaviour
	{
		[SerializeField] private Rigidbody forkLifterRigidbody;
		[SerializeField] private Transform forklifterTransform;
		[SerializeField] private Transform centerOfMass;
		[SerializeField] private float forklifterSpeed;
		[SerializeField] private float forklifterBackSpeed;
		[SerializeField] private float controlOffset;

		[Header("Ground check")]
		[SerializeField] private Transform groundCheckCenter;
		[SerializeField] private Transform groundCheckCenter2;
		[SerializeField] private float groundCheck;

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
			if (!IsOnGround())
				return;

			CalculateRotation(horizontal, deltaTime);
			ApplyMove(vertical, deltaTime);
			RotateWheels();
		}

		public void MoveFork(float dir, float deltaTime)
		{
			var forkPosition = fork.localPosition;

			forkPosition.y += forkSpeed * deltaTime * dir;
			forkPosition.y = Mathf.Clamp(forkPosition.y, minFrokPos, maxFrokPos);

			fork.localPosition = forkPosition;
		}

		private bool IsOnGround()
		{
			var ray = new Ray(groundCheckCenter.position, Vector3.down);
			var ray2 = new Ray(groundCheckCenter2.position, Vector3.down);

			return Physics.Raycast(ray, groundCheck) && Physics.Raycast(ray2, groundCheck);
		}

		private void CalculateRotation(float horizontal, float deltaTime)
		{
			if (horizontal > controlOffset || horizontal < -controlOffset)
			{
				rotation += turnSpeed * horizontal * deltaTime;
				rotation = Mathf.Clamp(rotation, -maxRotation, maxRotation);
			}
			else
			{
				rotation = Mathf.Lerp(rotation, 0f, deltaTime * backRotation);
			}
		}

		private void ApplyMove(float vertical, float deltaTime)
		{
			if (vertical < 0.2f && vertical > -0.2f)
				return;

			var angularVelocity = forkLifterRigidbody.angularVelocity;
			angularVelocity.y = rotation * vertical * deltaTime;
			forkLifterRigidbody.angularVelocity = angularVelocity;

			var speed = (vertical < 0f ? forklifterBackSpeed : forklifterSpeed) * vertical;
			var forward = forklifterTransform.forward;
			forward.y = 0f;
			forward.Normalize();
			forkLifterRigidbody.velocity = forward * speed;
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