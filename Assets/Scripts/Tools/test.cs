using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    bool run = true;
	void Start () {
        DataManager.OnLoadBundleFinished();
	}
	
	void Update () {
        if (!run)
            return;
        run = false;
        for (int i = 0; i < 20; i++)
        {
            SoundData tempdata = DataManager.GetSoundDataById(i);
            if (tempdata != null)
            {
                Debug.Log(tempdata.Id + ":" + tempdata.Name + ":" + tempdata.FullPathName);
            }
        }
	}
}
