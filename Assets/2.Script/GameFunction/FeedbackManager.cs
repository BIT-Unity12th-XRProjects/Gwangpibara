using UnityEngine;

public enum GameEventCode
{
    AquireItem, Correct, InCorrect
}

public class FeedbackManager : Singleton<FeedbackManager>
{

    public void PlayEffect(bool isBibration, SFXType sfxType)
    {
        if (isBibration)
        {
            Handheld.Vibrate();
        }
    }


}
