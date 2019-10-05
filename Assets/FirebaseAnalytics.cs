 using UnityEngine;

    public class FirebaseAnalytics : MonoBehaviour
    {
    //Start is called before the first frame update
    void Start()
    {
       Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                SSTools.ShowMessage("Firebase Analytics is Initilized Dependency Status : "+dependencyStatus, SSTools.Position.top, SSTools.Time.threeSecond);
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
               // UnityEngine.Debug.LogError(System.String.Format(
                 SSTools.ShowMessage("Could not resolve all Firebase dependencies: {0}" + dependencyStatus,SSTools.Position.top,SSTools.Time.threeSecond);
                // Firebase Unity SDK is not safe to use here.
            }
        });
        Debug.Log("Firebase is Started :");

       // SSTools.ShowMessage("Firebase started", SSTools.Position.bottom, SSTools.Time.oneSecond);
    }

    public void LogEventofLevelPass()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("Level Passed", "Level Number", 3);
        SSTools.ShowMessage("Event to Firebase is Been Submitted:",SSTools.Position.bottom,SSTools.Time.twoSecond);
    }
    public void LogEventofPlay()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("Play", "Play Clicked", 3);
        SSTools.ShowMessage("Event to Firebase is Been Submitted:",SSTools.Position.bottom,SSTools.Time.twoSecond);
    }
}

