using UnityEngine;

public abstract class ARObject : MonoBehaviour, IDetect
{
    protected Renderer[] _renderer;
    protected bool _initialized = false;

    protected virtual void Awake()
    {
        _renderer = GetComponentsInChildren<Renderer>(includeInactive: true);
    }

    protected virtual void Start()
    {
        if (!_initialized)
        {
            Debug.LogError($"[{name}] 초기화되지 않았습니다. 반드시 Setting() 호출 필요", this);
        }

        foreach(Renderer renderer in _renderer)
        {
            renderer.enabled = false;
        }
    }

    public virtual void TakeCloseOverlap()
    {
        foreach (Renderer renderer in _renderer)
        {
            renderer.enabled = true;
        }
    }

    public virtual void NotTakeDetect()
    {
        foreach (Renderer renderer in _renderer)
        {
            renderer.enabled = false;
        }
    }

    public abstract void TakeRayHit();
    public abstract void TakeClick();
}

