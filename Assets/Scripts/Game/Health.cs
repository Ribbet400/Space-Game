using UnityEngine;
using System.Collections;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class Health : NetworkBehaviour
{
    // synchronisable variable
    [SerializeField]
    NetworkVariableInt health = new NetworkVariableInt(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly }, 100);

    public GameObject deathPanel;
    public GameObject gamePanel;

    public GameObject[] respawnPoints;

    public Renderer[] renderers;
    public Collider[] colliders;
    public Behaviour[] scripts;

    public int boidDamage = 5; // do we use???

    Rigidbody rb;

    void Start()
    {
        deathPanel = GameObject.FindGameObjectWithTag("DeathPanel").transform.GetChild(0).gameObject; // change tag?
        gamePanel = GameObject.FindGameObjectWithTag("GameHolder").transform.GetChild(0).gameObject;
        respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        rb = gameObject.GetComponent<Rigidbody>();

        rb.MovePosition(respawnPoints[Random.Range(0, respawnPoints.Length)].transform.position); // spwan random - fix this!
    }
    void Update()
    {
        if (health.Value <= 0 && IsLocalPlayer)
        { 
            Death();
            health.Value = 100;
        }
    }

    public void TakeDamage(int damage)
    {
        health.Value -= damage;
    }

    void Death()
    {
        gamePanel.SetActive(false);
        deathPanel.SetActive(true);
        RespawnServerRPC();
    }

    IEnumerator Respawn() // CLEAN THIS UP
    {
        SetPlayerState(false);
        Vector3 respawn = respawnPoints[Random.Range(0, respawnPoints.Length)].transform.position; // but this moves camera...
        rb.MovePosition(respawn); // issues with sync

        yield return new WaitForSeconds(5); // delay

        SetPlayerState(true);
        if (IsLocalPlayer)
        {
            deathPanel.SetActive(false);
            gamePanel.SetActive(true);
        }
    }

    [ServerRpc]
    void RespawnServerRPC()
    {
        RespawnClientRPC();
    }

    [ClientRpc]
    void RespawnClientRPC()
    {
        StartCoroutine(Respawn());
    }

    void SetPlayerState(bool state)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = state;
        }
        foreach (var collider in colliders)
        {
            collider.enabled = state;
        }
        foreach (var script in scripts)
        {
            script.enabled = state;
        }
        rb.isKinematic = !state;
    }
}
