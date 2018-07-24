using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPathData  {

    public enum UIType
    {
        TYPE_BASE = 0,
        TYPE_POP = 1,
    }
    public string path;
    public string name;
    public UIType uiType;
    public bool backFlag = false;
    public bool needSelfClose = false;
    public static Dictionary<string, UIPathData> UINameDic = new Dictionary<string, UIPathData>();

    public UIPathData(string path1, UIType uiType1, bool addback = false, bool closestate = false)
    {
        path = path1;
        uiType = uiType1;
        int lastPos = path1.LastIndexOf('/');
        if (lastPos > 0)
        {
            name = path1.Substring(lastPos + 1);
        }
        else
        {
            name = path1;
        }
        backFlag = addback;
        needSelfClose = closestate;
        UINameDic.Add(name, this);
    }
}
public class UIInfo
{
    public static UIPathData LoadingUIRoot = new UIPathData("Common/LoadingUIRoot", UIPathData.UIType.TYPE_POP);
    public static UIPathData MenuBaseRootUI = new UIPathData("Common/MenuBaseRoot", UIPathData.UIType.TYPE_BASE);
}
