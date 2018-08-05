using UnityEngine;
using System.Collections;

// For use with Photon and SteamVR
public class NetworkedPlayer : Photon.MonoBehaviour
{
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;

    private Transform playerGlobal;
    private Transform playerHead;
    private Transform playerLeftHand;
    private Transform playerRightHand;

    void Start ()
    {
        Debug.Log("Player instantiated");

        if (photonView.isMine)
        {
            Debug.Log("Player is mine");

            playerGlobal = GameObject.Find("Camera Floor Offset").transform;
            playerHead = playerGlobal.Find("Main Camera");
            playerLeftHand = playerGlobal.Find("Left Hand");
            playerRightHand = playerGlobal.Find("Right Hand");

            head.transform.SetParent(playerHead);
            head.transform.localPosition = Vector3.zero;

            leftHand.transform.SetParent(playerLeftHand);
            leftHand.transform.localPosition = Vector3.zero;
            leftHand.transform.localRotation = Quaternion.identity;
            leftHand.transform.localScale = Vector3.one;

            rightHand.transform.SetParent(playerRightHand);
            rightHand.transform.localPosition = Vector3.zero;
            rightHand.transform.localRotation = Quaternion.identity;
            rightHand.transform.localScale = Vector3.one;
        }
    }
	
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // if (!playerGlobal)
        // {
        //     Debug.LogWarning("No player global, ignoring");
        //     return;
        // }
        if (stream.isWriting)
        {
            if (!playerGlobal)
            {
                Debug.LogWarning("No player global, ignoring");
                return;
            }
            stream.SendNext(playerGlobal.position);
            stream.SendNext(playerGlobal.rotation);
            stream.SendNext(playerHead.localPosition);
            stream.SendNext(playerHead.localRotation);
            stream.SendNext(playerLeftHand.localPosition);
            stream.SendNext(playerLeftHand.localRotation);
            stream.SendNext(playerRightHand.localPosition);
            stream.SendNext(playerRightHand.localRotation);
        }
        else
        {
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
            head.transform.localPosition = (Vector3)stream.ReceiveNext();
            head.transform.localRotation = (Quaternion)stream.ReceiveNext();
            leftHand.transform.localPosition = (Vector3)stream.ReceiveNext();
            leftHand.transform.localRotation = (Quaternion)stream.ReceiveNext();
            rightHand.transform.localPosition = (Vector3)stream.ReceiveNext();
            rightHand.transform.localRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
