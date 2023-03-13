//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Settings/Input/InputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""CameraControl"",
            ""id"": ""2adea77c-7d4c-4d17-be5f-1e43f60ca1be"",
            ""actions"": [
                {
                    ""name"": ""CameraRotation"",
                    ""type"": ""Value"",
                    ""id"": ""66aee973-fbcf-4144-811f-818eb11720bc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CameraMovement"",
                    ""type"": ""Value"",
                    ""id"": ""e5235941-076d-4575-82f1-a5e435ad5a8a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""872fc951-0fb5-4244-a3f8-fe3b9aea723d"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""5caf9cdb-ed08-4ee3-a26c-03e00c739443"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""db72e57c-2857-4021-a452-95cd7ded68d2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""53fd189a-8f26-41a8-a716-b07fb737026c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e9bf9477-587c-443e-b6f0-f24d0e07e75e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b7504189-d0ff-4524-9e2b-72c346e0f16f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4b3c3d0d-5f49-4cfa-b56d-e350ac394220"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5b964ed0-e302-429c-990e-60f1d89bca07"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""MouseControl"",
            ""id"": ""51dfce00-9cc0-49a2-b28a-84f1f827ac5e"",
            ""actions"": [
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""3ee7e57b-7b87-4aab-a4a3-f1d54ed5b8f4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""LeftMousePressed"",
                    ""type"": ""Button"",
                    ""id"": ""4f42ca21-f1f8-48cc-90aa-92c7afdf0268"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""52cc01e5-a122-4ae0-9c8c-1ffb6e45c011"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4390605c-352f-448c-8a5c-a031fcf9e4e9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftMousePressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CameraControl
        m_CameraControl = asset.FindActionMap("CameraControl", throwIfNotFound: true);
        m_CameraControl_CameraRotation = m_CameraControl.FindAction("CameraRotation", throwIfNotFound: true);
        m_CameraControl_CameraMovement = m_CameraControl.FindAction("CameraMovement", throwIfNotFound: true);
        // MouseControl
        m_MouseControl = asset.FindActionMap("MouseControl", throwIfNotFound: true);
        m_MouseControl_MousePosition = m_MouseControl.FindAction("MousePosition", throwIfNotFound: true);
        m_MouseControl_LeftMousePressed = m_MouseControl.FindAction("LeftMousePressed", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // CameraControl
    private readonly InputActionMap m_CameraControl;
    private ICameraControlActions m_CameraControlActionsCallbackInterface;
    private readonly InputAction m_CameraControl_CameraRotation;
    private readonly InputAction m_CameraControl_CameraMovement;
    public struct CameraControlActions
    {
        private @InputActions m_Wrapper;
        public CameraControlActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraRotation => m_Wrapper.m_CameraControl_CameraRotation;
        public InputAction @CameraMovement => m_Wrapper.m_CameraControl_CameraMovement;
        public InputActionMap Get() { return m_Wrapper.m_CameraControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlActions set) { return set.Get(); }
        public void SetCallbacks(ICameraControlActions instance)
        {
            if (m_Wrapper.m_CameraControlActionsCallbackInterface != null)
            {
                @CameraRotation.started -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnCameraRotation;
                @CameraRotation.performed -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnCameraRotation;
                @CameraRotation.canceled -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnCameraRotation;
                @CameraMovement.started -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.performed -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.canceled -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnCameraMovement;
            }
            m_Wrapper.m_CameraControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CameraRotation.started += instance.OnCameraRotation;
                @CameraRotation.performed += instance.OnCameraRotation;
                @CameraRotation.canceled += instance.OnCameraRotation;
                @CameraMovement.started += instance.OnCameraMovement;
                @CameraMovement.performed += instance.OnCameraMovement;
                @CameraMovement.canceled += instance.OnCameraMovement;
            }
        }
    }
    public CameraControlActions @CameraControl => new CameraControlActions(this);

    // MouseControl
    private readonly InputActionMap m_MouseControl;
    private IMouseControlActions m_MouseControlActionsCallbackInterface;
    private readonly InputAction m_MouseControl_MousePosition;
    private readonly InputAction m_MouseControl_LeftMousePressed;
    public struct MouseControlActions
    {
        private @InputActions m_Wrapper;
        public MouseControlActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePosition => m_Wrapper.m_MouseControl_MousePosition;
        public InputAction @LeftMousePressed => m_Wrapper.m_MouseControl_LeftMousePressed;
        public InputActionMap Get() { return m_Wrapper.m_MouseControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseControlActions set) { return set.Get(); }
        public void SetCallbacks(IMouseControlActions instance)
        {
            if (m_Wrapper.m_MouseControlActionsCallbackInterface != null)
            {
                @MousePosition.started -= m_Wrapper.m_MouseControlActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_MouseControlActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_MouseControlActionsCallbackInterface.OnMousePosition;
                @LeftMousePressed.started -= m_Wrapper.m_MouseControlActionsCallbackInterface.OnLeftMousePressed;
                @LeftMousePressed.performed -= m_Wrapper.m_MouseControlActionsCallbackInterface.OnLeftMousePressed;
                @LeftMousePressed.canceled -= m_Wrapper.m_MouseControlActionsCallbackInterface.OnLeftMousePressed;
            }
            m_Wrapper.m_MouseControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @LeftMousePressed.started += instance.OnLeftMousePressed;
                @LeftMousePressed.performed += instance.OnLeftMousePressed;
                @LeftMousePressed.canceled += instance.OnLeftMousePressed;
            }
        }
    }
    public MouseControlActions @MouseControl => new MouseControlActions(this);
    public interface ICameraControlActions
    {
        void OnCameraRotation(InputAction.CallbackContext context);
        void OnCameraMovement(InputAction.CallbackContext context);
    }
    public interface IMouseControlActions
    {
        void OnMousePosition(InputAction.CallbackContext context);
        void OnLeftMousePressed(InputAction.CallbackContext context);
    }
}
