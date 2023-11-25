using UnityEngine;

public class GyroParallax : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public RectTransform rectTransform; // RectTransform of the UI element
        public float parallaxEffectMultiplier = 1.0f; // Control the parallax effect
        [HideInInspector] public Vector2 initialPos;
    }

    public ParallaxLayer[] layers; // Array of parallax layers
    private Gyroscope _gyro;
    private bool _isGyroReset;

    private void Start()
    {
        if (!SystemInfo.supportsGyroscope)
            return;
        
        _gyro = Input.gyro;
        _gyro.enabled = true;

        // Store the initial positions of all layers
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].initialPos = layers[i].rectTransform.anchoredPosition;
        }
    }

    private void OnEnable()
    {
        _isGyroReset = false;
    }

    private void Update()
    {
        if (_gyro == null)
            return;

        if (!_isGyroReset)
        {
            ResetGyroPosition();
            _isGyroReset = true;
        }

        for (int i = 0; i < layers.Length; i++)
        {
            ParallaxLayer layer = layers[i];
            Vector2 deltaPos = new Vector2(-_gyro.gravity.x, -_gyro.gravity.y) * layer.parallaxEffectMultiplier * 100;
            layer.rectTransform.anchoredPosition = layer.initialPos + deltaPos;
        }
    }

    private void OnDisable()
    {
        _isGyroReset = false;
    }

    private void ResetGyroPosition()
    {
        Vector2 currentGravity = new Vector2(-_gyro.gravity.x, -_gyro.gravity.y);

        for (int i = 0; i < layers.Length; i++)
        {
            ParallaxLayer layer = layers[i];
            layer.initialPos = layer.rectTransform.anchoredPosition - (currentGravity * layer.parallaxEffectMultiplier * 100);
        }
    }
}