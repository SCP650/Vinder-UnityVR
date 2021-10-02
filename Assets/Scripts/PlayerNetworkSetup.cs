using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{
    public GameObject LocalXRRigGameObject;
    public GameObject AvartarHead;
    public GameObject AvartarBody;
    public GameObject MainAvatarGameObject;
    public GameObject[] AvatarModelPrefabs;

    public TextMeshProUGUI PlayerName_Text;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //This is me
            LocalXRRigGameObject.SetActive(true);
            GetComponent<MovementController>().enabled = true;
            GetComponent<AvatarInputConverter>().enabled = true;

            //get avatar selection  data so correct model can be instantiated

            object avatarSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVARTAR_SELECTION_NUMBER, out avatarSelectionNumber))
            {
                photonView.RPC("InitializeSelectedAvatarModel", RpcTarget.AllBuffered, (int) avatarSelectionNumber);
            }

            SetLayerRecursively(AvartarHead, 12);
            SetLayerRecursively(AvartarBody, 11);

            TeleportationArea[] teleportationAreas = FindObjectsOfType<TeleportationArea>();
            if(teleportationAreas.Length > 0)
            {
                foreach(var item in teleportationAreas)
                {
                    item.teleportationProvider = LocalXRRigGameObject.GetComponent<TeleportationProvider>();
                }
            }

            MainAvatarGameObject.AddComponent<AudioListener>();
        }
        else
        {
            LocalXRRigGameObject.SetActive(false);
            GetComponent<MovementController>().enabled = false;
            GetComponent<AvatarInputConverter>().enabled = false;
            //remote player's head and body should be seen
            SetLayerRecursively(AvartarHead, 0);
            SetLayerRecursively(AvartarBody, 0);
        }

        if(PlayerName_Text != null)
        {
            PlayerName_Text.text = photonView.Owner.NickName;
        }
    }

    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    [PunRPC]
    public void InitializeSelectedAvatarModel(int avatarSelectionNumber)
    {
        GameObject selectedAvatarGameobject = Instantiate(AvatarModelPrefabs[avatarSelectionNumber], LocalXRRigGameObject.transform);

        AvatarInputConverter avatarInputConverter = transform.GetComponent<AvatarInputConverter>();
        AvatarHolder avatarHolder = selectedAvatarGameobject.GetComponent<AvatarHolder>();
        SetUpAvatarGameobject(avatarHolder.HeadTransform, avatarInputConverter.AvatarHead);
        SetUpAvatarGameobject(avatarHolder.BodyTransform, avatarInputConverter.AvatarBody);
        SetUpAvatarGameobject(avatarHolder.HandLeftTransform, avatarInputConverter.AvatarHand_Left);
        SetUpAvatarGameobject(avatarHolder.HandRightTransform, avatarInputConverter.AvatarHand_Right);
    }

    void SetUpAvatarGameobject(Transform avatarModelTransform, Transform mainAvatarTransform)
    {
        avatarModelTransform.SetParent(mainAvatarTransform);
        avatarModelTransform.localPosition = Vector3.zero;
        avatarModelTransform.localRotation = Quaternion.identity;
    }
}
