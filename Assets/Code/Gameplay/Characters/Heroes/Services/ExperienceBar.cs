using UnityEngine;
using UnityEngine.UI;
using Code.Gameplay.Characters.Heroes.Services;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fillImage;

    private IExperienceService _experienceService;

    public void Initialize(IExperienceService experienceService)
    {
        _experienceService = experienceService;
        _experienceService.OnXPChanged += HandleXPChanged;

        // Optional: Initialize immediately
        HandleXPChanged(_experienceService.CurrentXP, _experienceService.RequiredXP);
    }

    private void OnDestroy()
    {
        if (_experienceService != null)
        {
            _experienceService.OnXPChanged -= HandleXPChanged;
        }
    }

    private void HandleXPChanged(float currentXP, float requiredXP)
    {
        float progress = Mathf.Clamp01(currentXP / requiredXP);
        _slider.value = progress;

        _fillImage.color = Color.Lerp(Color.green, Color.yellow, Mathf.InverseLerp(0.2f, 1f, progress));
    }
}
