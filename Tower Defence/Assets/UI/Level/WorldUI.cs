using Abstract;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WorldUI : MonoBehaviour
    {
        public TMP_Text moneyText;
        public TMP_Text livesText;
        public TMP_Text waveCountText;

        // Update is called once per frame
        void Update()
        {
            moneyText.text = "<sprite=\"UI-Gold\" name=\"gold\"> " + GameStats.money;
            livesText.text = "<sprite=\"UI-Life\" name=\"life\"> " + GameStats.lives;
            waveCountText.text = " # " + GameStats.rounds;
        }
    }
}
