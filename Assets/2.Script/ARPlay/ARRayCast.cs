using UnityEngine;

public class ARRayCast : MonoBehaviour
{
    [SerializeField] private Camera _arCamera; // AR 카메라 (Main Camera 연결)
    [SerializeField] private float _rayDistance = 1.0f; // 레이 길이

    void Update()
    {
        Ray ray = new Ray(_arCamera.transform.position, _arCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance))
        {
            Debug.Log("Ray hit object: " + hit.collider.gameObject.name);

            // TODO : 오브젝트 상호작용
        }
    }
}
