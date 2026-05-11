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

    [Header("Efectos Visuales")]
    public SmoothTurnEffect turnEffect;

    [Header("Sistemas")]
    public EncounterSystem encounterSystem;

    void Awake()
    {
        inputActions = new GameInputActions();

        if (turnEffect == null)
        {
            turnEffect = GetComponent<SmoothTurnEffect>();
        }
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;

        inputActions.UI.TogglePause.performed += 
            _ => GameManager.Instance.TogglePause();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (turnEffect != null && turnEffect.IsTurning)
            return;

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
        if (GameManager.Instance == null || 
            GameManager.Instance.currentState != GameState.Exploration) 
            return;

        MoveForward(); 
    }

    void TryTurnLeft()
    {
        if (GameManager.Instance == null || 
            GameManager.Instance.currentState != GameState.Exploration) 
            return;
            
        TurnLeft();
    }

    void TryTurnRight()
    {
        if (GameManager.Instance == null || 
            GameManager.Instance.currentState != GameState.Exploration) 
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

            if (encounterSystem != null)
            {
                encounterSystem.CheckEncounter();
            }
        }
        else
        {
            Debug.Log("Hay una pared");
        }
    }

    void TurnLeft()
    {
        if (turnEffect != null)
        {
            StartCoroutine(
                turnEffect.AnimateTurn(-1, () =>
                {
                    facing = (Direction)(((int)facing + 3) % 4);
                    Debug.Log("Mirando: " + facing);
                })
            );
        }
        else
        {
            facing = (Direction)(((int)facing + 3) % 4);
        }
    }

    void TurnRight()
    {
        if (turnEffect != null)
        {
            StartCoroutine(
                turnEffect.AnimateTurn(1, () =>
                {
                    facing = (Direction)(((int)facing + 1) % 4);
                    Debug.Log("Mirando: " + facing);
                })
            );
        }
        else
        {
            facing = (Direction)(((int)facing + 1) % 4);
        }
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