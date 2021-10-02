using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HandSynchronization : MonoBehaviour, IPunObservable
{
    public Transform leftHandTransform;
    private PhotonView m_PhotonView;

    //LEFt Hand Sync
    private float m_Distance_leftHand;

    //positions 
    private Vector3 m_Direction_LeftHand;
    private Vector3 m_NetworkPosition_LeftHand;
    private Vector3 m_StoredPosition_LeftHand;

    //Rotation
    private Quaternion m_NetworkRotation_LeftHand;
    private float m_Angle_LeftHand;

    bool m_firstTake = false;

    private void OnEnable()
    {
        m_firstTake = true;
    }

    private void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();
        m_StoredPosition_LeftHand = leftHandTransform.localPosition;
        m_NetworkPosition_LeftHand = Vector3.zero;
        m_NetworkRotation_LeftHand = Quaternion.identity;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //This player is me, need to send data to other player 

            //send hand position data
            m_Direction_LeftHand = leftHandTransform.localPosition - m_StoredPosition_LeftHand;
            m_StoredPosition_LeftHand = leftHandTransform.localPosition;
            stream.SendNext(leftHandTransform.localPosition);
            stream.SendNext(m_Direction_LeftHand);

            //send lef hand rotation
            stream.SendNext(leftHandTransform.localRotation);
        }
        else
        {
            //the remote player
            m_NetworkPosition_LeftHand = (Vector3)stream.ReceiveNext();
            m_Direction_LeftHand = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                //init
                leftHandTransform.localPosition = m_NetworkPosition_LeftHand;
                m_Distance_leftHand = 0;
            }
            else
            {
                // delay between send tiem and server time 
                float lag = (float) (PhotonNetwork.Time - info.SentServerTime);
                m_NetworkPosition_LeftHand += m_Direction_LeftHand * lag;
                m_Distance_leftHand = Vector3.Distance(leftHandTransform.localPosition, m_NetworkPosition_LeftHand);
            }
            m_NetworkRotation_LeftHand = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                m_Angle_LeftHand = 0;
                leftHandTransform.localRotation = m_NetworkRotation_LeftHand;
            }
            else
            {
                m_Angle_LeftHand = Quaternion.Angle(leftHandTransform.localRotation, m_NetworkRotation_LeftHand);

            }

            if (m_firstTake)
            {
                m_firstTake = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_PhotonView.IsMine)
        {
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, m_NetworkPosition_LeftHand, m_Distance_leftHand * (1.0f / PhotonNetwork.SerializationRate));
            leftHandTransform.localRotation = Quaternion.RotateTowards(leftHandTransform.localRotation, m_NetworkRotation_LeftHand, m_Angle_LeftHand * (1.0f / PhotonNetwork.SerializationRate));
        }
      

    }
}
