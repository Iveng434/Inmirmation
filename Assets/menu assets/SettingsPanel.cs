using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    public GameObject settingsPanel;

    public void OpenPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        settingsPanel.SetActive(false);
    }
}