using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIData  {

    public enum UIType
    {
        TYPE_BASE = 0,
        TYPE_POP = 1,
        TYPE_TOP_BAR = 2,
        TYPE_DIALOG = 3,
        TYPE_TIPS = 4,
    }
    public string path;
    public string name;
    public UIType uiType;
    public BaseView curView;
    public bool isCache = true;
    public UIData(string path1, UIType uiType1, bool cache = true)
    {
        path = path1;
        uiType = uiType1;
        isCache = cache;
        int lastPos = path1.LastIndexOf('/');
        if (lastPos > 0)
        {
            name = path1.Substring(lastPos + 1);
        }
        else
        {
            name = path1;
        }
    }
}
public class UIInfo
{
    public static UIData LoadingUIRoot = new UIData("Common/LoadingUIRoot", UIData.UIType.TYPE_POP);
    public static UIData MenuBaseRootUI = new UIData("Common/MenuBaseRoot", UIData.UIType.TYPE_BASE);
}
