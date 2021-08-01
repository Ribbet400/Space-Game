using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
	public float gravity = -9.8f;
	public void Attract(Rigidbody body) // called by a gravity body
	{
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 localUp = body.transform.up;

		// Apply downwards gravity to body
		body.AddForce(gravityUp * gravity);
		// Apply aligning rotation
		body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
	}
}
