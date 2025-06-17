using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 특정 시간을 트래킹하여 위치/회전값 샘플링 후 평균 좌표 구하는 코드
/// </summary>
public class PositionSampler : MonoBehaviour
{
    private List<Vector3> _positionSamples = new List<Vector3>();
    private List<Quaternion> _rotationSamples = new List<Quaternion>();
    private bool _isSampling = false;
    private float _samplingDuration = 3f;
    private float _elapsedTime = 0f;

    public bool IsSampling => _isSampling;

    // 샘플링 초기화
    public void StartSampling()
    {
        _positionSamples.Clear();
        _rotationSamples.Clear();
        _elapsedTime = 0f;
        _isSampling = true;
    }

    // 샘플링 종료
    public void StopSampling()
    {
        _isSampling = false;
    }

    // 트래킹 중인 이미지의 좌료값 수집
    public void AddSample(Vector3 position, Quaternion rotation)
    {
        if (!_isSampling) return;

        _positionSamples.Add(position);
        _rotationSamples.Add(rotation);
    }

    // 특정 시간동안 트래킹 진행 후 샘플링 종료
    public void UpdateSampling(float deltaTime)
    {
        if (!_isSampling) return;

        _elapsedTime += deltaTime;
        if (_elapsedTime >= _samplingDuration)
        {
            StopSampling();
        }
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
}
