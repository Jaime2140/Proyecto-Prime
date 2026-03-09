using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Referencias")]
    public DungeonMap dungeonMap;
    
    [Header("Estado")]
    public Vector2Int position = new Vector2Int(1, 1);
    public enum Direction { North, East, South, West }
    public Direction facing = Direction.North;
    
    private GameInputActions inputActions;

    void Awake()
    {
        inputActions = new GameInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;

        inputActions.UI.TogglePause.performed += _ => GameManager.Instance.TogglePause();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 inputDir = context.ReadValue<Vector2>();

        if (inputDir.y > 0.5f)
        {
            TryMoveForward();
        }
        else if (inputDir.x < -0.5f)
        {
            TryTurnLeft();
        }
        else if (inputDir.x > 0.5f)
        {
            TryTurnRight();
        }
    }

    void TryMoveForward()
    {
        if (GameManager.Instance == null || GameManager.Instance.currentState != GameState.Exploration) 
            return;

        MoveForward(); 
    }

    void TryTurnLeft()
    {
        if (GameManager.Instance == null || GameManager.Instance.currentState != GameState.Exploration) 
            return;
            
        TurnLeft();
    }

    void TryTurnRight()
    {
        if (GameManager.Instance == null || GameManager.Instance.currentState != GameState.Exploration) 
            return;

        TurnRight();
    }

    void MoveForward()
    {
        Vector2Int dir = DirectionToVector(facing);
        Vector2Int target = position + dir;

        if (!dungeonMap.BlocksMovement(target.x, target.y))
        {
            position = target;
            Debug.Log("Posición: " + position);
        }
        else
        {
            Debug.Log("Hay una pared");
        }
    }

    void TurnLeft()
    {
        facing = (Direction)(((int)facing + 3) % 4);
        Debug.Log("Mirando: " + facing);
    }

    void TurnRight()
    {
        facing = (Direction)(((int)facing + 1) % 4);
        Debug.Log("Mirando: " + facing);
    }

    Vector2Int DirectionToVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return Vector2Int.up;
            case Direction.South: return Vector2Int.down;
            case Direction.East:  return Vector2Int.right;
            case Direction.West:  return Vector2Int.left;
        }
        return Vector2Int.zero;
    }
}