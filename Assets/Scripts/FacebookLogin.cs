using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System.Globalization;
using Unity.VisualScripting.Antlr3.Runtime;
using Newtonsoft.Json.Linq;
using TMPro;

public class FacebookLogin : MonoBehaviour
{
    private AuthManager authManager;
    private string Token;

    public TextMeshProUGUI FBUsername;
    private void Awake()
    {

        authManager = GetComponent<AuthManager>();
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }

    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {

            // FB.ActivateApp();
            Debug.Log("Init");
            string s = "client token " + FB.ClientToken + "Userd Id " + AccessToken.CurrentAccessToken.UserId;
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
        DealWithFbMenus(FB.IsLoggedIn);
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square$height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        }
        else
        {
            print("Not logged in");
        }
    }

    private void DisplayUsername(IResult result)
    {
        if(result.Error == null)
        {
            string name = "" + result.ResultDictionary["first_name"];
            if (FBUsername != null) FBUsername.text = name;
            FBUsername.text = name;
            Debug.Log("" + name);
        }
    }

    private void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            Debug.Log("Prifile Pic");

        }
    }


    public void SignInFacebook()
    {
        var perms = new List<string>() {"public_profile", "email"};

        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private async void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            SetInit();
            var aToken = AccessToken.CurrentAccessToken;
            // Token = AccessToken.CurrentAccessToken.TokenString;
            Debug.Log($"Facebook Login token: {aToken.TokenString}");

            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }

            await authManager.SignInWithFacebook(aToken.TokenString);
        }
        else
        {
            Debug.Log("Failed to log in");
        }
    }

    private void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            print("Facebook is Login!");
            string s = "client token " + FB.ClientToken + "User Id" + AccessToken.CurrentAccessToken.UserId;
        }
        else
        {
            print("Facebook is not Logged in!");
        }
        DealWithFbMenus(FB.IsLoggedIn);
    }

    public void LogoutFacebook()
    {
        StartCoroutine(LogOut());
    }

    IEnumerator LogOut()
    {
        FB.LogOut();
        while (FB.IsLoggedIn)
        {
            print("Loggin Out");
            yield return null;
        }
        print("Logout Successfull");
        // if(FB_profilePic != null) FB_profilePic.sprite = null;
        if (FBUsername != null) FBUsername.text = "";
    }

    //region other
    public void FacebookSharefeed()
    {

    }

}
