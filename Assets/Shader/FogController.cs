using UnityEngine;

[ExecuteAlways] // エディタでも動かす（必要なら）
public class FogSettingsController : MonoBehaviour
{
    [Header("Fog Settings")]
    public bool enableFog = true;

    public FogMode fogMode = FogMode.Exponential;
    public Color fogColor = new Color(1.0f, 0.6f, 0.4f); // 夕方風
    [Range(0f, 1f)] public float fogDensity = 0.02f;

    public float fogStartDistance = 30f;
    public float fogEndDistance = 100f;

    void Update()
    {
        ApplyFogSettings();
    }

    void ApplyFogSettings()
    {
        RenderSettings.fog = enableFog;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = fogMode;

        if (fogMode == FogMode.Linear)
        {
            RenderSettings.fogStartDistance = fogStartDistance;
            RenderSettings.fogEndDistance = fogEndDistance;
        }
        else
        {
            RenderSettings.fogDensity = fogDensity;
        }
    }
}
