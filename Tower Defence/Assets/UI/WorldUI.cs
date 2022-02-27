using Gameplay;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Displays the player's stats in world space
    /// </summary>
    public class WorldUI : MonoBehaviour
    {
        [Tooltip("The TMP text to display the player's current balance")]
        public TMP_Text moneyText;
        [Tooltip("The TMP text to display the player's lives remaining")]
        public TMP_Text livesText;
        [Tooltip("The TMP text to display the wave number")]
        public TMP_Text waveCountText;

        /// <summary>
        /// Updates the player's current stats
        /// </summary>
        private void Update()
        {
            moneyText.text = "<sprite=\"UI-Gold\" name=\"gold\"> " + GameStats.money;
            livesText.text = "<sprite=\"UI-Life\" name=\"life\"> " + GameStats.Lives;
            waveCountText.text = " # " + GameStats.Rounds;
        }
    }
}
