using UnityEngine;

[ExecuteAlways]
public class CameraFitSprite : MonoBehaviour
{
    [Header("Assign the SpriteRenderer of your map (Map1-Snow)")]
    [SerializeField] private SpriteRenderer mapSprite;

    [Range(0f, 0.6f)]
    public float rightPanelPercent = 0.25f;   // 25% of screen reserved for shop panel
    public float paddingWorldUnits = 0f;

    private Camera cam;
    private int lastW, lastH;

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        Fit();
    }

    private void Update()
    {
        // Refit when resolution/aspect changes (device / resize / editor)
        if (cam == null) cam = GetComponent<Camera>();

        if (Screen.width != lastW || Screen.height != lastH)
            Fit();

#if UNITY_EDITOR
        if (!Application.isPlaying)
            Fit();
#endif
    }

    public void Fit()
    {
        if (cam == null) cam = GetComponent<Camera>();

        // ---- Null safety (prevents your error) ----
        if (cam == null) return;
        if (mapSprite == null) return;
        if (mapSprite.sprite == null) return;

        lastW = Screen.width;
        lastH = Screen.height;

        cam.orthographic = true;

        Bounds b = mapSprite.bounds;

        float usableW = Mathf.Max(0.01f, 1f - rightPanelPercent);
        float usableAspect = (Screen.width * usableW) / (float)Screen.height;

        float orthoFromHeight = b.size.y * 0.5f;
        float orthoFromWidth = (b.size.x * 0.5f) / usableAspect;

        cam.orthographicSize = Mathf.Max(orthoFromHeight, orthoFromWidth) + paddingWorldUnits;

        // shift camera left so the right side stays free for your shop panel
        float shiftX = (b.size.x * rightPanelPercent) * 0.5f;

        Vector3 p = cam.transform.position;
        cam.transform.position = new Vector3(b.center.x - shiftX, b.center.y, p.z);
    }
}
