using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AuthManager : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public string facebookToken;
    async void Awake()
    {
        await UnityServices.InitializeAsync();

        SetupEvents();

    }

    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            infoText.text = $"Player ID: {AuthenticationService.Instance.PlayerId}";
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
        };
        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.LogError("Player signed out");
        };
        AuthenticationService.Instance.Expired += () =>
        {
            Debug.LogError("Player session could not be refreshed and expired");
        };
    }


    public async void SignInAnonymously()
    {
        await SignInAnonymouslyAsync();
    }
    private async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously");
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch(AuthenticationException e)
        {
            Debug.Log(e);
        }catch(RequestFailedException e)
        {
            Debug.Log(e);
        }
    }

    public async Task SignInWithFacebook(string token)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithFacebookAsync(token);

            Debug.Log("Sign in with facebook success");
        }
        catch (AuthenticationException e)
        {
            Debug.Log(e);
            Debug.Log("Sign with Facebook Failed");
        }
        catch(RequestFailedException e)
        {
            Debug.LogException(e);
        }
    }

}
