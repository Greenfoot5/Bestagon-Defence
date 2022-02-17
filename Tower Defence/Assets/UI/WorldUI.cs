using Gameplay;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Displays the user's stats in world space
    /// </summary>
    public class WorldUI : MonoBehaviour
    {
        public TMP_Text moneyText;
        public TMP_Text livesText;
        public TMP_Text waveCountText;

        /// <summary>
        /// Updates the user's current stats
        /// </summary>
        private void Update()
        {
            moneyText.text = "<sprite=\"UI-Gold\" name=\"gold\"> " + GameStats.money;
            livesText.text = "<sprite=\"UI-Life\" name=\"life\"> " + GameStats.Lives;
            waveCountText.text = " # " + GameStats.Rounds;
        }
    }
}
