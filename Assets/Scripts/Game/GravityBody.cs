using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
	public GameObject[] planets;

	private GameObject planet = null;
	private GameObject previousPlanet = null;

	public bool changePlanet = false;

	Rigidbody rb;

	void Awake()
	{
		planets = GameObject.FindGameObjectsWithTag("Planet"); // find of type???

		rb = GetComponent<Rigidbody>();

		// Disable rigidbody gravity and rotation as this is simulated in GravityAttractor script
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
	}

	GameObject GetClosestPlanet(GameObject[] planets)
    {
		float closest = 10000000f;
		GameObject closestPlanet = null;
		foreach (GameObject planet in planets)
		{
			float distance = Vector3.Distance(gameObject.transform.position, planet.transform.position);
			if (distance < closest)
            {
				closest = distance;
				closestPlanet = planet;
            }
		}
		return (closestPlanet);
	}
	void FixedUpdate()
	{
		previousPlanet = planet;
		planet = GetClosestPlanet(planets);

		if (previousPlanet != planet)
		{
			rb.velocity = Vector3.zero;
			changePlanet = true;
		}
        else
        {
			changePlanet = false;
        }

		// changePlanet may be used later
		// Allow this body to be influenced by planet's gravity
		planet.GetComponent<GravityAttractor>().Attract(rb);
	}
}
