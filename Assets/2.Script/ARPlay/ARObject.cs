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
            Debug.LogError($"[{name}] �ʱ�ȭ���� �ʾҽ��ϴ�. �ݵ�� Setting() ȣ�� �ʿ�", this);
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

