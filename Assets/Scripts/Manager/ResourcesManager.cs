using UnityEngine;
using System.Collections;

public class ResourcesManager  {

	public static UnityEngine.Object LoadAndInstantiate(string path)
	{
		UnityEngine.Object res=Load(path);
		if(res!=null)
		{
			UnityEngine.Object obj=GameObject.Instantiate(res);
			return obj;
		}
		return null;	
	}
	public static UnityEngine.Object Load(string path)
	{
		UnityEngine.Object res=null;
		res=Resources.Load(path);
		return res;
	}

    public static UnityEngine.Object LoadAnimation(string path)
    {
        UnityEngine.Object res = null;
        res = Resources.Load(path);
        return res;
    }
}
