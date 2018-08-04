using UnityEngine;

public class PhotonNetworkManager : Photon.PunBehaviour
{
    void Awake()
    {
    }

    void Start()
    {
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.sendRate = 15;
        PhotonNetwork.ConnectUsingSettings("1");
    }

    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    // public void Connect()
    // {
    //     // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
    //     if (PhotonNetwork.connected)
    //     {
    //     }
    //     else
    //     {
    //         // #Critical, we must first and foremost connect to Photon Online Server.
            
    //     }
    // }

    public override void OnConnectedToMaster()
    {
        Debug.LogError("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.LogError("OnJoinedLobby");
        // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
        var options = new RoomOptions();
        var lobby = new TypedLobby();
        PhotonNetwork.JoinOrCreateRoom("Room1", options, lobby);
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError("OnJoinedRoom");
        PhotonNetwork.Instantiate("Player Sync", Vector3.zero, Quaternion.identity, 0);
    }
    
    // Gets called when another player joins the game
    public override void OnPhotonPlayerConnected(PhotonPlayer other) {
        Debug.LogError("OnPhotonPlayerConnected ");
        PhotonNetwork.Instantiate("Player Sync", Vector3.zero, Quaternion.identity, 0);
    }
}