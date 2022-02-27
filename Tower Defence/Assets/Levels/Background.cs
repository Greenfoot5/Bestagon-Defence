using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Levels
{
    /// <summary>
    /// Deselects the background when the player clicks on it
    /// </summary>
    public class Background : MonoBehaviour, IPointerDownHandler
    {
        /// <summary>
        /// Deselects anything the player has selected when the background is clicked
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            BuildManager.instance.Deselect();
        }
    }
}
