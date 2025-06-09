using UnityEngine;
using UnityEngine.UI;
using Code.Gameplay.Abilities.Configs;
using System;

namespace Code.Gameplay.Abilities.Behaviours
{
    public class AbilityCardBehaviour : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text title;
        [SerializeField] private Text description;
        [SerializeField] private Button selectButton;

        private AbilityConfig _config;
        private Action<AbilityConfig> _onSelected;

        public void Setup(AbilityConfig config, Action<AbilityConfig> onSelected)
        {
            _config = config;
            _onSelected = onSelected;

            if (icon != null) icon.sprite = config.Icon;
            if (title != null) title.text = config.DisplayName;
            if (description != null) description.text = config.Description;

            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() =>
            {
                Debug.Log($"[AbilityCard] Clicked: {_config.Type}");
                _onSelected?.Invoke(_config); 
            });
        }
    }
}