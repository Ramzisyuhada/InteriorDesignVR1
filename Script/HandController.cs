using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandController : MonoBehaviour
{
    private InputDevice _Targetdevices;
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics leftcontrollercharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftcontrollercharacteristics, devices);

        foreach (var device in devices)
        {
            Debug.Log(device.name + device.characteristics);
        }
        if(devices.Count > 0)
        {
            _Targetdevices = devices[1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        _Targetdevices.TryGetFeatureValue(CommonUsages.primaryButton, out bool _primaryButton);
        Debug.Log(_primaryButton);
        _Targetdevices.TryGetFeatureValue(CommonUsages.trigger, out float _trigger);
        Debug.Log(_trigger);


    }
}
