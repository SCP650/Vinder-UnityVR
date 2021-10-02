using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputListener : MonoBehaviour
{
    List<InputDevice> inputDevices;
    InputDeviceCharacteristics deviceCharacteristics;
    public XRNode controllerNode;
    // Start is called before the first frame update
    void Start()
    {
       inputDevices = new List<InputDevice>();
    }

    // Update is called once per frame
    void Update()
    {
       // deviceCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;

        InputDevices.GetDevicesAtXRNode(controllerNode, inputDevices);
        foreach( InputDevice inputDevice in inputDevices)
        {
            bool inputValue;
            //acces value
            //if (inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out inputValue))
           
        }
    }
}
