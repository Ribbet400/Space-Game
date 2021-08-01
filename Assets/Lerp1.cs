using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp1 : MonoBehaviour
{
    private Quaternion QlocalUp;
    private Quaternion QgravityUp;
    private GameObject planet;
    private Quaternion wantedRotation;

    // Start is called before the first frame update
    void Start()
    {
        planet = GameObject.FindGameObjectWithTag("Planet");
        Vector3 gravityUp = (transform.position - planet.transform.position).normalized;
        Vector3 localUp = transform.up.normalized;
        QgravityUp = Quaternion.Euler(gravityUp);
        QlocalUp = Quaternion.Euler(localUp);
        wantedRotation = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 gravityUp = (transform.position - planet.transform.position).normalized;
        Vector3 localUp = transform.up.normalized;
        wantedRotation = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;
        float distance = Vector3.Distance(transform.position, planet.transform.position);
        if (distance < 50)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, wantedRotation, 0.1f);
        }
        
    }



}