/********************************************************************************
 *	文件名：	SingletonDontDestoryUnity.cs
 *	工程名：	mmo
 *	创建人：	Julian
 *	创建时间: 2016.07.04
 *
 *	功能说明：   继承MonBehaviour的不销毁单例模板
 *	修改记录：
*********************************************************************************/
using UnityEngine;
using System.Collections;

public class SingletonDontDestoryUnity<T> : MonoBehaviour where T :SingletonDontDestoryUnity<T> {

	private static SingletonDontDestoryUnity<T> mInstance;
	public static T Instance
	{
		get
		{
			return (T)mInstance;
		}
	}
	public static bool Exists
	{
		get;
		private set;
	}

	private bool mIsInit = false;
	public bool IsInit
	{
		get{ return mIsInit; }
	}
	
	protected virtual void Awake()
	{  
		if(mInstance!=null)
		{
			Destroy(this.gameObject);
		  	return;
		}
		else
		{
			Exists=false;
		}
		mInstance=this;
		Exists=true;   
		DontDestroyOnLoad(this);
		mIsInit = true;
	}
	protected virtual void OnDestroy()
	{
		if(mInstance==this)
		{
			Exists=false;
			mInstance=null;
		}
	}
}
