using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class BundleCreator : Editor {


   // ==============================Test=======================================
        static string ResPath = Application.dataPath + "/BundleRes";
        static string OutPutPathRoot = Application.dataPath + "/Bundle";

        //[MenuItem("Bundle/CreateTestBundle")]
        //public static void CreateTestBundle()
        //{
        //    FileUtil.DeleteFileOrDirectory(OutPutPathRoot);
        //    CreateTestBundle(ResPath, OutPutPathRoot);
        //}
        //static string[] FileEnd = new string[] { "prefab" };
        //static void CreateTestBundle(string resPath, string outPutPath)
        //{
        //    Dictionary<string, string> PathDic = GetFilePathNamePathsDic(resPath, FileEnd);
        //    Dictionary<string, Object> ObjsDic = GetFileNameObjDic(PathDic);
        //    Dictionary<string, Object> finalObjsDic = new Dictionary<string, Object>();

        //    foreach (KeyValuePair<string, string> curObjPath in PathDic)
        //    {
        //        string curKey = curObjPath.Key;
        //        if (!ObjsDic.ContainsKey(curKey))
        //        {
        //            Debug.LogError(curKey);
        //            continue;
        //        }
        //        //curKey = RecordSourceLevel(curKey, startPath, curObjPath.Value, loadPath);
        //        finalObjsDic.Add(curKey, ObjsDic[curObjPath.Key]);
        //    }
        //    BuildAssetBundleOptions optionNormal = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DisableWriteTypeTree | BuildAssetBundleOptions.DeterministicAssetBundle;
        //    foreach (KeyValuePair<string, Object> obj in finalObjsDic)
        //    {
        //        BuildPipeline.PushAssetDependencies();
        //        string outputPath = outPutPath + "/" + obj.Key + ".bundle";
        //        MyFileUtil.CheckPath(outputPath);
        //        BuildPipeline.BuildAssetBundle(obj.Value, null, outputPath, optionNormal, BuildTarget.Android);
        //        BuildPipeline.PopAssetDependencies();
        //    }
        //    //FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath + "/Bundle");
        //    //FileUtil.CopyFileOrDirectory(outPutPath, Application.streamingAssetsPath + "/Bundle");
        //    Debug.Log("Create Test Bundle Done");
        //}
        static Dictionary<string, string> GetFilePathNamePathsDic(string path, string[] fileEndArray)
        {
            Dictionary<string, string> pathDic = new Dictionary<string, string>();
            GetFilePathNamePathDicRecursion(pathDic, path, path, fileEndArray);
            return pathDic;
        }

        //文件名：OBJ 字典  
        static Dictionary<string, Object> GetFileNameObjDic(Dictionary<string, string> pathDic)
        {
            Dictionary<string, Object> objDic = new Dictionary<string, Object>();

            List<string> fileNameList = new List<string>(pathDic.Keys);
            for (int i = 0; i < fileNameList.Count; i++)
            {
                objDic.Add(fileNameList[i], AssetDatabase.LoadMainAssetAtPath(pathDic[fileNameList[i]]));
            }
            return objDic;
        }
        static void GetFilePathNamePathDicRecursion(Dictionary<string, string> pathDic, string startPath, string curPath, string[] fileEndArray)
        {
            string[] fileArray = Directory.GetFiles(curPath);
            string[] directoryArray = Directory.GetDirectories(curPath);

            for (int i = 0; i < fileArray.Length; i++)
            {
                for (int j = 0; j < fileEndArray.Length; j++)
                {
                    if (fileArray[i].EndsWith(fileEndArray[j]))
                    {
                        string curFilePath = fileArray[i].Replace("\\", "/");
                        curFilePath = fileArray[i].Substring(startPath.Length + 1);
                        pathDic.Add(curFilePath.Substring(0, curFilePath.Length - fileEndArray[j].Length - 1).Replace("\\", "/"), fileArray[i].Replace(Application.dataPath, "Assets").Replace("\\", "/"));
                        break;
                    }
                }
            }

            for (int i = 0; i < directoryArray.Length; i++)
            {
                GetFilePathNamePathDicRecursion(pathDic, startPath, directoryArray[i], fileEndArray);
            }
        }
//    //文件名：路径 字典  
//    static Dictionary<string, string> GetFileNamePathsDic(string path, string[] fileEndArray)
//    {
//        Dictionary<string, string> pathDic = new Dictionary<string, string>();
//        GetFileNamePathDicRecursion(pathDic, path, path, fileEndArray);
//        return pathDic;
//    }

//    static void GetFileNamePathDicRecursion(Dictionary<string, string> pathDic, string startPath, string curPath, string[] fileEndArray)
//    {
//        string[] fileArray = Directory.GetFiles(curPath);
//        string[] directoryArray = Directory.GetDirectories(curPath);

//        for(int i = 0; i < fileArray.Length; i++)
//        {
//            for(int j = 0; j < fileEndArray.Length; j++)
//            {
//                if(fileArray[i].EndsWith(fileEndArray[j]))
//                {
//                    string curFilePath = fileArray[i].Replace("\\","/");
//                    curFilePath = fileArray[i].Substring(startPath.Length + 1);
//                    pathDic.Add(curFilePath.Substring(0,curFilePath.Length - fileEndArray[j].Length - 1), fileArray[i].Replace(Application.dataPath, "Assets").Replace("\\","/"));
//                    break;
//                }
//            }
//        }

//        for(int i = 0; i < directoryArray.Length; i++)
//        {
//            GetFileNamePathDicRecursion(pathDic, directoryArray[i], directoryArray[i], fileEndArray);
//        }
//    }


//    static string DataResPath = Application.dataPath + "/BundleRes" + "/Data";
//    [MenuItem("Bundle/CreateDataBundle")]
//    public static void CreateDataBundle()
//    {
//        if(Directory.Exists(Application.dataPath + "/Resources/Data"))
//        {
//            FileUtil.DeleteFileOrDirectory(DataResPath);
//            FileUtil.CopyFileOrDirectory(Application.dataPath + "/Resources/Data", DataResPath);
//        }
//        FileUtil.DeleteFileOrDirectory(OutPutPathRoot + "/Data");
//        CreateDataBundle(DataResPath, OutPutPathRoot + "/Data");
//        CopyBundleToStreamPath(Application.dataPath + "/Bundle/Data", StreamingBundleRoot + "/Data");
//    }

//    static void CreateDataBundle(string resPath, string outPutPath)
//    {
//        AssetDatabase.Refresh();
//        Dictionary<string, string> dataPathDic = GetFilePathNamePathsDic(resPath, DataFileEnd);

//        //for(int i = 0; i < localNoUseDataNameList.Count; i++)
//        //{
//        //    if(dataPathDic.ContainsKey(localNoUseDataNameList[i]))
//        //    {
//        //        dataPathDic.Remove(localNoUseDataNameList[i]);
//        //    }
//        //}

//        Dictionary<string, Object> objectDic = GetFileNameObjDic(dataPathDic);
		
//        Object[] objectArray = new Object[objectDic.Count];
//        string[] objectNameArray = new string[objectDic.Count];
//        int objArrayIndex = 0;
		
//        foreach(KeyValuePair<string, Object> obj in objectDic)
//        {
//            objectArray[objArrayIndex] = obj.Value;
//            objectNameArray[objArrayIndex] = obj.Key;
//            objArrayIndex++;
//        }
		
//        BuildAssetBundleOptions optionNormal = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.CollectDependencies;
		
//        BuildPipeline.PushAssetDependencies();
		
		
//        string dataOutPutPath = outPutPath + "/Data.bundle";
//        MyFileUtil.CheckPath(dataOutPutPath);
//        BuildPipeline.BuildAssetBundleExplicitAssetNames(objectArray, objectNameArray, dataOutPutPath, optionNormal, BuildTarget.Android);
//        BuildPipeline.PopAssetDependencies();
		
////		FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath + "/Bundle/Data");
////		FileUtil.CopyFileOrDirectory(outPutPath,Application.streamingAssetsPath + "/Bundle/Data");
//        Debug.Log("Create Data Bundle Done");
//    }

//    //==============================Sound=======================================
//    static string SoundResPath = Application.dataPath + "/BundleRes" + "/Sound";
//    [MenuItem("Bundle/CreateSoundBundle")]
//    public static void CreateSoundBundle()
//    {
//        if(Directory.Exists(Application.dataPath + "/Resources/Sound"))
//        {
//            FileUtil.DeleteFileOrDirectory(SoundResPath);
//            FileUtil.CopyFileOrDirectory(Application.dataPath + "/Resources/Sound", SoundResPath);
//        }

//        FileUtil.DeleteFileOrDirectory(OutPutPathRoot + "/Sound");

//        CreateSoundBundle(SoundResPath, OutPutPathRoot + "/Sound");
//    }
//    static string[] soundFileEnd = new string[] { "mp3", "ogg", "wav", "OGG", "MP3", "WAV" };
//    static void CreateSoundBundle(string resPath, string outPutPath)
//    {
//        Dictionary<string, string> soundPathDic = GetFilePathNamePathsDic(resPath, soundFileEnd);
//        Dictionary<string, Object> soundObjsDic = GetFileNameObjDic(soundPathDic);
//        Dictionary<string, Object> finalObjsDic = new Dictionary<string, Object>();

//        foreach (KeyValuePair<string, string> curObjPath in soundPathDic)
//        {
//            string curKey = curObjPath.Key;
//            if (!soundObjsDic.ContainsKey(curKey))
//            {
//                Debug.LogError(curKey);
//                continue;
//            }

//            //curKey = RecordSourceLevel(curKey, startPath, curObjPath.Value, loadPath);
//            finalObjsDic.Add(curKey, soundObjsDic[curObjPath.Key]);
//        }
//        BuildAssetBundleOptions optionNormal = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DisableWriteTypeTree | BuildAssetBundleOptions.DeterministicAssetBundle;
//        foreach (KeyValuePair<string, Object> obj in finalObjsDic)
//        {
//            BuildPipeline.PushAssetDependencies();
//            string outputPath = outPutPath + "/" + obj.Key + ".bundle";
//            MyFileUtil.CheckPath(outputPath);
//            BuildPipeline.BuildAssetBundle(obj.Value, null, outputPath, optionNormal, BuildTarget.Android);
//            BuildPipeline.PopAssetDependencies();
//        }

//        //FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath + "/Bundle/Sound");
//        //FileUtil.CopyFileOrDirectory(outPutPath, Application.streamingAssetsPath + "/Bundle/Sound");

//        Debug.Log("Create Sound Bundle Done");
//    }
//    static Dictionary<string, string> GetFilePathNamePathsDic(string path, string[] fileEndArray)
//    {
//        Dictionary<string, string> pathDic = new Dictionary<string, string>();
//        GetFilePathNamePathDicRecursion(pathDic, path, path, fileEndArray);
//        return pathDic;
//    }
	
//   

//    static string[] DataFileEnd = new string[]{"txt"};
	
//    static List<string> GetFilePathByRoot(string rootPath, string[] fileEndArray)
//    {
//        List<string> pathList = new List<string>();
//        GetFilePathByRootRecursion(pathList, rootPath, fileEndArray);
//        return pathList;
//    }
	
//    static void GetFilePathByRootRecursion(List<string> pathList, string curPath, string[] fileEndArray)
//    {
//        string[] fileArray = Directory.GetFiles(curPath);
//        string[] directoryArray = Directory.GetDirectories(curPath);
		
//        for(int i = 0; i < fileArray.Length; i++)
//        {
//            for(int j = 0; j < fileEndArray.Length; j++)
//            {
//                if(fileArray[i].EndsWith(fileEndArray[j]))
//                {
//                    pathList.Add(fileArray[i].Replace(Application.dataPath, "Assets").Replace("\\","/"));
//                    break;
//                }
//            }
//        }
		
//        for(int i = 0; i < directoryArray.Length; i++)
//        {
//            GetFilePathByRootRecursion(pathList, directoryArray[i], fileEndArray);
//        }
//    }
	
//    static List<Object> GetObjectByPathList(List<string> pathList)
//    {
//        List<Object> objList = new List<Object>();
		
//        for(int i = 0; i < pathList.Count; i++)
//        {
//            objList.Add(AssetDatabase.LoadMainAssetAtPath(pathList[i]));
//        }
//        return objList;
//    }
	
}