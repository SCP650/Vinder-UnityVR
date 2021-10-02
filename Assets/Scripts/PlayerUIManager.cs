using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIManager : MonoBehaviour
{
    public GameObject VRMenu;
    public GameObject GoHomeBtn;
    // Start is called before the first frame update
    void Start()
    {
        VRMenu.SetActive(false);
        GoHomeBtn.GetComponent<Button>().onClick.AddListener(VirtualWorldManager.Instance.LeaveRoomAndLoadHomeScene);
    }
     
}
