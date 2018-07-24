using UnityEngine;
using System.Collections;

public class StrDictionary  {

	public static string GetClientDictionaryString(string key,params object []args)
	{
		if (string.IsNullOrEmpty(key))
		{
			return "Empty ---key erro!";
		}
		if (key.Length < 3)
        {
           return key+" -- ServerDictionaryFormat ERROR2 Length < 3";
        }
        string dicIdStr = key.Substring(2, key.Length - 3);

		try
		{
			string str=string.Format(LocalizationManager.Get(dicIdStr),args);
			return str.Replace("#r","\n");
		}
		catch(System.Exception e)
		{
			return "formate erro!";
		}
	} 
	public static string[] ParseArgs(string[] all)
	{
			if (all==null)
			{
					return null;
			}
			string []a=new string[all.Length];
			for(int i =0;i<all.Length;i++)
			{
					string str=all[i];
					if (!string.IsNullOrEmpty(str))
					{
							if (str[0]=='#')
							{
									a[i]=GetClientDictionaryString(str);
									continue;
							}
					}
					a[i]=all[i];
			}
			return a;
	}
	public static string GetServerDictionaryString(string keystr)
	{  
		if (string.IsNullOrEmpty(keystr))
		{
			return "Empty ---key erro!";
		}
		char firstchar=keystr[0];
		if (firstchar!='#')
		{
			return keystr;
		}
		int dicEndPos = keystr.IndexOf('*');
		if (dicEndPos > 0) //#{12345}*hello*newWorld          #{12345}*#{123}*#{1234}
            {
                string dictionaryStr = keystr.Substring(0, dicEndPos);
                string elementStr = keystr.Substring(dicEndPos + 1, keystr.Length - dicEndPos - 1);
				string[] allElements = ParseArgs(elementStr.Split('*'));
				
                return GetClientDictionaryString(dictionaryStr, allElements);
            }
            else // #{12345} 
            {
                if (keystr.Length < 3)
                {
                    return keystr+" -- ServerDictionaryFormat ERROR2 Length < 3";
                }
                string dicIdStr = keystr.Substring(2, keystr.Length - 3);
				string str=LocalizationManager.Get(dicIdStr);
				return str.Replace("#r","\n");
            }
            return keystr+" -- ServerDictionaryFormat ERROR3";
	}
	public static string GetDictionaryString(string keystr,params object []args)
	{
		string str="";
		if (!string.IsNullOrEmpty(keystr))
		{
			char firstChar=keystr[0];
			if (firstChar!='#')
			{
				str=string.Format(keystr,args);
			}
			else
			{
				str=StrDictionary.GetClientDictionaryString(keystr,args);
			}
		}
		return str;
	}
}
