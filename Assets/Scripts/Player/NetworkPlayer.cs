using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
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

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        cameraToFollow = FindObjectOfType<CameraToFollow>();
        inputCanvas = FindObjectOfType<Canvas>();
        joystick = inputCanvas.GetComponentInChildren<VariableJoystick>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerAnimation = rigidbody.GetComponent<PlayerAnimation>();

        EnableJoystickInput();
        if(IsOwner)
        {
            cameraToFollow.SetObjectTransform(transform);
        }

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

            playerAnimation.setDirection(movementDirection);

        }
    }

    [ServerRpc]
    private void UpdateServerRpc(float horizontal, float vertical)
    {
        movementDirection = new Vector3(horizontal, vertical, transform.position.z).normalized;

        transform.Translate(movementDirection * movementSpeed * Time.deltaTime);

    }

}