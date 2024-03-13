using Unity.Netcode;
using UnityEngine;

public class ClientNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;
    [SerializeField] private float movementSpeed = 0.5f;
    public bool isJoystick;
    private Rigidbody2D rigidbody;

    private VariableJoystick joystick;
    private Canvas inputCanvas;
    private PlayerAnimation playerAnimation;
    private CameraToFollow cameraToFollow;

    Vector3 movementDirection;

    private NetworkVariable<Class> classActive = new NetworkVariable<Class>(); 
    public enum Class
    {
        Tank, 
        Armor,
        Heal
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        cameraToFollow = FindObjectOfType<CameraToFollow>();
        inputCanvas = FindObjectOfType<Canvas>();
        joystick = inputCanvas.GetComponentInChildren<VariableJoystick>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerAnimation = rigidbody.GetComponent<PlayerAnimation>();

        EnableJoystickInput();
        if (IsOwner)
        {
            cameraToFollow.SetObjectTransform(transform);
        }

        // classActive.Value = 

    }

    private void EnableJoystickInput()
    {
        isJoystick = true;
        inputCanvas.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            float horizontal = joystick.Direction.x;
            float vertical = joystick.Direction.y;

            // UpdateServerRpc(horizontal, vertical);

            movementDirection = new Vector3(horizontal, vertical, transform.position.z).normalized;

            rigidbody.velocity = movementDirection * movementSpeed;
            // transform.Translate(movementDirection * movementSpeed);

            playerAnimation.setDirection(movementDirection);

        }
    }

}