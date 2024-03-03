using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;

public class TestRealy : MonoBehaviour
{
    private async void Start()
    {
        await UnityServices.InitializeAsync();


    }
}
