using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlayTrackingManager : MonoBehaviour
{
    [SerializeField] ARPlayImageTracker _arPlayImageTracker;
    
    private ARTrackedImage _currentTrackedImage;
    private GameObject _imagePrefab;
    private Transform _trackedImageTransform;
    
    private List<Vector3> _positionSamples = new List<Vector3>();
    private List<Quaternion> _rotationSamples = new List<Quaternion>();
    public bool isSampling = false;
    
    // 트래킹 후 원점 좌표값
    public Transform GetTrackedImageTransform() => _trackedImageTransform;
    
    private void OnEnable()
    {
        _arPlayImageTracker.OnTrackingStarted += HandleTrackingStarted;
    }

    private void OnDisable()
    {
        _arPlayImageTracker.OnTrackingStarted -= HandleTrackingStarted;
    }
    
    private void HandleTrackingStarted(ARTrackedImage image, GameObject prefab)
    {
        _currentTrackedImage = image;
        _imagePrefab = prefab;
        
        StartSampling();

        
    }

    // 매 프레임마다 이미지의 위치/회전 값을 샘플링 리스트에 추가
    private void Update()
    {
        if (isSampling == false || _currentTrackedImage == null)
            return;
        FollowImageDuringSampling();

    }

    // 샘플링하는 코드
    private void StartSampling()
    {
        isSampling = true;

        _positionSamples.Clear();
        _rotationSamples.Clear();
        StartCoroutine(CoSampling(3f));
    }

    // 아직 샘플링 중이면 이미지에 프리팹이 따라다니도록
    private void FollowImageDuringSampling()
    {
        _imagePrefab.transform.position = _currentTrackedImage.transform.position;
        _imagePrefab.transform.rotation = _currentTrackedImage.transform.rotation;
    }

    // 샘플링 완료 후 이미지 좌표를 원점으로 지정 후 저장되어있다면 원점을 기준으로 마커 불러오기
    private void OnSamplingComplete()
    {
        isSampling = false;

        Vector3 avgPos =GetAveragePosition();
        Quaternion avgRot =GetAverageRotation();

        _trackedImageTransform = FixImageTransform(_imagePrefab, avgPos, avgRot);
        
    }

    public void AddSample(Vector3 position, Quaternion rotation)
    {
        _positionSamples.Add(position);
        _rotationSamples.Add(rotation);
    }

    // 샘플링된 위치값들의 퍙균을 구해서 반환
    public Vector3 GetAveragePosition()
    {
        Vector3 sum = Vector3.zero;
        foreach (var pos in _positionSamples)
        {
            sum += pos;
        }
        Vector3 averageImagePosition = sum / _positionSamples.Count;
        return averageImagePosition;
    }

    // 샘플링된 보간들의 평균값을 구해서 반환
    public Quaternion GetAverageRotation()
    {
        if (_rotationSamples.Count == 0) return Quaternion.identity;

        Quaternion avg = _rotationSamples[0];
        for (int i = 1; i < _rotationSamples.Count; i++)
        {
            avg = Quaternion.Slerp(avg, _rotationSamples[i], 1f / (i + 1));
        }
        return avg;
    }

    IEnumerator CoSampling(float seconds)
    {
        float elapsed = 0f;
        while (elapsed < seconds)
        {
            AddSample(_currentTrackedImage.transform.position, _currentTrackedImage.transform.rotation);
            yield return null;
            elapsed += Time.deltaTime;
        }
        OnSamplingComplete();
    }
    
    public Transform FixImageTransform(GameObject imagePrefab, Vector3 averagePosition, Quaternion averageRotation)
    {
        imagePrefab.transform.position = averagePosition;
        imagePrefab.transform.rotation = averageRotation;
        
        return imagePrefab.transform;
    }
}
