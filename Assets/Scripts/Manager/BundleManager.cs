using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class BundleManager {

	public static bool IsCanUnloadBundle = true;

	public static bool UnloadBundle(AssetBundle curBundle, bool flag)
	{
		if(IsCanUnloadBundle)
		{   
			if(curBundle!=null)
			{
				curBundle.Unload(flag);
			}
			return true;
		}
		else
		{
			return false;
		}
	}
    public static string BundleRoot = "/Bundle";
    public static StringBuilder sb = new StringBuilder(512);
    public static StringBuilder sb1 = new StringBuilder(512);
    protected const string Separator = "/";
    protected const string FilePrefix = "file:///";


    public static string GetLocalPathRoot()
    {
        string LocalPathRoot = string.Empty;
#if UNITY_4_7
            LocalPathRoot = string.Format("{0}/ResData_UNITY4", Application.persistentDataPath);
#else
			LocalPathRoot = string.Format("{0}/ResData", Application.persistentDataPath);
#endif
        return LocalPathRoot;
    }

    public static string GetDataLocalUrl(string folderPath, string fileName)
    {
        if (folderPath.EndsWith(Separator) == true)
        {
            folderPath = folderPath.Substring(0, folderPath.Length - 1);
        }
        sb.Length = 0;
        sb1.Length = 0;
        sb.AppendFormat("{0}{1}{2}{3}", GetLocalPathRoot(), folderPath, Separator, fileName);
        string localPath = sb.ToString();

        if (File.Exists(localPath) == true)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			sb.Length = 0;
			return sb.AppendFormat("{0}{1}",FilePrefix,localPath).ToString();
#endif
            sb.Length = 0;
            return sb.AppendFormat("{0}{1}", FilePrefix, localPath).ToString();
        }
        else
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			sb.Length=0;
			return sb.AppendFormat("{0}{1}{2}{3}",Application.streamingAssetsPath,folderPath,Separator,fileName).ToString();
#endif
            sb.Length = 0;
            return sb.AppendFormat("{0}{1}{2}{3}{4}", FilePrefix, Application.streamingAssetsPath, folderPath, Separator, fileName).ToString();
        }
    }

    #region Data
    static string DataRootPath = BundleRoot + "/Data";
    static string DataFileName = "Data.bundle";
    private static AssetBundle mDataBundle = null;
    public static AssetBundle DataBundle
    {
        get { return mDataBundle; }
    }

    public delegate void OnLoadDataFinishedDelegate();

    private static bool mLoadingDataBundleFlag = false;
    private static bool mWaittingLoadDataFlag = false;

    public static IEnumerator LoadData(OnLoadDataFinishedDelegate onFinished)
    {
        mWaittingLoadDataFlag = false;
        if (mLoadingDataBundleFlag)
        {
            mWaittingLoadDataFlag = true;
        }
        else
        {
            mLoadingDataBundleFlag = true;
            WWW wwwData = new WWW(GetDataLocalUrl(DataRootPath, DataFileName));
            yield return wwwData;

            if (mWaittingLoadDataFlag == true)
            {
                mWaittingLoadDataFlag = false;
            }

            mLoadingDataBundleFlag = false;

            if (wwwData.assetBundle != null)
            {
                mDataBundle = wwwData.assetBundle;
                if (onFinished != null)
                {
                    onFinished();
                }
            }
            else
            {
                Debug.Log("no data LoadModelFromList");
            }
        }

    }

    public static object LoadTable(string fileName)
    {
        if (mDataBundle == null)
        {
            Debug.Log("mDataBundle == null :: " + fileName);
            return null;
        }
        object obj = mDataBundle.Load(fileName, typeof(object)) as object;

        return obj;
    }

    public static void UnloadDataBundle()
    {
        if (UnloadBundle(mDataBundle, true))
        {
            mDataBundle = null;
        }
    }
    #endregion


}
