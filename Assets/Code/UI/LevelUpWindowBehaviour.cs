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
    public class LevelUpWindowBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private int cardsToShow = 3;

        private IAbilityService _abilityService;
        private IHeroProvider _heroProvider;
        private AbilityDatabase _abilityDatabase;
        private HudWindow _hudWindow; // Inject HudWindow to get cards parent
        private List<AbilityCardBehaviour> _currentCards = new();

        // Get cards parent from HudWindow
        private Transform CardsParent => _hudWindow?.LevelUpCardsParent;

        [Inject]
        public void Construct(IAbilityService abilityService, IHeroProvider heroProvider, AbilityDatabase abilityDatabase, HudWindow hudWindow)
        {
            _abilityService = abilityService;
            _heroProvider = heroProvider;
            _abilityDatabase = abilityDatabase;
            _hudWindow = hudWindow;
        }

        public void ShowLevelUpWindow()
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f; // Pause the game

            ClearCards();
            GenerateRandomCards();
        }

        private void ClearCards()
        {
            foreach (var card in _currentCards)
            {
                if (card != null)
                    DestroyImmediate(card.gameObject);
            }
            _currentCards.Clear();
        }

        private void GenerateRandomCards()
        {
            if (CardsParent == null)
            {
                Debug.LogError("[LevelUpWindow] Cards parent is null!");
                HideLevelUpWindow();
                return;
            }

            // Filter valid abilities
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
                Debug.LogWarning("No available abilities to show!");
                HideLevelUpWindow();
                return;
            }

            // Shuffle and pick unique random ones
            var selectedAbilities = new List<AbilityConfig>();
            while (selectedAbilities.Count < cardsToShow && validAbilities.Count > 0)
            {
                var index = Random.Range(0, validAbilities.Count);
                var picked = validAbilities[index];
                selectedAbilities.Add(picked);
                validAbilities.RemoveAt(index); // Prevent duplicates
            }

            foreach (var ability in selectedAbilities)
            {
                CreateCard(ability);
            }
        }


        private void CreateCard(AbilityConfig abilityConfig)
        {
            GameObject cardObj = Instantiate(cardPrefab, CardsParent);
            AbilityCardBehaviour card = cardObj.GetComponent<AbilityCardBehaviour>();

            card.Setup(abilityConfig, OnCardSelected);
            _currentCards.Add(card);
        }

        private void OnCardSelected(AbilityConfig selectedAbility)
        {
            Debug.Log($"[LevelUpWindow] Player selected: {selectedAbility.Type}");

            // Get the hero's stats from the provider
            Stats heroStats = _heroProvider.Stats;
            if (heroStats == null)
            {
                Debug.LogError("[LevelUpWindow] Hero stats not available!");
                return;
            }

            // Apply the ability using the injected service and stats
            if (_abilityService.CanApply(selectedAbility))
            {
                _abilityService.ApplyAbility(selectedAbility, heroStats);
                Debug.Log($"[LevelUpWindow] Successfully applied: {selectedAbility.Type}");

                // Debug: Show current stat values
                DebugCurrentStats(selectedAbility, heroStats);
            }
            else
            {
                Debug.LogWarning($"[LevelUpWindow] Cannot apply ability: {selectedAbility.Type}");
            }

            HideLevelUpWindow();
        }

        private void DebugCurrentStats(AbilityConfig appliedAbility, Stats heroStats)
        {
            // Show the affected stat's new value
            var statType = GetStatTypeForAbility(appliedAbility.Type);
            if (statType != Code.Gameplay.UnitStats.StatType.Unknown)
            {
                float newValue = heroStats.GetStat(statType);
                float modifiers = heroStats.GetStatModifiersValue(statType);
                Debug.Log($"[LevelUpWindow] {statType} - New Total: {newValue}, Modifiers: {modifiers}");
            }
        }

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

        private void HideLevelUpWindow()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f; // Resume the game
            ClearCards();
        }
    }
}