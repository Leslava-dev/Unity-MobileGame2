/*using UnityEngine;

namespace Game
{
    public class NewCharContols : MonoBehaviour
    {
        private Controllers.Input.InputController _inputController;

        [SerializeField] private float moveSpeed = 5f;
        private Vector2 _moveInput;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _inputController = new Controllers.Input.InputController();
            _inputController.SubscribeEvents();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _inputController.MovementReceived += OnMovementReceived;
        }

        private void UnsubscribeEvents()
        {
            _inputController.MovementReceived -= OnMovementReceived;
        }

        private void OnMovementReceived(Vector2 movement)
        {
            _moveInput = movement;
        }

        private void FixedUpdate()
        {
            Vector3 movement = new Vector3(_moveInput.x, 0, _moveInput.y);
            _rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
            _inputController.Dispose();
        }
    }
}*/
//example. from the lesson

