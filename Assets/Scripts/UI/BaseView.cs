using UnityEngine;
using System.Collections;
using System;

public class BaseView :MonoBehaviour {	

	public virtual bool IsEnable
	{
		get
		{
			return isEnable;
		}
		set
		{
			isEnable=value;
		}
	}
	public UIData CurUIData
	{
		get
		{
			return mUIData;
		}
		set
		{
			mUIData=value;
		}
	}
	protected bool isEnable=true;	
	private UIData mUIData=null;
	public virtual void OnEnter(object parm,Action enterFinish=null)
	{   
		if(enterFinish!=null)
		{
			enterFinish();
		}
	}	
	public virtual void OnExit(object parm,Action exitFinish=null)
	{   
		if(exitFinish!=null)
		{
			exitFinish();
		}
	}
	public virtual void OnPause(object parm,Action pauseFinish=null)
	{   
		if(pauseFinish!=null)
		{
			pauseFinish();
		}
	}	
	public virtual void OnResume(object parm,Action resumeFinish=null)
	{   
		if(resumeFinish!=null)
		{
			resumeFinish();
		}	
	}
	public virtual void DestroySelf()
	{

	}
	public virtual void CloseUI(Action closeFinish=null)
	{
		SingletonUnity<UIManager>.Instance.CloseUI(this,closeFinish);
	}
	public virtual void CloseUI(UIData pathData,Action closeFinish=null)
	{
		SingletonUnity<UIManager>.Instance.CloseUI(pathData,closeFinish);
	}
	public  void ShowUI(UIData pathData,UIManager.OnOpenUIDelegate delOpen=null,object parm=null)
	{
		SingletonUnity<UIManager>.Instance.ShowUI(pathData,delOpen,parm);
	}
	public void ShowUIWithEntter(UIData pathData,UIManager.OnOpenUIDelegate delOpen=null,object parm=null)
	{
		SingletonUnity<UIManager>.Instance.ShowUIWithEnter(pathData,delOpen,parm);
	}
	public virtual void Back(object parm=null,Action closeFinish=null)
	{
		SingletonUnity<UIManager>.Instance.CloseUI(this,closeFinish);
	}
}
