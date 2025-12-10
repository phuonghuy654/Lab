using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public GameObject targetPanel;

    public void ClosePanel()
    {
        targetPanel.SetActive(false);
    }
}
