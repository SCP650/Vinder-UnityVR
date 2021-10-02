using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class MovementController : MonoBehaviour
{
    public List<XRController> controllers;
    public float speed = 1.0f;
    public GameObject head = null;

    [SerializeField]
    TeleportationProvider teleportationProvider;
    public GameObject MainVRPlayer;
    public GameObject XRRigGameObject;

    private void OnEnable()
    {
        teleportationProvider.endLocomotion += OnEndLocomotion;//be notified when teleportion has ended 
    }

    private void OnDisable()
    {
        teleportationProvider.endLocomotion -= OnEndLocomotion;
    }

    void OnEndLocomotion(LocomotionSystem locomotion)
    {
        MainVRPlayer.transform.position = MainVRPlayer.transform.TransformPoint(XRRigGameObject.transform.localPosition);
        XRRigGameObject.transform.localPosition = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (XRController xRController in controllers)
        {
            if (xRController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 positionVector))
            {
                if (positionVector.magnitude > 0.15f)
                {
                    Move(positionVector);
                }
            }
        }
    }

    private void Move(Vector2 positionVector)
    {
        Vector3 direction = new Vector3(positionVector.x, 0, positionVector.y);
        Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);
        direction = Quaternion.Euler(headRotation) * direction;

        Vector3 movement = direction * speed;
        transform.position += (Vector3.ProjectOnPlane(Time.deltaTime * movement, Vector3.up)); //movmeent is in x and z directions always 
    }
}
