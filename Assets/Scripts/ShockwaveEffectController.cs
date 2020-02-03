using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer))]
public class ShockwaveEffectController : MonoBehaviour
{
    public float WaveSpeed = 1;
    public bool TriggerOnStart = false;
    public Vector3 ExpandedScale = new Vector3(300, 300, 1);

    public UnityEvent OnShockwaveStart;
    public UnityEvent OnShockwaveEnd;

    private Vector3 StartScale = Vector3.one;

    private MeshRenderer _renderer;
    private Material _material;
    private float _shockwaveProgress;

    [SerializeField]
    private float _magnitude = 1;

    [SerializeField]
    [Range(0, 0.5f)]
    private float _ringWidth = 0.2f;

    [SerializeField]
    [Range(0, 1f)]
    private float _ringDiameter = 1f;

    [SerializeField] private int _sortingOrder = 100;
    [SerializeField] private string _sortingLayer = "Default";

    public float magnitude
    {
        get => _magnitude;
        set
        {
            SetDistortionStrength(value);
            _magnitude = value;
        }
    }

    public float ringWidth
    {
        get => _ringWidth;
        set
        {
            SetRingWidth(value);
            _ringWidth = value;
        }
    }

    public float ringDiameter
    {
        get => _ringDiameter;
        set
        {
            SetRingRadius(value);
            _ringDiameter = value;
        }
    }

    private void SetRingRadius(float value)
    {
        if (_material)
            _material.SetFloat("_Radius", (value / 2) - ringWidth);
    }

    private void SetRingWidth(float value)
    {
        if (_material)
            _material.SetFloat("_Width", value);

    }

    private void SetDistortionStrength(float value)
    {
        if (_material)
            _material.SetFloat("_DistortionStrength", value);
    }

    private void OnValidate()
    {
        SetDistortionStrength(_magnitude);
        SetRingWidth(_ringWidth);
        SetRingRadius(_ringDiameter);
    }


    // Start is called before the first frame update
    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.sortingOrder = _sortingOrder;
        _renderer.sortingLayerName = _sortingLayer;
        if (TriggerOnStart)
            _shockwaveProgress = 0;
        else
            _shockwaveProgress = 10;
        _material = _renderer.material;
        SetDistortionStrength(_magnitude);
        SetRingWidth(_ringWidth);
        SetRingRadius(_ringDiameter);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
            StartShockWave();

        if (_shockwaveProgress >= 1)
        {
            OnShockwaveEnd.Invoke();
        }
        else
        {
            _shockwaveProgress += Time.deltaTime * WaveSpeed;
            transform.localScale = Vector3.Lerp(StartScale, ExpandedScale, _shockwaveProgress);
            SetDistortionStrength(Mathf.Lerp(_magnitude, 0, _shockwaveProgress * _shockwaveProgress));

        }
    }

    private void StartShockWave()
    {
        OnShockwaveStart.Invoke();
        _shockwaveProgress = 0;
    }
}
