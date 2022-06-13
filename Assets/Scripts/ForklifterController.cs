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

		private float rotation;

		private void Start()
		{
			forkLifterRigidbody.centerOfMass = centerOfMass.localPosition;
		}

		private void FixedUpdate()
		{
			var vertical = Input.GetAxis("Vertical");
			var horizontal = Input.GetAxis("Horizontal");

			Move(vertical, horizontal);
		}

		public void Move(float vertical, float horizontal)
		{
			if (horizontal > 0.2f || horizontal < -0.2f)
			{
				rotation += turnSpeed * horizontal * Time.deltaTime;
				rotation = Mathf.Clamp(rotation, -maxRotation, maxRotation);
			}
			else
			{
				rotation = Mathf.Lerp(rotation, 0f, Time.deltaTime * backRotation);
			}

			if (vertical > 0.2f || vertical < -0.2f)
			{
				forkLifterRigidbody.angularVelocity = new Vector3(0, rotation * vertical * Time.deltaTime, 0);

				var speed = forklifterSpeed * vertical;
				forkLifterRigidbody.velocity = forklifterTransform.forward * speed;
			}
		}
	}
}