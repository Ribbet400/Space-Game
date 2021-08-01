using UnityEngine;

public class StartCameraMovement : MonoBehaviour
{
    public Transform centre;
    public float speed = 10;
    void Update()
    {
        transform.RotateAround(centre.position, Vector3.up, speed * Time.deltaTime);
    }
}
