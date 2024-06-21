using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class JoyconXR
{
    private Joycon joycon;
    private XRDirectInteractor interactor;
    private InputDeviceDescription targetDescription;
    private XRSimulatedController targetDevice;
    private XRSimulatedControllerState targetState;
    private XRHMD HMD;
    private XRSimulatedHMD simulatedHMD;
    private XRSimulatedHMDState simulatedHMDState;


    private Vector3 initialHandToHmdOffset;
    private bool isInitialized = false;

    public JoyconXR(Joycon j, XRDirectInteractor i)
    {
        joycon = j;
        interactor = i;
        targetDescription = new InputDeviceDescription
        {
            product = nameof(XRSimulatedController),
            capabilities = new XRDeviceDescriptor
            {
                deviceName = $"{nameof(XRSimulatedController)} - {(j.isLeft ? UnityEngine.InputSystem.CommonUsages.LeftHand : UnityEngine.InputSystem.CommonUsages.RightHand)}",
                characteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Controller | (j.isLeft ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right),
            }.ToJson(),
        };
        Debug.Log("JoyconXR created");
    }

    public void Start()
    {
        targetDevice = InputSystem.AddDevice(targetDescription) as XRSimulatedController;
        targetState = new XRSimulatedControllerState();
        HMD = InputSystem.GetDevice<XRHMD>();
        targetState.Reset();
        joycon.Recenter();
        targetState.deviceRotation = joycon.GetVector() * Quaternion.Euler(0, HMD.centerEyeRotation.value.eulerAngles.y, 0);
        Quaternion adjustmentRotation = Quaternion.Euler(90, 0, 0);
        targetState.deviceRotation = targetState.deviceRotation * adjustmentRotation;
        InputState.Change(targetDevice, targetState);

        // Calculate initial offset
        initialHandToHmdOffset = interactor.transform.position - HMD.devicePosition.ReadValue();
        isInitialized = true;

        Debug.Log("JoyconXR device added");
    }

    public void Update()
    {
        if (!isInitialized) return;

        Quaternion adjustmentRotation = Quaternion.Euler(90, 0, 0);
        Vector3 Neckoffset = new Vector3(0, -0.2f, 0);


        if (joycon.GetButtonDown(Joycon.Button.DPAD_DOWN))
        {
            targetState.Reset();
            joycon.Recenter();
            targetState.deviceRotation = joycon.GetVector() * Quaternion.Euler(0, HMD.centerEyeRotation.value.eulerAngles.y, 0);
        }

        // Update buttons and sticks
        targetState.WithButton(ControllerButton.SecondaryButton, joycon.GetButton(Joycon.Button.PLUS) || joycon.GetButton(Joycon.Button.MINUS));

        // Update Grip 
        targetState.grip = joycon.GetButton(Joycon.Button.SHOULDER_2) ? 1 : 0;
        targetState.WithButton(ControllerButton.GripButton, joycon.GetButton(Joycon.Button.SHOULDER_2));

        //Update Trigger
        targetState.trigger = joycon.GetButton(Joycon.Button.DPAD_UP) ? 1 : 0;
        Debug.Log($"triggersate:{targetState.trigger}");
        targetState.WithButton(ControllerButton.TriggerButton, joycon.GetButton(Joycon.Button.DPAD_UP));

        float[] stick = joycon.GetStick();
        targetState.primary2DAxis = new Vector2(stick[0], stick[1]);

        // Update rotation
        targetState.deviceRotation = joycon.GetVector() * Quaternion.Euler(0, HMD.centerEyeRotation.value.eulerAngles.y, 0);
        
        targetState.deviceRotation = targetState.deviceRotation * adjustmentRotation;
        
        Vector3 euler = targetState.deviceRotation.eulerAngles;
        float temp = euler.y;
        euler.y = -euler.z;
        euler.z = temp;
        targetState.deviceRotation = Quaternion.Euler(euler);


        // Calculate new hand position based on HMD position and Joycon rotation
        
        Vector3 hmdPosition = HMD.devicePosition.ReadValue() + Neckoffset;
        Vector3 direction = targetState.deviceRotation * Vector3.forward;
        Vector3 newHandPosition = hmdPosition + 0.27f * direction.normalized * initialHandToHmdOffset.magnitude;
        

        // Update the hand position
        targetState.devicePosition = newHandPosition;

        InputState.Change(targetDevice, targetState);
    }

    public void Stop()
    {
        InputSystem.RemoveDevice(targetDevice);
        Debug.Log("JoyconXR device removed");
    }
}
