using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Shooting : NetworkBehaviour
{
    public int damage = 10;
    public float range = 100f;
    public LayerMask ignore;

    public ParticleSystem muzzleFlash;
    public GameObject bloodEffect;
    public GameObject impactEffect;

    //public bool isAuto;
    //public float errorMargin = 0.1f;
    public float fireRate = 15f;

    public Camera cam;

    private float nextFire = 0f;

    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextFire) // or button down?
            {
                nextFire = Time.time + 1f / fireRate;
                ShootServerRpc(cam.transform.forward);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)] // client => server // do we need require ownership???
    void ShootServerRpc(Vector3 forward)
    {
        //Vector3 error = new Vector3(UnityEngine.Random.Range(-errorMargin, errorMargin), UnityEngine.Random.Range(-errorMargin, errorMargin), UnityEngine.Random.Range(-errorMargin, errorMargin));
        //Vector3 shootDirection = (cam.transform.forward + error).normalized;
        if (Physics.Raycast(cam.transform.position, forward, out RaycastHit hit, range, ~ignore))
        {
            // HIT PLAYER
            var enemyHealth = hit.transform.GetComponent<Health>();
            if (enemyHealth != null)
            {
                // player
                enemyHealth.TakeDamage(damage);
                GameObject blood = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                blood.GetComponent<NetworkObject>().Spawn();
                Destroy(blood, 2f);
            }

            // HIT BOID
            //else if (hit.transform.gameObject.CompareTag("Boid"))
            //{
            //    Destroy(hit.transform.gameObject);
            //}

            else if (!hit.transform.gameObject.CompareTag("Boid"))
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                impact.GetComponent<NetworkObject>().Spawn();
                Destroy(impact, 2f);
                             
            }
        }
        ShootClientRpc();
    }  

    [ClientRpc] // server => client
    void ShootClientRpc()
    {
        muzzleFlash.Play(); // where to run?
        
        /*if (IsLocalPlayer && hit.CompareTag("Boid"))
        {
            Destroy(hit.transform.gameObject);
        }*/
    }

        /*

            [ServerRpc] // client => server
            void ShootServerRpc()
            {
                // do raycast on server???
                ShootClientRpc();
            }

            [ClientRpc] // server => client
            void ShootClientRpc()
            {
                muzzleFlash.Play();

                //Vector3 error = new Vector3(UnityEngine.Random.Range(-errorMargin, errorMargin), UnityEngine.Random.Range(-errorMargin, errorMargin), UnityEngine.Random.Range(-errorMargin, errorMargin));
                //Vector3 shootDirection = (cam.transform.forward + error).normalized;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, range, ~ignore))
                {
                    // HIT PLAYER
                    var enemyHealth = hit.transform.GetComponent<Health>();
                    if (enemyHealth != null)
                    {
                        // player
                        enemyHealth.TakeDamage(damage);
                        GameObject blood = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(blood, 2f);
                    }

                    // HIT BOID
                    else if (hit.transform.gameObject.CompareTag("Boid"))
                    {
                        Destroy(hit.transform.gameObject);
                    }

                    else
                    {
                        GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                        //impact.GetComponent<NetworkObject>().Spawn();
                        Destroy(impact, 2f);
                    }
                }

            } */
    }
