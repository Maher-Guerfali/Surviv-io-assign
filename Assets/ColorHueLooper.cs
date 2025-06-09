using UnityEngine;
using DG.Tweening;
using UnityEngine.UI; // Optional if you're applying it to a UI element


/// <summary>
/// Continuously cycles the color hue of a material or image or text.
/// Attach this script to any GameObject that needs a hue loop effect.
/// </summary>
public class ColorHueLooper : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("Renderer to apply the hue color effect. Leave null if using UI.")]
    [SerializeField] private Renderer targetRenderer;

    [Tooltip("Image to apply the hue color effect.")]
    [SerializeField] private Image targetImage;

    [Tooltip("TextMeshProUGUI to apply the hue color effect.")]
    [SerializeField] private Text targetTxt;

    [Header("Hue Settings")]
    [SerializeField] private float loopDuration = 5f;
    [SerializeField] private bool useUnscaledTime = true;

    private Material materialInstance;
    private Tween colorTween;

    private void Start()
    {
        if (targetRenderer != null)
        {
            // Use a copy to avoid changing the original material asset
            materialInstance = Instantiate(targetRenderer.material);
            targetRenderer.material = materialInstance;
        }

        StartHueLoop();
    }

    private void StartHueLoop()
    {
        float hue = 0f;

        colorTween = DOTween.To(() => hue, x => {
            hue = x;
            Color newColor = Color.HSVToRGB(hue / 360f, 1f, 1f);

          
            if (targetTxt != null) targetTxt.color = newColor;

        }, 360f, loopDuration)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Restart)
        .SetUpdate(useUnscaledTime);
    }

    private void OnDestroy()
    {
        colorTween?.Kill();
        if (materialInstance != null)
        {
            Destroy(materialInstance);
        }
    }
}
