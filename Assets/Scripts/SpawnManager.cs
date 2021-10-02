using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject GenericVRPlayterPrefab;
    public Vector3 SpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate(GenericVRPlayterPrefab.name,SpawnPosition, Quaternion.identity);//spawn player across the network
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
