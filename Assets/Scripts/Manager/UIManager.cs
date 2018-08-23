using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class UIManager : SingletonUnity<UIManager> {

    private UIPanel BaseUIRoot = null;
    private UIPanel PopUIRoot = null;
    private UIPanel TopBarUIRoot = null;
    private UIPanel DialogUIRoot = null;
    private UIPanel TipUIRoot = null;
    public delegate void OnOpenUIDelegate(BaseView view, object param);

    private Dictionary<string, BaseView> mDicCache = new Dictionary<string, BaseView>();
    private Dictionary<string, BaseView> mDicBase = new Dictionary<string, BaseView>();
    private Dictionary<string, BaseView> mDicPop = new Dictionary<string, BaseView>();
    private Dictionary<string, BaseView> mDicTop = new Dictionary<string, BaseView>();
    private Dictionary<string, BaseView> mDicDialog = new Dictionary<string, BaseView>();

    private Dictionary<string, BaseView> mDicTip = new Dictionary<string, BaseView>();

    private List<BaseView> mShowViewList = new List<BaseView>();
    private List<BaseView> mShowDialogList = new List<BaseView>();
    private List<BaseView> mShowTopBarList = new List<BaseView>();

    UIPanel CreateRootObj(string objName, int depth)
    {
        GameObject obj = new GameObject();
        obj.gameObject.name = objName;
        obj.transform.parent = gameObject.transform;
        NGUITools.SetLayer(obj, gameObject.layer);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        UIPanel retPanel = obj.AddComponent<UIPanel>();

        if (retPanel != null)
        {
            retPanel.depth = depth;

        }
        return retPanel;
    }

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
        if (BaseUIRoot == null)
        {
            BaseUIRoot = CreateRootObj("BaseUIRoot", 10);
        }
        if (PopUIRoot == null)
        {
            PopUIRoot = CreateRootObj("PopUIRoot", 20);
        }
        if (TopBarUIRoot == null)
        {
            TopBarUIRoot = CreateRootObj("TopBarUIRoot", 30);
        }
        if (DialogUIRoot == null)
        {
            DialogUIRoot = CreateRootObj("DialogUIRoot", 40);
        }
        if (TipUIRoot == null)
        {
            TipUIRoot = CreateRootObj("TipUIRoot", 50);
        }
    }

    public void ShowUIWithEnter(UIData pathData, OnOpenUIDelegate delOpen = null, object parm = null)
    {
        ShowUI(pathData, delegate(BaseView view, object param)
        {
            if (view != null)
            {
                view.OnEnter(parm, delegate
                {
                    {
                        if (delOpen != null)
                        {
                            delOpen(view, parm);
                        }
                    }
                });
            }
        }, parm);
    }

    public void ShowUI(UIData pathData, OnOpenUIDelegate delOpenUI = null, object parm = null)
    {
        Dictionary<string, BaseView> curDic = null;
        switch (pathData.uiType)
        {
            case UIData.UIType.TYPE_BASE:
                curDic = mDicBase;
                break;
            case UIData.UIType.TYPE_POP:
                curDic = mDicPop;
                break;
            case UIData.UIType.TYPE_DIALOG:
                curDic = mDicDialog;
                break;
            case UIData.UIType.TYPE_TIPS:

                break;
            case UIData.UIType.TYPE_TOP_BAR:
                curDic = mDicTop;
                break;
        }
        if (curDic == null)
            return;
        if (mDicCache.ContainsKey(pathData.name))
        {
            if (!curDic.ContainsKey(pathData.name))
            {
                curDic.Add(pathData.name, mDicCache[pathData.name]);
            }
            mDicCache.Remove(pathData.name);
        }
        if (curDic.ContainsKey(pathData.name))
        {
            DoShowUI(pathData, curDic[pathData.name].gameObject, delOpenUI, parm);
            return;
        }
        LoadUI(pathData, delOpenUI, parm);
    }

    public void LoadUI(UIData pathData, OnOpenUIDelegate delOpenUI = null, object param = null)
    {
        string str = "UI/" + pathData.path;
        GameObject curWindow = ResourcesManager.Load(str) as GameObject;
        if (curWindow != null)
        {
            DoShowUI(pathData, curWindow, delOpenUI, param);
            return;
        }
    }
    void DoShowUI(UIData pathData, GameObject curWindow, OnOpenUIDelegate delOpenUI, object parm = null)
    {
        if (curWindow == null)
        {
            if (delOpenUI != null)
            {
                delOpenUI(null, parm);
            }
            return;
        }
        BaseView view = null;
        Transform parent;
        Dictionary<string, BaseView> relativeDic = null;
        List<BaseView> mShowList = null;
        switch (pathData.uiType)
        {
            case UIData.UIType.TYPE_BASE:
                parent = BaseUIRoot.transform;
                relativeDic = mDicBase;
                mShowList = mShowViewList;
                break;
            case UIData.UIType.TYPE_DIALOG:
                parent = DialogUIRoot.transform;
                relativeDic = mDicDialog;
                mShowList = mShowDialogList;
                break;
            case UIData.UIType.TYPE_TIPS:
                parent = TipUIRoot.transform;
                relativeDic = mDicTip;
                break;
            case UIData.UIType.TYPE_TOP_BAR:
                parent = TopBarUIRoot.transform;
                mShowList = mShowTopBarList;
                relativeDic = mDicTop;
                break;
            case UIData.UIType.TYPE_POP:
                parent = PopUIRoot.transform;
                relativeDic = mDicPop;
                mShowList = mShowViewList;

                break;
            default:
                parent = UIRoot.list[0].transform;
                break;
        }
        if (relativeDic != null)
        {
            if (relativeDic.ContainsKey(pathData.name))
            {
                view = relativeDic[pathData.name];
                view.gameObject.transform.parent = parent;
                view.gameObject.transform.localScale = Vector3.one;
                view.gameObject.transform.localPosition = Vector3.zero;
                NGUITools.SetActive(view.gameObject, true);
                if (mShowList != null)
                    mShowList.Add(view);
            }
            else
            {
                GameObject obj = MonoBehaviour.Instantiate(curWindow) as GameObject;
                obj.transform.parent = parent;
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                view = obj.GetComponent<BaseView>();
                if (view != null)
                {
                    view.CurUIData = pathData;
                    relativeDic.Add(pathData.name, view);
                    if (mShowList != null)
                    {
                        mShowList.Add(view);
                    }
                }
                else
                {
                    Debug.LogError("base view need script:" + pathData.name);
                }
            }
        }
        if (delOpenUI != null)
        {
            delOpenUI(view, parm);
        }
    }


    public void Back(object parm = null, Action closeFinish = null)
    {
        int count = mShowDialogList.Count;
        if (count > 0)
        {
            BaseView view = mShowDialogList[count - 1];
            if (view != null && view.IsEnable)
            {
                view.Back(parm, closeFinish);
                return;
            }
            return;
        }
        count = mShowViewList.Count;
        if (count > 0)
        {
            BaseView view = mShowViewList[count - 1];
            if (view != null && view.IsEnable)
            {
                view.Back(parm, closeFinish);
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Back();
        }
    }

    public void CloseUI(UIData pathData, Action closeFinish = null)
    {
        Dictionary<string, BaseView> mDic = null;
        switch (pathData.uiType)
        {
            case UIData.UIType.TYPE_BASE:
                mDic = mDicBase;
                break;
            case UIData.UIType.TYPE_DIALOG:
                mDic = mDicDialog;
                break;
            case UIData.UIType.TYPE_POP:
                mDic = mDicPop;
                break;
            case UIData.UIType.TYPE_TOP_BAR:
                mDic = mDicTop;
                break;
            case UIData.UIType.TYPE_TIPS:
                mDic = mDicTip;
                break;
        }
        if (mDic != null && mDic.ContainsKey(pathData.name))
        {
            mDic[pathData.name].OnExit(null, delegate
            {
                RecyleUI(pathData);
                if (closeFinish != null)
                {
                    closeFinish();
                }
            });
        }
    }
    public void Clear()
    {
        mDicCache.Clear();
        mDicBase.Clear();
        mDicPop.Clear();
        mDicTop.Clear();
        mDicDialog.Clear();
        mShowViewList.Clear();
        mShowDialogList.Clear();
        mShowTopBarList.Clear();
    }
    public void CloseUI(BaseView view, Action closeFinsh = null)
    {
        UIData data = view.CurUIData;
        CloseUI(data, closeFinsh);
    }
    public void RecyleUI(UIData pathData)
    {
        Dictionary<string, BaseView> mDic = null;
        List<BaseView> mShowList = null;
        switch (pathData.uiType)
        {
            case UIData.UIType.TYPE_BASE:
                mDic = mDicBase;
                mShowList = mShowViewList;
                break;
            case UIData.UIType.TYPE_DIALOG:
                mDic = mDicDialog;
                mShowList = mShowDialogList;
                break;
            case UIData.UIType.TYPE_POP:
                mDic = mDicPop;
                mShowList = mShowViewList;
                break;
            case UIData.UIType.TYPE_TOP_BAR:
                mDic = mDicTop;
                mShowList = mShowTopBarList;
                break;
            case UIData.UIType.TYPE_TIPS:
                mDic = mDicTip;
                break;
        }
        if (mDic == null)
            return;
        BaseView view = mDic[pathData.name];
        if (view != null)
        {
            if (pathData.isCache)
            {
                NGUITools.SetActive(view.gameObject, false);
                mDicCache.Add(pathData.name, view);
            }
            else
            {
                GameObject.Destroy(view.gameObject);
            }
            mDic.Remove(pathData.name);
            if (mShowList != null)
            {
                mShowList.Remove(view);
            }
        }
    }	
}
