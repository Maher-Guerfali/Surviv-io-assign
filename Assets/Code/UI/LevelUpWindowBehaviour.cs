
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Abilities.Services;
using Code.Gameplay.UnitStats.Behaviours;
using Zenject;
using Code.Gameplay.Characters.Heroes.Services;
using Code.UI;
using Unity.VisualScripting;

namespace Code.Gameplay.Abilities.Behaviours
{
    /// <summary>
    /// Manages the Level Up window.
    /// Responsible for showing random ability cards and applying selected abilities to the Hero.
    /// The cards will be spawned under the HudWindow's LevelUpCardsParent.
    /// Pauses the game while the window is active.
    /// </summary>
    public class LevelUpWindowBehaviour : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private int cardsToShow = 3;

        private IAbilityService _abilityService;
        private IHeroProvider _heroProvider;
        private AbilityDatabase _abilityDatabase;
        private HudWindow _hudWindow; // Used to access LevelUpCardsParent
        private List<AbilityCardBehaviour> _currentCards = new();

        /// <summary>
        /// Cached reference to the LevelUpCardsParent in the HUD window.
        /// </summary>
        private Transform CardsParent => _hudWindow?.LevelUpCardsParent;

        /// <summary>
        /// Inject dependencies.
        /// </summary>
        [Inject]
        public void Construct(IAbilityService abilityService, IHeroProvider heroProvider, AbilityDatabase abilityDatabase, HudWindow hudWindow)
        {
            _abilityService = abilityService;
            _heroProvider = heroProvider;
            _abilityDatabase = abilityDatabase;
            _hudWindow = hudWindow;
        }

        /// <summary>
        /// Public entry point to show the Level Up window.
        /// Pauses the game and generates random ability cards.
        /// </summary>
        public void ShowLevelUpWindow()
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f; // Pause the game

            ClearCards();
            GenerateRandomCards();
        }

        /// <summary>
        /// Destroys existing cards and clears the list.
        /// </summary>
        private void ClearCards()
        {
            foreach (var card in _currentCards)
            {
                if (card != null)
                    DestroyImmediate(card.gameObject);
            }
            _currentCards.Clear();
        }

        /// <summary>
        /// Generates and displays random ability cards.
        /// Filters out abilities the Hero already maxed out.
        /// Ensures no duplicate abilities in a single display.
        /// </summary>
        private void GenerateRandomCards()
        {
            if (CardsParent == null)
            {
                Debug.LogError("[LevelUpWindow] Cards parent is null!");
                HideLevelUpWindow();
                return;
            }

            // Filter abilities that can still be stacked
            var validAbilities = _abilityDatabase.Abilities
                .Where(ability =>
                {
                    int currentStacks = _abilityService.GetStackCount(ability.Type);
                    return currentStacks < ability.MaxStacks;
                })
                .DistinctBy(a => a.Type) // Ensure uniqueness
                .ToList();

            if (validAbilities.Count == 0)
            {
                Debug.LogWarning("[LevelUpWindow] No available abilities to show!");
                HideLevelUpWindow();
                return;
            }

            // Randomly select abilities
            var selectedAbilities = new List<AbilityConfig>();
            while (selectedAbilities.Count < cardsToShow && validAbilities.Count > 0)
            {
                var index = Random.Range(0, validAbilities.Count);
                var picked = validAbilities[index];
                selectedAbilities.Add(picked);
                validAbilities.RemoveAt(index); // Avoid duplicates
            }

            foreach (var ability in selectedAbilities)
            {
                CreateCard(ability);
            }
        }

        /// <summary>
        /// Instantiates an ability card and sets it up.
        /// </summary>
        private void CreateCard(AbilityConfig abilityConfig)
        {
            GameObject cardObj = Instantiate(cardPrefab, CardsParent);
            AbilityCardBehaviour card = cardObj.GetComponent<AbilityCardBehaviour>();

            card.Setup(abilityConfig, OnCardSelected);
            _currentCards.Add(card);
        }

        /// <summary>
        /// Callback for when an ability card is selected by the player.
        /// Applies the ability to the Hero and closes the window.
        /// </summary>
        private void OnCardSelected(AbilityConfig selectedAbility)
        {
            Debug.Log($"[LevelUpWindow] Player selected: {selectedAbility.Type}");

            Stats heroStats = _heroProvider.Stats;
            if (heroStats == null)
            {
                Debug.LogError("[LevelUpWindow] Hero stats not available!");
                return;
            }

            if (_abilityService.CanApply(selectedAbility))
            {
                _abilityService.ApplyAbility(selectedAbility, heroStats);
                Debug.Log($"[LevelUpWindow] Successfully applied: {selectedAbility.Type}");

                DebugCurrentStats(selectedAbility, heroStats);
            }
            else
            {
                Debug.LogWarning($"[LevelUpWindow] Cannot apply ability: {selectedAbility.Type}");
            }

            HideLevelUpWindow();
        }

        /// <summary>
        /// Logs the new value and modifiers of the affected stat after applying an ability.
        /// </summary>
        private void DebugCurrentStats(AbilityConfig appliedAbility, Stats heroStats)
        {
            var statType = GetStatTypeForAbility(appliedAbility.Type);
            if (statType != Code.Gameplay.UnitStats.StatType.Unknown)
            {
                float newValue = heroStats.GetStat(statType);
                float modifiers = heroStats.GetStatModifiersValue(statType);
                Debug.Log($"[LevelUpWindow] {statType} - New Total: {newValue}, Modifiers: {modifiers}");
            }
        }

        /// <summary>
        /// Maps AbilityType to the corresponding StatType.
        /// </summary>
        private Code.Gameplay.UnitStats.StatType GetStatTypeForAbility(AbilityType abilityType)
        {
            return abilityType switch
            {
                AbilityType.HealthPotionsBoost => Code.Gameplay.UnitStats.StatType.HealthPotionEffectiveness,
                AbilityType.PiercingProjectiles => Code.Gameplay.UnitStats.StatType.Piercing,
                AbilityType.BouncingProjectiles => Code.Gameplay.UnitStats.StatType.Bounces,
                AbilityType.OrbitingProjectiles => Code.Gameplay.UnitStats.StatType.OrbitingProjectiles,
                AbilityType.AgilityUp => Code.Gameplay.UnitStats.StatType.RotationSpeed,
                AbilityType.HealthUp => Code.Gameplay.UnitStats.StatType.MaxHealth,
                AbilityType.DamageUp => Code.Gameplay.UnitStats.StatType.Damage,
                _ => Code.Gameplay.UnitStats.StatType.Unknown
            };
        }

        /// <summary>
        /// Hides the Level Up window and resumes the game.
        /// Clears any displayed cards.
        /// </summary>
        private void HideLevelUpWindow()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f; // Resume the game
            ClearCards();
        }
    }
}
