using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    void InitUI()
    {
        if (!SingletonUnity<UIManager>.Exists)
        {
            this.gameObject.AddComponent<UIManager>();
        }
    }

    public static bool InitScreenSizeFlag = false;
    public static float ScreenWidth = 0;
    public static float ScreenHeight = 480;
    public static float ScreenWidthScale = 0;
    public static float ScreenHeightScale = 0;

    void Awake()
    {
        if (UIController.InitScreenSizeFlag == false)
        {
            UIController.InitScreenSizeFlag = true;

            UIRoot uiRoot = gameObject.GetComponent<UIRoot>();
            ScreenHeight = uiRoot.manualHeight;
            ScreenWidth = Mathf.RoundToInt(ScreenHeight * ((float)Screen.width / Screen.height));

            ScreenWidthScale = ScreenWidth / Screen.width;
            ScreenHeightScale = ScreenHeight / Screen.height;
        }

        InitUI();
    }
}
