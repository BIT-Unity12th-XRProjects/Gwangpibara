using UnityEngine;

public abstract class ARObject : MonoBehaviour, IDetect
{
    protected Renderer _renderer;
    protected bool _initialized = false;

    protected virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    protected virtual void Start()
    {
        if (!_initialized)
        {
            Debug.LogError($"[{name}] �ʱ�ȭ���� �ʾҽ��ϴ�. �ݵ�� Setting() ȣ�� �ʿ�", this);
        }

        _renderer.enabled = false;
    }

    public virtual void TakeCloseOverlap()
    {
        _renderer.enabled = true;
    }

    public virtual void NotTakeDetect()
    {
        _renderer.enabled = false;
    }

    public abstract void TakeRayHit();
    public abstract void TakeClick();
}

