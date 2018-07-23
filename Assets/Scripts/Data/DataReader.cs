using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;

public static class DataReader
{

    public static byte[] GetBytes(object obj)
    {
        TextAsset textAsset = obj as TextAsset;
        if (textAsset == null)
            return null;
        return textAsset.bytes;
    }
    public static List<T> LoadImportData<T>(string fileName)
    {
#if 	UNITY_EDITOR
        object obj = Resources.Load("Data/" + fileName);
        if (obj == null)
        {
            obj = BundleManager.LoadTable(fileName);
            if (obj == null)
            {
                Debug.LogWarning("not found file :" + fileName);
                return null;
            }
        }
#else
        object obj = BundleManager.LoadTable(fileName);
        if (obj == null)
        {
            obj = Resources.Load("Data/" + fileName);
            if (obj == null)
            {
                return null;
            }
        }
#endif

        ByteReader reader = new ByteReader(GetBytes(obj));
        BetterList<string> keyList = reader.ReadCSV();

        while (keyList != null)
        {
            if (keyList[0].Contains("*"))
            {
                keyList.RemoveAt(0);
                break;
            }
            keyList = reader.ReadCSV();
        }
        List<T> mDataList = new List<T>();
        int nameLen = keyList.size;
        FieldInfo[] fieldInfo = new FieldInfo[nameLen];
        for (int i = 0; i < nameLen; i++)
        {
            fieldInfo[i] = typeof(T).GetField(keyList[i]);
        }

        BetterList<string> temp;
        temp = reader.ReadCSV();
        while (temp != null)
        {
            if (string.IsNullOrEmpty(temp[0]) || !temp[0].Contains("*"))
            {
                temp = reader.ReadCSV();
                continue;
            }
            temp.RemoveAt(0);
            {
                T t = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
                for (int j = 0; j < temp.size; j++)
                {
                    object v = null;
                    string s = temp[j];

                    if (string.IsNullOrEmpty(s) || fieldInfo[j] == null)
                        continue;
                    if (fieldInfo[j].FieldType.IsEnum)
                    {
                        try
                        {
                            v = Enum.Parse(fieldInfo[j].FieldType, s);
                        }
                        catch (Exception ex)
                        {
                           Debug.Log("error datafile=" + fileName + " fieldinfo=" + fieldInfo[j].Name + " fieldinfoValue=" + s);
                        }
                    }
                    else
                    {
                        try
                        {
                            v = Convert.ChangeType(s, fieldInfo[j].FieldType);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("error datafile=" + fileName + " fieldinfo=" + fieldInfo[j].Name + " fieldinfoValue=" + s + "  ID " + temp[0]);
                        }
                    }
                    fieldInfo[j].SetValue(t, v);

                }
                mDataList.Add(t);
            }
            temp = reader.ReadCSV();
        }
        return mDataList;
    }


    public static List<T> LoadImportDicData<T>(string fileName, string DicDataName)
    {
#if UNITY_EDITOR
		object obj = Resources.Load("Data/" + fileName);
		if (obj == null)
		{
            obj = BundleManager.LoadTable(fileName);
            if (obj == null)
			{
				return null;
			}
		}
#else
        object obj = BundleManager.LoadTable(fileName);
        if (obj == null)
        {
            obj = Resources.Load("Data/" + fileName);
            if (obj == null)
            {
                return null;
            }
        }
#endif
        ByteReader reader = new ByteReader(GetBytes(obj));
        BetterList<string> tempKeyList = reader.ReadCSV();

        while (tempKeyList != null)
        {
            if (tempKeyList[0].Contains("*"))
            {
                tempKeyList.RemoveAt(0);
                break;
            }
            tempKeyList = reader.ReadCSV();
        }
        List<string> keyList = new List<string>();
        for (int i = 0; i < tempKeyList.size; i++)
        {
            keyList.Add(tempKeyList[i]);
        }

        List<T> mDataList = new List<T>();
        int nameLen = keyList.Count;
        FieldInfo[] fieldInfo = new FieldInfo[nameLen];
        for (int i = 0; i < nameLen; i++)
        {
            fieldInfo[i] = typeof(T).GetField(keyList[i]);
        }
        FieldInfo TDic = typeof(T).GetField(DicDataName);

        BetterList<string> temp;
        temp = reader.ReadCSV();
        while (temp != null)
        {
            if (string.IsNullOrEmpty(temp[0]) || !temp[0].Contains("*"))
            {
                temp = reader.ReadCSV();
                continue;
            }
            temp.RemoveAt(0);
            {
                T t = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
                Dictionary<string, string> dic = TDic.GetValue(t) as Dictionary<string, string>;

                for (int j = 0; j < temp.size; j++)
                {
                    object v = null;
                    string s = temp[j];

                    if (keyList[j].StartsWith("_"))
                    {
                        dic.Add(keyList[j], s);
                        //						continue;
                    }

                    if (string.IsNullOrEmpty(s) || fieldInfo[j] == null)
                        continue;
                    if (fieldInfo[j].FieldType.IsEnum)
                    {
                        try
                        {
                            v = Enum.Parse(fieldInfo[j].FieldType, s);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("error datafile=" + fileName + " fieldinfo=" + fieldInfo[j].Name + " fieldinfoValue=" + s);
                        }
                    }
                    else
                    {
                        try
                        {
                            v = Convert.ChangeType(s, fieldInfo[j].FieldType);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("error datafile=" + fileName + " fieldinfo=" + fieldInfo[j].Name + " fieldinfoValue=" + s + "  ID " + temp[0]);
                        }
                    }
                    fieldInfo[j].SetValue(t, v);

                }
                mDataList.Add(t);
            }
            temp = reader.ReadCSV();


        }
        return mDataList;
    }

