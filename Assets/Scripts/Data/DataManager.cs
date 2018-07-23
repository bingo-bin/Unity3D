using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager  {

	public static bool initFlag = false;
	public static bool initDoneFlag = false;
    public static bool DataInitFinishFlag {
        get { return DataManager.initDoneFlag; }
    }
	public static void InitData(MonoBehaviour mono)
	{
		if(initFlag == true)
		{
			return;
		}
		initDoneFlag = false;
		initFlag = true;
        if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
		{
			mono.StartCoroutine(BundleManager.LoadData(OnLoadBundleFinished));
		}
	}

	public static void OnLoadBundleFinished()
	{
        ClearAllList();
        mSoundDataDic = DataReader.LoadTable<int, SoundData>("SoundData", "Id");
		initDoneFlag = true;
		BundleManager.UnloadDataBundle();
	}

    static void ClearAllList()
    {
       
    }

    private static Dictionary<int, SoundData> mSoundDataDic = new Dictionary<int, SoundData>();
    public static SoundData GetSoundDataById(int id)
    {
		if (mSoundDataDic==null||mSoundDataDic.Count == 0)
        {
            mSoundDataDic = DataReader.LoadTable<int, SoundData>("SoundData", "Id");
        }
        return DataReader.GetTableRow(mSoundDataDic, id);
    }
}