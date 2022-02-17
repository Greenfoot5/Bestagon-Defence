using System.Text;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Displays the version text so the user can know what version they're on
    /// </summary>
    public class VersionText : MonoBehaviour
    {
        private static bool _exists;
        
        /// <summary>
        /// Generates the VersionText and displays it for the user
        /// </summary>
        private void Start()
        {
            if (_exists)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            _exists = true;

            var text = new StringBuilder();

            text.AppendLine("Bestagon Defence");
            text.AppendFormat("{0}\n", Application.version);
            text.AppendFormat("Unity {0}\n", Application.unityVersion);
            text.Append(SystemInfo.operatingSystem);

            GetComponent<TextMeshProUGUI>().text = text.ToString();
        }
    }
}
