using UnityEngine;
using System.Collections;

public class UIWidgetResize : MonoBehaviour
{
    private UIWidget uiWidget;
    public int width = 480;
    public int height = 800;
    private float originWidth = 480f;
    void Awake()
    {
        if (uiWidget == null)
            uiWidget = this.GetComponent<UIWidget>();
        if (uiWidget != null)
        {
            float currentWidth = (Screen.width / (float)Screen.height) * 800f;

            int newWidth = Mathf.FloorToInt(currentWidth * (width / originWidth));
            if (newWidth != uiWidget.width)
            {
                uiWidget.width = newWidth;
                uiWidget.height = height;
            }

            BoxCollider collider = this.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.size = new Vector3(newWidth, height, 1);
            }
        }
    }
    void Start()
    {
        UICamera.onScreenResize += ScreenSizeChanged;
    }
    void Update()
    {
#if UNITY_EDITOR
        Awake();
#endif
    }
    void ScreenSizeChanged()
    {
        Awake();
    }

    void OnDestroy() { UICamera.onScreenResize -= ScreenSizeChanged; }
}
