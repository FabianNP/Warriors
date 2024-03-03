using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    private Animator animator;

    private string[] staticDirections = { "Static N", "Static NW", "Static W", "Static SW", "Static S", "Static SE", "Static E", "Static NE" };
    private string[] RunDirections = { "Run N", "Run NW", "Run W", "Run SW", "Run S", "Run SE", "Run E", "Run NE" };

    int lastDirection;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 _direction)
    {
        if (!IsLocalPlayer) return; // Solo ejecutar la animacion en el cliente local

        string[] directionArray = null;
        if (_direction.magnitude < 0.01)
        {
            directionArray = staticDirections;
        }
        else
        {
            directionArray = RunDirections;
            lastDirection = DirectionToIndex(_direction);
        }

        animator.Play(directionArray[lastDirection]);
    }

    private int DirectionToIndex(Vector2 direction)
    {
        float step = 360 / 8; //MARKER 45 one circle and 8 slices
        float offset = step / 2;//MARKER 22.5

        float angle = Vector2.SignedAngle(Vector2.up, direction);//MARKER return the signed angle in degrees between 

        angle += offset;
        if (angle < 0)
        {
            angle += 360;
        }

        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }
}
