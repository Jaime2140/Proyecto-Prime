using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Referencias")]
    public DungeonMap dungeonMap;
    // public RandomEncounter encounterSystem;
    
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

        // Nos suscribimos a la acción general "Move"
        inputActions.Player.Move.performed += OnMoveInput;
        
        // Mantenemos la pausa
        inputActions.UI.TogglePause.performed += _ => GameManager.Instance.TogglePause();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    // --- NUEVA LECTURA DE CONTROLES ---
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        // Leemos la dirección del input (WASD, Flechas o Joystick)
        Vector2 inputDir = context.ReadValue<Vector2>();

        // Si pulsamos Arriba (W o Flecha Arriba)
        if (inputDir.y > 0.5f)
        {
            TryMoveForward();
        }
        // Si pulsamos Izquierda (A o Flecha Izquierda)
        else if (inputDir.x < -0.5f)
        {
            TryTurnLeft();
        }
        // Si pulsamos Derecha (D o Flecha Derecha)
        else if (inputDir.x > 0.5f)
        {
            TryTurnRight();
        }
    }

    // --- LÓGICA DE PROTECCIÓN (EL GUARDIÁN) ---

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

    // --- MOVIMIENTO REAL ---

    void MoveForward()
    {
        Vector2Int dir = DirectionToVector(facing);
        Vector2Int target = position + dir;

        if (!dungeonMap.BlocksMovement(target.x, target.y))
        {
            position = target;
            Debug.Log("Posición: " + position);

            // if (encounterSystem != null) encounterSystem.CheckForEncounter();
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