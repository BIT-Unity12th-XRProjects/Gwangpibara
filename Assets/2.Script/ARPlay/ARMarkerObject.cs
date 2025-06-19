using UnityEngine;

public class ARMarkerObject : MonoBehaviour
{
    public void TakeRayHit()
    {
        Debug.Log("Camera Hit");
    }

    public void TakeClick()
    {
        Debug.Log("Click Obj");
    }
}
