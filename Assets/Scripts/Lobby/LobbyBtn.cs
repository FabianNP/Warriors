using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyBtn : MonoBehaviour
{
    private Button lobbyBtn;
    private TestLobby testLobby;
    private void Awake()
    {
        lobbyBtn = GetComponent<Button>();
        testLobby = FindFirstObjectByType<TestLobby>();
        lobbyBtn.onClick.AddListener(() => {
            testLobby.CreateLobby();
        }); 
    }


}
