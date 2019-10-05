using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;
using UnityEngine.UI;

public class MyFbScript : MonoBehaviour {

    private MyFbScript()
    {
    }

    public static MyFbScript fb;
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }

        fb = this;
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => { FB.ActivateApp(); });
        }
    }

    void OnApplicationPause (bool pauseStatus)
        {
            // Check the pauseStatus to see if we are in the foreground
            // or background
            if (!pauseStatus) {
                //app resume
                if (FB.IsInitialized) {
                    FB.ActivateApp();
                } else {
                    //Handle FB.Init
                    FB.Init( () => {
                        FB.ActivateApp();
                    });
                }
            }
        }

        void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                showToast("fAcebook SDK Initialize Successfully", 3);

            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
                   showToast("Failed to Initialize the Facebook SDK", 3);
            }
        }

        void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        void Login()
        {
            var perms = new List<string>() { "public_profile", "email" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }

        void AuthCallback(ILoginResult result)
        {
            if (FB.IsLoggedIn)
            {
                // AccessToken class will have session details
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                Debug.Log(aToken.UserId);
                // Print current access token's granted permissions
                foreach (string perm in aToken.Permissions)
                {
                    Debug.Log(perm);
                }
            }
            else
            {
                Debug.Log("User cancelled login");
            }
        }


        void Share()
        {
            FB.ShareLink(    new Uri("https://play.google.com/store/apps/details?id=com.twizar.chef.cooking.city.mad.crazy.game"), "Cooking City Resturant", "Latest Game of Cooking. Enjoy Delicious Food With Us",
                callback: ShareCallback
            );
        }


        void ShareCallback(IShareResult result)
        {
            if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
            {
                Debug.Log("ShareLink Error: " + result.Error);
            }
            else if (!String.IsNullOrEmpty(result.PostId))
            {
                // Print post identifier of the shared content
                Debug.Log(result.PostId);
            }
            else
            {
                // Share succeeded without postID
                Debug.Log("ShareLink success!");
            }
        }

        void SHOW_TOAST()
        {
            SSTools.ShowMessage("TOAST MESSAGE", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }


    // *****************************************************************************************************

    public Text txt;

    void showToast(string text,
        int duration)
    {
        StartCoroutine(showToastCOR(text, duration));
    }

    private IEnumerator showToastCOR(string text,
        int duration)
    {
        Color orginalColor = txt.color;

        txt.text = text;
        txt.enabled = true;

        //Fade in
        yield return fadeInAndOut(txt, true, 0.5f);

        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return fadeInAndOut(txt, false, 0.5f);

        txt.enabled = false;
        txt.color = orginalColor;
    }

    IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
    {
        //Set Values dep
        //ending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.clear;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }
/**
 * Include the Facebook namespace via the following code:
 * using Facebook.Unity;
 *
 * For more details, please take a look at:
 * developers.facebook.com/docs/unity/reference/current/FB.LogAppEvent
 */
    public void LogLevelPassedEvent (int levelNumber) {
        Dictionary<string,object> parameters = new Dictionary<string, object>();
        parameters["LevelNumber"] = levelNumber;
        FB.LogAppEvent(
            "Level Passed",
            null,
            parameters
        );
    }
    /**
     * Include the Facebook namespace via the following code:
     * using Facebook.Unity;
     *
     * For more details, please take a look at:
     * developers.facebook.com/docs/unity/reference/current/FB.LogAppEvent
     */
    public void LogLevelFailEvent (int levelNumber, int subLevel) {
        var parameters = new Dictionary<string, object>();
        parameters["LevelNumber"] = levelNumber;
        parameters["SubLevel"] = subLevel;
        FB.LogAppEvent(
            "Level Fail",
            null,
            parameters
        );
    }
    /**
  * Include the Facebook namespace via the following code:
  * using Facebook.Unity;
  *
  * For more details, please take a look at:
  * developers.facebook.com/docs/unity/reference/current/FB.LogAppEvent
  */
    public void LogPlayClikedEvent () {
        FB.LogAppEvent(
            "PlayCliked"
        );
    }
}
