using UnityEngine;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Abilities.Services;
using Code.Gameplay.Characters.Heroes.Services; // For IExperienceService
using System.Collections.Generic;
using Zenject;
using UnityEngine.UI;

namespace Code.Gameplay.Abilities.Behaviours
{
    public class LevelUpWindowBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform cardParent;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private AbilityDatabase database;

        private IAbilityService _abilityService;
        private IExperienceService _experienceService;

        [SerializeField] private Image levelUpEffectImage;
        [SerializeField] private Text levelUpEffectText; // Will display "Level X"
        [SerializeField] private float hueLoopSpeed = 2f;

        private bool _isHueLoopActive = false;
        private int _displayedLevel;

        [SerializeField] private Text levelDisplayText; // Add this for showing level number

        private int _levelToDisplay;


        [Inject]
        public void Construct(IAbilityService abilityService, IExperienceService experienceService)
        {
            _abilityService = abilityService;
            _experienceService = experienceService;

            _experienceService.OnLevelUp += OnLevelUp;
        }

        private void OnDestroy()
        {
            if (_experienceService != null)
                _experienceService.OnLevelUp -= OnLevelUp;
        }

        private void OnLevelUp(int newLevel)
        {
            _displayedLevel = newLevel;
            Show();
        }

        public void Show()
        {
            if (database == null)
            {
                Debug.LogError("AbilityDatabase is not assigned!");
                return;
            }

            gameObject.SetActive(true);

            // Freeze time and start hue animation
            

          if (levelUpEffectText != null)
                levelUpEffectText.text = $"{_displayedLevel + 1}";
            Time.timeScale = 0f;
            _isHueLoopActive = true;

            var displayed = new List<AbilityType>();
            for (int i = 0; i < 3; i++)
            {
                var ability = database.GetRandomAbility(displayed);
                if (ability == null) continue;
                displayed.Add(ability.Type);

                var cardGO = Instantiate(cardPrefab, cardParent);
                var card = cardGO.GetComponent<AbilityCardBehaviour>();
                card.Setup(ability, OnCardSelected);
            }
            // Show the next level number
            
            if (displayed.Count == 0)
            {
                Debug.LogWarning("No abilities available to display!");
                Close();
            }
        }

        private void Update()
        {
            if (!_isHueLoopActive) return;

            float hue = Mathf.Repeat(Time.unscaledTime * hueLoopSpeed, 1f);
            Color color = Color.HSVToRGB(hue, 1f, 1f);

            if (levelUpEffectImage != null)
                levelUpEffectImage.color = color;

            if (levelUpEffectText != null)
                levelUpEffectText.color = color;
        }

        private void OnCardSelected(AbilityConfig selected)
        {
            _abilityService.ApplyAbility(selected);

            // Confirm level-up: reset XP, update level, resume time
            _experienceService.ConfirmLevelUp();

            Close();
        }

        private void Close()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            _isHueLoopActive = false;

            foreach (Transform child in cardParent)
                Destroy(child.gameObject);
        }
    }
}
