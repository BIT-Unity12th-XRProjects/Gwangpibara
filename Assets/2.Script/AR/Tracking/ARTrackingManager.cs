using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTrackingManager : MonoBehaviour
{
    [SerializeField] TrackedImageHandler _trackedImageHandler;
    [SerializeField] PositionSampler _positionSampler;
    [SerializeField] OriginCorrecter _originCorrector;
    [SerializeField] SearchPosition _searchPosition;
    [SerializeField] MarkerLoader _markerLoader;
    [SerializeField] private CountDownUI _countDownUI;
    private GameObject _currentPrefab;
    private ARTrackedImage _currentTrackedImage;
    private bool _trackingCompleted = false;

    private void OnEnable()
    {
        _trackedImageHandler.OnTrackingStarted += HandleTrackingStarted;
    }

    private void OnDisable()
    {
        _trackedImageHandler.OnTrackingStarted -= HandleTrackingStarted;
    }

    // 이미지 인식 후 샘플링 시작
    private void HandleTrackingStarted(ARTrackedImage image, GameObject prefab)
    {
        if (_trackingCompleted) return;

        _currentTrackedImage = image;
        _currentPrefab = prefab;
        
        _countDownUI.StartCountdown(() =>
        {
            _positionSampler.StartSampling();
        });
    }

    // 매 프레임마다 이미지의 위치/회전 값을 샘플링 리스트에 추가
    private void Update()
    {
        if (_positionSampler.IsSampling == false || _currentTrackedImage == null)
            return;

        StartSampling();

        if (_positionSampler.IsSampling == false)
        {
            OnSamplingComplete();
        }
        else
        {
            
            FollowImageDuringSampling();
        }
    }

    // 샘플링하는 코드
    private void StartSampling()
    {
        _positionSampler.AddSample(_currentTrackedImage.transform.position, _currentTrackedImage.transform.rotation);
        _positionSampler.UpdateSampling(Time.deltaTime);
    }

    // 아직 샘플링 중이면 이미지에 프리팹이 따라다니도록
    private void FollowImageDuringSampling()
    {
        _currentPrefab.transform.position = _currentTrackedImage.transform.position;
        _currentPrefab.transform.rotation = _currentTrackedImage.transform.rotation;
    }

    // 샘플링 완료 후 이미지 좌표를 원점으로 지정 후 저장되어있다면 원점을 기준으로 마커 불러오기
    private void OnSamplingComplete()
    {
        Vector3 avgPos = _positionSampler.GetAveragePosition();
        Quaternion avgRot = _positionSampler.GetAverageRotation();

        _originCorrector.CorrectOrigin(avgPos, _currentTrackedImage.transform.position);
        _originCorrector.FixImageTransform(_currentPrefab, avgPos, avgRot);

        // TODO : 트래킹에 실패한 경우 다시 하도록 설정해야함
        
        _searchPosition.SetTrackedImagePosition(_currentTrackedImage.transform);
        _markerLoader.LoadAndSpawnMarkers(_currentTrackedImage.transform);
        Debug.Log(_currentTrackedImage.transform.position);
 

        _trackingCompleted = true;
        Debug.Log("[AR] 원점 보정 완료 및 마커 고정");
        
    }

}
