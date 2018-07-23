/********************************************************************************
 *	文件名：	SingletonUnity.cs
 *	工程名：	mmo
 *	创建人：	Julian
 *	创建时间:	2016.07.04
 *
 *	功能说明：   继承MonnoBehaviour的单例模式模板
 *	修改记录：
*********************************************************************************/
using UnityEngine;
using System.Collections;

public class SingletonUnity<T>:MonoBehaviour where T :SingletonUnity<T> {

	private static SingletonUnity<T> mInstance;
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
	protected virtual void Awake()
	{
		if(mInstance==null)
		{
			mInstance=this;
			Exists=true;
		}
		else
		{
			if(mInstance!=this)
			{
				Debug.LogWarning("Two Instance"+typeof(T).ToString());
				Destroy(mInstance);
				mInstance=this;

			}
		}
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
