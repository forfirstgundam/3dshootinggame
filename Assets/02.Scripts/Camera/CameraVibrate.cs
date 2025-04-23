using UnityEngine;

public class CameraVibrate : MonoBehaviour
{
    public static CameraVibrate Instance;
    private float _vibrateTime = 0;
    private float _vibrateMagnitude = 0;
    private Vector3 _originalPos;
    //private bool _isShaking = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _originalPos = transform.position;
    }

    public void ShakeCamera(float duration = 0.1f, float magnitude = 2f)
    {
        _originalPos = transform.position;
        _vibrateTime = duration;
        _vibrateMagnitude = magnitude;
    }

    private void LateUpdate()
    {
        if (_vibrateTime > 0)
        {
            //  Perlin Noise를 사용한 부드러운 흔들림
            float x = (Mathf.PerlinNoise(Time.time * 10, 0) - 0.5f) * _vibrateMagnitude;
            float y = (Mathf.PerlinNoise(0, Time.time * 10) - 0.5f) * _vibrateMagnitude;
            float z = (Mathf.PerlinNoise(0, Time.time * 10) - 0.5f) * _vibrateMagnitude;

            transform.localPosition = _originalPos + new Vector3(x, y, z);

            _vibrateTime -= Time.deltaTime; // 시간이 지나면 흔들림 감소
        }
    }

}
