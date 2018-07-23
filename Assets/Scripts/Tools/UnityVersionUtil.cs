using UnityEngine;
using System.Collections;

public class UnityVersionUtil {

	static public void SetActiveRecursive (GameObject go, bool state)
	{
        if (go == null)
            return;
		#if UNITY_3_5
		go.SetActiveRecursively(state);
		#else
		go.SetActive(state);
		#endif
	}

	static public bool IsActive(GameObject go)
	{
        if (go == null)
            return false;
#if UNITY_3_5
		return go.active;
#else
		return go.activeInHierarchy;
#endif
	}
    static public bool IsactiveInHierarchy(GameObject go)
    {
        if (go == null)
            return false;
#if UNITY_3_5
		return true;
#else
        return go.activeInHierarchy;
#endif
    }
}
