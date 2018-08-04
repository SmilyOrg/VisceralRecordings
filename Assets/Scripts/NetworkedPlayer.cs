using UnityEngine;
using System.Collections;

// For use with Photon and SteamVR
public class NetworkedPlayer : Photon.MonoBehaviour
{
    public GameObject head;

    private Transform playerGlobal;
    private Transform playerHead;

    void Start ()
    {
        Debug.LogError("Player instantiated");

        if (photonView.isMine)
        {
            Debug.LogError("Player is mine");

            playerGlobal = GameObject.Find("Camera Floor Offset").transform;
            playerHead = playerGlobal.Find("Main Camera");

            this.transform.SetParent(playerHead);
            this.transform.localPosition = Vector3.zero;
        }
    }
	
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonSerializeView " + stream.isWriting);
        if (stream.isWriting)
        {
            stream.SendNext(playerGlobal.position);
            stream.SendNext(playerGlobal.rotation);
            stream.SendNext(playerHead.localPosition);
            stream.SendNext(playerHead.localRotation);
        }
        else
        {
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
            head.transform.localPosition = (Vector3)stream.ReceiveNext();
            head.transform.localRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
