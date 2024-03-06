using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;
    [SerializeField] private float movementSpeed = 0.5f;
    public bool isJoystick;
    private Rigidbody2D rigidbody;

    private Transform spawnedObjectTransform;
    private VariableJoystick joystick;
    private Canvas inputCanvas;
    private PlayerAnimation playerAnimation;
    private CameraToFollow cameraToFollow;

    // Vector2 movementDirection;

    private void Awake()
    {
        cameraToFollow = FindObjectOfType<CameraToFollow>();
        inputCanvas = FindObjectOfType<Canvas>();
        joystick = inputCanvas.GetComponentInChildren<VariableJoystick>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerAnimation = rigidbody.GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        EnableJoystickInput();
        // cameraToFollow.SetObjectTransform(transform);
    }

    private void EnableJoystickInput()
    {
        isJoystick = true;
        inputCanvas.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(IsOwner)
        {
            float horizontal = joystick.Direction.x;
            float vertical = joystick.Direction.y;  

            UpdateServerRpc(horizontal, vertical);
        }
    }

    [ServerRpc]
    private void UpdateServerRpc(float horizontal, float vertical)
    {
        Vector3 movementDirection = new Vector3(horizontal, vertical, transform.position.z).normalized;

        transform.Translate(movementDirection * movementSpeed * Time.deltaTime);

    }

  }