// '21 MLAPI tutorial SRcoder
using UnityEngine;
using MLAPI;
using MLAPI.Transports.PhotonRealtime;
//using MLAPI.Transports.UNET;
 // FOR UNET: restore commented - LAN
 // FOR PHOTON: commented not needed (IP) - WAN
 // DO I NEED ApprovalCheck

public class ConnectionManager : MonoBehaviour
{
    public GameObject[] respawnPoints;

    //UNetTransport transport;
    PhotonRealtimeTransport transport;

    public GameObject connectionButtonPanel;
    public GameObject gamePanelHolder;
    public GameObject gamePanel;
    public Camera startCamera;

    public string room = "1";
    //public string IpAddress = "127.0.0.1";
    private string password = "Space123";

    private void Start()
    {
        gamePanel = gamePanelHolder.transform.GetChild(0).gameObject;
        respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
    }

    // Happens on server
    public void Host()
    {
        transport = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>();
        transport.RoomName = room;

        connectionButtonPanel.SetActive(false);
        startCamera.gameObject.SetActive(false);
        gamePanel.SetActive(true);

        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(GetRandomSpawn(), Quaternion.identity); // no longer needed?
    }

    // Happens on server 
    private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {
        // check incoming data
        bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == password;      
        callback(true, null, approve, GetRandomSpawn(), Quaternion.identity);
    } 

    public void Join()
    {
        //transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        //transport.ConnectAddress = IpAddress;
        transport = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>();
        transport.RoomName = room;

        connectionButtonPanel.SetActive(false);
        startCamera.gameObject.SetActive(false);
        gamePanel.SetActive(true);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(password);
        NetworkManager.Singleton.StartClient();
    }

    public void RoomChanged(string room)
    {
        this.room = room;
    }

    /*
    public void IPchanged(string newAddress)
    {
        this.IpAddress = newAddress;
    }*/
    Vector3 GetRandomSpawn()
    {
        Vector3 respawn = respawnPoints[Random.Range(0, respawnPoints.Length)].transform.position;
        return respawn;
    }
}