    //	public static Dictionary<string, T> LoadTable<T>(string fileName, string keyName) where T : class
    //	{   
    //		List<T>list=LoadImportData<T>(fileName);
    //		return LoadTable<T>(list,keyName);
    //	}

    public static Dictionary<T1, T2> LoadDicTable<T1, T2>(string fileName, string keyName, string dicName) where T2 : class
    {
        List<T2> list = LoadImportDicData<T2>(fileName, dicName);
        return LoadTable<T1, T2>(list, keyName);
    }
    public static Dictionary<T1, T2> LoadTable<T1, T2>(string fileName, string keyName) where T2 : class
    {
        List<T2> list = LoadImportData<T2>(fileName);
        return LoadTable<T1, T2>(list, keyName);
    }
    public static Dictionary<T1, T2> LoadTable<T1, T2>(List<T2> list, string keyName) where T2 : class
    {
		if(list != null)
		{
			Dictionary<T1, T2> dictionary = new Dictionary<T1, T2>(list.Count);
			FieldInfo field = typeof(T2).GetField(keyName);
			if (field != null&&list!=null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					T2 t = list[i];
					T1 v = (T1)field.GetValue(t);
					try
					{
						dictionary.Add(v, t);
					}
					catch (Exception e)
					{   
						Debug.Log(field.Name+" "+keyName);
						Debug.Log(typeof(T2).Name);
						Debug.Log(i.ToString());
						Debug.LogError(v);
					}
					
				}
			}
			return dictionary;
		}
		else
		{
			return null;
		}
    }
    public static Dictionary<T1, List<T2>> LoadTableList<T1, T2>(string fileName, string keyName) where T2 : class
    {
        List<T2> list = LoadImportData<T2>(fileName);
        return LoadTableList<T1, T2>(list, keyName);
    }
    public static Dictionary<T1, List<T2>> LoadTableList<T1, T2>(List<T2> list, string keyName) where T2 : class
    {
        Dictionary<T1, List<T2>> dictionary = new Dictionary<T1, List<T2>>();
        FieldInfo field = typeof(T2).GetField(keyName);
        if (field != null&&list!=null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T2 t = list[i];
                T1 v = (T1)field.GetValue(t);
                List<T2> outlist = null;
                if (!dictionary.TryGetValue(v, out outlist))
                {
                    outlist = new List<T2>();
                    dictionary.Add(v, outlist);
                }
                outlist.Add(t);
            }
        }
        return dictionary;
    }

    public static T2 GetTableRow<T1, T2>(Dictionary<T1, T2> dataDict, T1 key) where T2 : class
    {
		if(dataDict != null)
		{
			T2 result;
			if (dataDict.TryGetValue(key, out result))
			{
				//			Debug.Log("find data");
				return result;
			}
		}
        return (T2)((object)null);
    }
    public static List<T2> GetTableList<T1, T2>(Dictionary<T1, List<T2>> dataDict, T1 key) where T2 : class
    {
        List<T2> result;
        if (dataDict.TryGetValue(key, out result))
        {
            return result;
        }
        return null;
    }
}
