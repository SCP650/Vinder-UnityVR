using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    PhotonView m_photonView;
    private Rigidbody rb;
    public bool isBeingHeld = false;//track if it is being hold

    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void TransferOwnership()
    {
        m_photonView.RequestOwnership();
    }
    public void OnSelectEnter()
    {
        m_photonView.RPC("StartNetworkGrabbing",RpcTarget.AllBuffered);
        if(m_photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            return;//no need to transfer ownership 
        }
        TransferOwnership();
    }

    public void OnSelectExit()
    {
        m_photonView.RPC("StopNetworkGrabbing",RpcTarget.AllBuffered );
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {   //target view is the phoont view of the grabed object 
        if(targetView != m_photonView)
        {
            return;
        }
        m_photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("The transfer is complete, new ownser:" + previousOwner);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        throw new System.NotImplementedException();
    }

    [PunRPC]
    public void StartNetworkGrabbing()
    {
        isBeingHeld = true;
    }
    [PunRPC]
    public void StopNetworkGrabbing()
    {
        isBeingHeld = false;
    }

    private void Update()
    {
        if (isBeingHeld)
        {
            rb.isKinematic = true;
            gameObject.layer = 13;
        }
        else
        {
            rb.isKinematic = false;
            gameObject.layer = 8;
        }
    }
}
