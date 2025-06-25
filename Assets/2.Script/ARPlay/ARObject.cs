using UnityEngine;

public abstract class ARObject : MonoBehaviour, IDetect
{
    protected Renderer _renderer;
    protected Collider _collider;
    protected bool _initialized = false;

    protected virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
        if (!_initialized)
        {
            Debug.LogError($"[{name}] 초기화되지 않았습니다. 반드시 Setting() 호출 필요", this);
        }

        _renderer.enabled = false;
        _collider.enabled = false;
    }

    public virtual void TakeCloseOverlap()
    {
        _renderer.enabled = true;
        _collider.enabled = true;
    }

    public virtual void NotTakeDetect()
    {
        _renderer.enabled = false;
        _collider.enabled = false;
    }

    public abstract void TakeRayHit();
    public abstract void TakeClick();
}

