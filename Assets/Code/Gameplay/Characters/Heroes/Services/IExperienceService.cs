using UnityEngine;

namespace Code.Gameplay.Characters.Heroes.Services
{
    public interface IExperienceService
    {
        // Adds XP, clamped to required XP max for current level
        void AddExperience(float amount);

        // Current accumulated XP towards next level
        float CurrentXP { get; }

        // XP needed to reach the next level
        float RequiredXP { get; }

        // Current player level (starts at 1)
        int CurrentLevel { get; }

        // Event fired when player levels up, with new level number
        event System.Action<int> OnLevelUp;

        // Event fired whenever XP changes (currentXP, requiredXP)
        event System.Action<float, float> OnXPChanged;

        // Called after player selects a level-up card,
        // confirms level up, resets XP, and resumes gameplay
        void ConfirmLevelUp();

        // Optional: To reset XP and level to initial state (e.g. for new game)
        void ResetExperience();
    }
}
