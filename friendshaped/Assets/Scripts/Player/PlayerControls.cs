// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player/PlayerControls.inputactions'

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
            ""name"": ""movement"",
            ""id"": ""71331439-5492-48cc-b917-f1a10ab75686"",
            ""actions"": [
                {
                    ""name"": ""walkLeft"",
                    ""type"": ""Button"",
                    ""id"": ""85e8138f-d417-463e-bd57-f7cc750ea1ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""walkRight"",
                    ""type"": ""Button"",
                    ""id"": ""4ee7d6d0-68a6-4231-94ee-520f581b4566"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b3c1a5d2-cb82-40bc-8709-1a3643c09120"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""walkLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""372563db-7dab-4c37-a77a-a1a4919e2e51"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""walkLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d375bb1f-0e0c-4557-9853-c6f977042787"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""walkRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96c0751a-65c9-4182-907a-b8164f1cd384"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""walkRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // movement
        m_movement = asset.FindActionMap("movement", throwIfNotFound: true);
        m_movement_walkLeft = m_movement.FindAction("walkLeft", throwIfNotFound: true);
        m_movement_walkRight = m_movement.FindAction("walkRight", throwIfNotFound: true);
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

    // movement
    private readonly InputActionMap m_movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_movement_walkLeft;
    private readonly InputAction m_movement_walkRight;
    public struct MovementActions
    {
        private @PlayerControls m_Wrapper;
        public MovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @walkLeft => m_Wrapper.m_movement_walkLeft;
        public InputAction @walkRight => m_Wrapper.m_movement_walkRight;
        public InputActionMap Get() { return m_Wrapper.m_movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @walkLeft.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnWalkLeft;
                @walkLeft.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnWalkLeft;
                @walkLeft.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnWalkLeft;
                @walkRight.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnWalkRight;
                @walkRight.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnWalkRight;
                @walkRight.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnWalkRight;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @walkLeft.started += instance.OnWalkLeft;
                @walkLeft.performed += instance.OnWalkLeft;
                @walkLeft.canceled += instance.OnWalkLeft;
                @walkRight.started += instance.OnWalkRight;
                @walkRight.performed += instance.OnWalkRight;
                @walkRight.canceled += instance.OnWalkRight;
            }
        }
    }
    public MovementActions @movement => new MovementActions(this);
    public interface IMovementActions
    {
        void OnWalkLeft(InputAction.CallbackContext context);
        void OnWalkRight(InputAction.CallbackContext context);
    }
}
