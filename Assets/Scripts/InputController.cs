using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Game.Input
{
    public class InputController : IDisposable
    {
        private readonly InputActionAsset _inputActions;
        private readonly InputAction _movementAction;

        public event Action<Vector2> MovementReceived;
        public event Action MovementEnd;

        public InputController(InputActionAsset inputActions)
        {
            _inputActions = inputActions;
            var actionMap = _inputActions.FindActionMap("Player");
            _movementAction = actionMap.FindAction("Move");

            _inputActions.Enable();

            _movementAction.performed += OnMovementPerformed;
            _movementAction.canceled += OnMovementCanceled;
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            MovementReceived?.Invoke(context.ReadValue<Vector2>());
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            MovementEnd?.Invoke();
        }

        public void Dispose()
        {
            _movementAction.performed -= OnMovementPerformed;
            _movementAction.canceled -= OnMovementCanceled;
        }
    }
}

