using UnityEngine;
using UnityEngine.InputSystem;
using Game.Input;
using Dreamteck.Forever;

public class SheepMovement : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private Runner _basicRunner;
    [SerializeField] private float _slideSpeed = 5f;
    [SerializeField] private float _joystickSlow = 2f;

    private InputController _inputController;
    private Vector2 _targetVector;
    private float _addValue;
    private const int LevelWidth = 5;

    private void Awake()
    {
        if (inputActions != null)
        {
            _inputController = new InputController(inputActions);
            SubscribeEvents();
        }
        else
        {
            Debug.LogError("Input Actions not recognized in inspector!");
        }
    }

    private void SubscribeEvents()
    {
        _inputController.MovementReceived += OnMovementReceived;
        _inputController.MovementEnd += OnMovementEnd;
    }

    private void UnsubscribeEvents()
    {
        _inputController.MovementReceived -= OnMovementReceived;
        _inputController.MovementEnd -= OnMovementEnd;
    }

    private void OnDestroy()
    {
        if (_inputController != null)
        {
            UnsubscribeEvents();
            _inputController.Dispose();
        }
    }

    private void OnMovementReceived(Vector2 movement)
    {
        _addValue = movement.x / _joystickSlow;

        _targetVector = new Vector2(
            Mathf.Clamp(_targetVector.x + _addValue, -LevelWidth, LevelWidth),
            0
        );

        Debug.Log($"Joystick Input: {movement.x}, AddValue: {_addValue}, Target: {_targetVector}");
    }

    private void OnMovementEnd()
    {
        _targetVector = _basicRunner.motion.offset;
        Debug.Log("Movement ended. Resetting target.");
    }

    private void Update()
    {
    
        Vector2 currentOffset = _basicRunner.motion.offset;

        _targetVector = new Vector2(
            Mathf.Clamp(_targetVector.x + _addValue, -LevelWidth, LevelWidth),
            0
        );

        Vector2 finalOffset = Vector2.MoveTowards(
            currentOffset,
            _targetVector,
            _slideSpeed * Time.deltaTime
        );

        _basicRunner.motion.offset = finalOffset;
    }
}



