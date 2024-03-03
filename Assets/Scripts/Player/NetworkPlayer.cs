using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;
using Unity.Netcode.Components;
using System;

public class NetworkPlayer : NetworkBehaviour
{
    // public NetworkVariable<NetworkGuid> AvatarGuid = new NetworkVariable<NetworkGuid>();

    [SerializeField] private Transform spawnedObjectPrefab;
    [SerializeField] private float movementSpeed = 0.5f;
    public bool isJoystick;
    private Rigidbody2D rigidbody;

    private Transform spawnedObjectTransform;
    private VariableJoystick joystick;
    private Canvas inputCnavas;
    private PlayerAnimation playerAnimation;
    private CameraToFollow cameraToFollow;

    Vector2 movementDirection;


    //NOTE Only can be Value Types but not References Types
    /*
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

        if (!IsOwner) return;        

        PlayerMovement();

        SpawnDespawn();
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            MoveServerRpc(rigidbody.position, rigidbody.velocity);
        }
        else
        {
            rigidbody.position += movementDirection * movementSpeed * Time.deltaTime;
        }


    }

    private void PlayerMovement()
    {

        if (isJoystick)
        {
            movementDirection = new Vector2(joystick.Direction.x, joystick.Direction.y).normalized;
        }
        rigidbody.velocity = movementDirection * movementSpeed;
        playerAnimation.SetDirection(movementDirection);

    }

    private void SpawnDespawn()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
            Destroy(spawnedObjectTransform.gameObject);
        }
    }

    
    [ServerRpc]
    void MoveServerRpc(Vector2 position, Vector2 velocity)
    {
        MoveClientRpc(position, velocity);
    }

    [ClientRpc]
    void MoveClientRpc(Vector2 position, Vector2 velocity)
    {
        if (IsOwner) return;

        rigidbody.position = position;
        rigidbody.velocity = velocity;
    }
    
}