using Abstract;
using UnityEngine;
using UnityEngine.Serialization;

namespace Levels.Generic.MainMenu
{
    public class MenuSelection : MonoBehaviour
    {
        [FormerlySerializedAs("UI")]
        [Tooltip("@UI, or the UI that controls scaling and position")]
        [SerializeField]
        private RectTransform ui;

        // Start is called before the first frame update
        private void Start()
        {
            if (!RemoteConfig.IsValidVersion())
            {
                transform.position = new Vector3(0, ui.rect.height * ui.localScale.y, 0);
            }
            else if (!SetUsername.HasUsername())
            {
                transform.position = new Vector3(ui.rect.width * ui.localScale.x, 0, 0);
            }
        }

        public void ContinueWithoutUpdating()
        {
            transform.position = !SetUsername.HasUsername() ? new Vector3(ui.rect.width * ui.localScale.x, 0, 0) : Vector3.zero;
            RemoteConfig.IsValidVersion();
        }
        
        public void GetUpdate()
        {
            Application.OpenURL("https://greenfoot5.itch.io/bestagon-defence");
            Application.Quit();
        }
    }
}
