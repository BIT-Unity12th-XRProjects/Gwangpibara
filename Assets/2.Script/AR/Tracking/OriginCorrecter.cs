using Unity.XR.CoreUtils;
using UnityEngine;

public class OriginCorrecter : MonoBehaviour
{
    [SerializeField] XROrigin _xROrigin;

    // 현재 트래킹된 이미지 좌표와 샘플링된 평균 좌표 사이의 차이를 계산
    public void CorrectOrigin(Vector3 averagePosition, Vector3 currentPosition)
    {
        Vector3 offset = currentPosition - averagePosition;
        _xROrigin.transform.position -= offset;
    }

    // 이미지 좌표를 평균 위치/회전으로 고정
    public Transform FixImageTransform(GameObject imagePrefab, Vector3 averagePosition, Quaternion averageRotation)
    {
        imagePrefab.transform.position = averagePosition;
        imagePrefab.transform.rotation = averageRotation;
        
        return imagePrefab.transform;
    }
}
