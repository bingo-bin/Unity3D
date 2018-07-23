/********************************************************************************
 *	文件名：	Singleton.cs
 *	工程名：	mmo
 *	创建人：	Julian
 *	创建时间: 2016.07.04
 *
 *	功能说明：   普通单例模式模板
 *	修改记录：
*********************************************************************************/
using System;

public class Singleton<T> where T:class,new() {

	private static T mInstance;
	public static T Instance
	{
		get
		{
			if(mInstance==null)
			{
				mInstance=Activator.CreateInstance<T>();
			}
			return mInstance;
		}
	}
	public static bool Exists
	{
		get
		{
			return mInstance!=null;
		}
	}
	public static void ClearInstace()
	{
		mInstance=null;
	}
}
