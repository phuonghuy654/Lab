using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    // Panel cần tắt — gán trong Inspector
    public GameObject targetPanel;

    // nếu script gắn cùng GameObject với Button, mình sẽ lấy component Button ở Awake
    private Button btn;

    void Awake()
    {
        // tìm Button trên cùng GameObject
        btn = GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogWarning("Không tìm thấy component Button trên " + gameObject.name +
                             ". Nếu script nằm trên object khác, gán Button bằng tay hoặc sửa code.");
            return;
        }

        // đăng ký listener
        btn.onClick.AddListener(ClosePanel);
    }

    // public vì nếu bạn muốn gán qua Inspector vẫn được
    public void ClosePanel()
    {
        if (targetPanel == null)
        {
            Debug.LogWarning("Target panel chưa được gán trên " + name);
            return;
        }

        Debug.Log("Đang tắt panel: " + targetPanel.name);
        targetPanel.SetActive(false);
    }
}