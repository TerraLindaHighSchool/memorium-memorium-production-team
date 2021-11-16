// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player_Control/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Clouds"",
            ""id"": ""c0495412-aaab-4fb1-b1ca-7ddcba26220b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""101ffd62-6302-471a-9219-40015a858e25"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""407b5df1-c1db-4420-8b19-3005c13bc771"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""031e4524-e577-49bd-a5bb-29815d0d4c2e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""74e369ed-fcf5-4332-8c07-4cf356d3f920"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""c62d47d1-e24a-444e-82a4-9024b4d74c95"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""18a0a062-a22c-446a-865c-d32b286a5d25"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""eb18bfce-7509-412f-9fe0-6fe4f2404a50"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9a52f65d-ce5d-4d05-9b0b-68647da32c96"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f12571b7-6c9f-4c88-b3f7-5800c141a189"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""353c0156-efa8-4452-94c8-17c8bba4a818"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e67c196f-a635-46e8-8b6b-81770c01a1c2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Shift W"",
                    ""id"": ""bab90ab2-3593-4d51-b4c0-9c126df24ae5"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""7af0dd92-f699-44c5-be91-d2405cf2caee"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""3fa570c0-cfba-40c1-af4a-af28823d9282"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Desktop"",
            ""bindingGroup"": ""Desktop"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Clouds
        m_Clouds = asset.FindActionMap("Clouds", throwIfNotFound: true);
        m_Clouds_Move = m_Clouds.FindAction("Move", throwIfNotFound: true);
        m_Clouds_Jump = m_Clouds.FindAction("Jump", throwIfNotFound: true);
        m_Clouds_Interact = m_Clouds.FindAction("Interact", throwIfNotFound: true);
        m_Clouds_Sprint = m_Clouds.FindAction("Sprint", throwIfNotFound: true);
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

    // Clouds
    private readonly InputActionMap m_Clouds;
    private ICloudsActions m_CloudsActionsCallbackInterface;
    private readonly InputAction m_Clouds_Move;
    private readonly InputAction m_Clouds_Jump;
    private readonly InputAction m_Clouds_Interact;
    private readonly InputAction m_Clouds_Sprint;
    public struct CloudsActions
    {
        private @PlayerControls m_Wrapper;
        public CloudsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Clouds_Move;
        public InputAction @Jump => m_Wrapper.m_Clouds_Jump;
        public InputAction @Interact => m_Wrapper.m_Clouds_Interact;
        public InputAction @Sprint => m_Wrapper.m_Clouds_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_Clouds; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CloudsActions set) { return set.Get(); }
        public void SetCallbacks(ICloudsActions instance)
        {
            if (m_Wrapper.m_CloudsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CloudsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CloudsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CloudsActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_CloudsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_CloudsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_CloudsActionsCallbackInterface.OnJump;
                @Interact.started -= m_Wrapper.m_CloudsActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_CloudsActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_CloudsActionsCallbackInterface.OnInteract;
                @Sprint.started -= m_Wrapper.m_CloudsActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_CloudsActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_CloudsActionsCallbackInterface.OnSprint;
            }
            m_Wrapper.m_CloudsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }
        }
    }
    public CloudsActions @Clouds => new CloudsActions(this);
    private int m_DesktopSchemeIndex = -1;
    public InputControlScheme DesktopScheme
    {
        get
        {
            if (m_DesktopSchemeIndex == -1) m_DesktopSchemeIndex = asset.FindControlSchemeIndex("Desktop");
            return asset.controlSchemes[m_DesktopSchemeIndex];
        }
    }
    public interface ICloudsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
}
