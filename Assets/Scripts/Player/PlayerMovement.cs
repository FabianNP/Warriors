using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTransform;
    [SerializeField] private float movementSpeed = 0.5f;
    public bool isJoystick;

    private VariableJoystick joystick;
    private Canvas inputCnavas;
    private Rigidbody2D rigidbody;
    private PlayerAnimation playerAnimation;
    private CameraToFollow cameraToFollow;

    Vector2 movementDirection;


    /*
    //NOTE Only can be Value Types but not References Types
    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public string message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(new MyCustomData
    {
        _bool = true,
        _int = 56,
        message = "Algo por aqui",
    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; randomNumber: " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };
    }
    */
    private void Awake()
    {
        cameraToFollow = FindObjectOfType<CameraToFollow>();
        inputCnavas = FindObjectOfType<Canvas>();
        joystick = inputCnavas.GetComponentInChildren<VariableJoystick>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerAnimation = rigidbody.GetComponent<PlayerAnimation>();    
    }
    private void Start()
    {
        EnableJoystickInput();
        cameraToFollow.SetObjectTransform(transform);
    }
    private void EnableJoystickInput()
    {
        isJoystick = true;
        inputCnavas.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!IsOwner) {
            return;
        }
        if (isJoystick)
        {
            movementDirection = new Vector2(joystick.Direction.x, joystick.Direction.y).normalized;
        }
        playerAnimation.setDirection(movementDirection);
        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);

            // TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } });   
            // TestServerRpc(new ServerRpcParams());   

            /*
            Debug.Log("T");
            randomNumber.Value = new MyCustomData {
                _int = Random.Range(0, 100),
                _bool = !randomNumber.Value._bool,
                message = "Algo por aca",
            };
            */
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
            Destroy(spawnedObjectTransform.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        rigidbody.velocity = movementDirection * movementSpeed;
    }
    /*

    // ---------------------------------------------------------
    // Send messages to the server, only runs on the server 

    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcparams)
    {
        Debug.Log("TestServerRpc " + OwnerClientId + "; " + serverRpcparams.Receive.SenderClientId);
    }

    // -----------------------------------------------------------
    // Send a message from the server to the client

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("client");
    }
    */
}
