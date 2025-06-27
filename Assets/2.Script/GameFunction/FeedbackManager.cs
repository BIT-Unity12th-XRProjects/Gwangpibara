using UnityEngine;

public enum GameEventCode
{
    AquireItem, Correct, InCorrect
}

public class FeedbackManager : Singleton<FeedbackManager>
{

    public void PlayEffect(bool isVibration, SFXType sfxType)
    {
        SoundManager.instance.PlaySFX(sfxType);

//        if (isVibration)
//        {
           

//#if UNITY_ANDROID && !UNITY_EDITOR
//            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
//            {
//                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
//                AndroidJavaObject vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");

//                if (vibrator != null)
//                {
//                    vibrator.Call("vibrate", 100); // 100ms 진동
//                }
//            }
//#endif
//        }
    }


}
