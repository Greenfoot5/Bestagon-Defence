using UnityEngine;
using TMPro;
using System.Text;

public class VersionText : MonoBehaviour
{
    private static bool exists = false;

    void Start()
    {
        if (exists)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        exists = true;

        StringBuilder text = new StringBuilder();

        text.AppendLine("Bestagon Defence");
        text.AppendFormat("{0} - Build {1}\n", Application.version, 0);
        text.AppendFormat("Unity {0}\n", Application.unityVersion);
        text.Append(SystemInfo.operatingSystem);

        GetComponent<TextMeshProUGUI>().text = text.ToString();
    }
}
