using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : SingletonUnity<UIManager> {

    public delegate void OnOpenUIDelegate(bool bSuccess, object param);
    public delegate void OnLoadUIDelegate(GameObject resObject, object param);
    private Dictionary<string, GameObject> mDicUI = new Dictionary<string, GameObject>();
    public List<UIPathData> CurShowUIList = new List<UIPathData>();
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Start()
    { 
        
    }

    void Init()
    {
        mDicUI.Clear();
        CurShowUIList.Clear();
    }

    public void ShowUI(UIPathData pathData, OnOpenUIDelegate delOpenUI = null, object param = null)
    {
        if (mDicUI.ContainsKey(pathData.name))
        {
            DoShowUI(pathData, mDicUI[pathData.name], delOpenUI, param);
            return;
        }
        LoadUI(pathData, delOpenUI, param);
    }

    public void LoadUI(UIPathData pathData, OnOpenUIDelegate delOpenUI = null, object param = null)
    {
        string str = "UI/" + pathData.path;
        GameObject curWindow = ResourcesManager.Load(str) as GameObject;
        if (curWindow != null)
        {
            DoShowUI(pathData, curWindow, delOpenUI, param);
            return;
        }
    }
    void DoShowUI(UIPathData pathData, GameObject curWindow, OnOpenUIDelegate delOpenUI, object param = null)
    {
        if (curWindow == null)
        {
            return;
        }
        Transform parent = this.transform;
       
        if (mDicUI.ContainsKey(pathData.name))
        {
            if (UnityVersionUtil.IsActive(mDicUI[pathData.name]) == false)
            {
                mDicUI[pathData.name].transform.parent = parent;
                mDicUI[pathData.name].transform.localScale = Vector3.one;
                mDicUI[pathData.name].transform.localPosition = Vector3.zero;
                NGUITools.SetActive(mDicUI[pathData.name], true);
            }
        }
        else if (mDicUI != null)
        {
            GameObject obj = MonoBehaviour.Instantiate(curWindow) as GameObject;
            obj.transform.parent = parent;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            if (obj != null)
            {
                mDicUI.Add(pathData.name, obj);
            }
        }
        if (delOpenUI != null)
        {
            delOpenUI(curWindow != null, param);
        }
        if (pathData.backFlag)
        {
            AddShowUIList(pathData);
        }
    }

    void AddShowUIList(UIPathData newpathdata)
    {
        if (CurShowUIList.Contains(newpathdata))
        {
            CurShowUIList.Remove(newpathdata);
        }
        CurShowUIList.Insert(0, newpathdata);
    }

    public void CloseUI(UIPathData pathData)
    {
        if (mDicUI == null)
            return;
        string curName = pathData.name;
        if (!mDicUI.ContainsKey(curName))
            return;

        NGUITools.SetActive(mDicUI[curName], false);

        if (pathData.backFlag)
        {
            RemoveShowList(pathData);
        }
    }

    void RemoveShowList(UIPathData pathData)
    {
        if (CurShowUIList.Contains(pathData))
        {
            CurShowUIList.Remove(pathData);
        }
    }

    public bool BackUIFun()
    {
        //if (IsUnlockTutorialEnable())
        //    return true;
        if (CurShowUIList.Count <= 0)
            return false;

        UIPathData curPathdata = CurShowUIList[0];
        if (curPathdata.needSelfClose)
        {
            return true;
        }

        CurShowUIList.RemoveAt(0);
        if (curPathdata.name.Equals("ExitGameRoot"))
        {
            return false;
        }
        //else if()
        //{
        //}
        
        return true;
    }

}
