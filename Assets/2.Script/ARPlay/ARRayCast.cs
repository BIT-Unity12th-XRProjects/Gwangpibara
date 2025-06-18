using UnityEngine;

public class ARRayCast : MonoBehaviour
{
    [SerializeField] private Camera _arCamera; // AR ī�޶� (Main Camera ����)
    [SerializeField] private float _rayDistance = 1.0f; // ���� ����

    void Update()
    {
        Ray ray = new Ray(_arCamera.transform.position, _arCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance))
        {
            Debug.Log("Ray hit object: " + hit.collider.gameObject.name);

            // TODO : ������Ʈ ��ȣ�ۿ�
        }
    }
}
