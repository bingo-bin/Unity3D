using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalizationManager  {
	
	static Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();
	static int mLanguageIndex = -1;
	static string mLanguage;
	public static bool ResourceLoadDictionary ()
	{   
		TextAsset txt = Resources.Load("LocalizationBase", typeof(TextAsset)) as TextAsset;
		SystemLanguage syslanguage=Application.systemLanguage;
		string language = PlayerPrefs.GetString("Language", "English");
		if (txt != null && LoadCSV(txt)) 
		{   
		    SelectLanguage(language);
			return true;
		}
		#if UNITY_EDITOR
			Debug.LogWarning("Localization file not found!");
		#endif
		return false;
	}
	public static bool BundleLoadDictionary ()
	{   
#if UNITY_EDITOR
		TextAsset txt = Resources.Load("Data/Localization", typeof(TextAsset)) as TextAsset;
		if(txt == null)
		{
			txt = BundleManager.LoadTable("Localization")as TextAsset;
		}
#else
		TextAsset txt = BundleManager.LoadTable("Localization")as TextAsset;
        if(txt == null)
		{
			txt = Resources.Load("Data/Localization", typeof(TextAsset)) as TextAsset;
		}
#endif
        SystemLanguage syslanguage=Application.systemLanguage;
		string language = PlayerPrefs.GetString("Language", "English");

		if (txt != null && LoadCSV(txt)) 
		{   
			SelectLanguage(language);
			return true;
		}
		#if UNITY_EDITOR
		Debug.LogWarning("Localization file not found!");
		#endif
		return false;
	}

	static bool LoadAndSelect (string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			if (mDictionary.Count == 0 && !BundleLoadDictionary()) return false;
			if (SelectLanguage(value)) return true;
		}
		return false;
	}
	static public bool LoadCSV (TextAsset asset)
	{
		ByteReader reader = new ByteReader(asset);
		BetterList<string> temp = reader.ReadCSV();
		if (temp.size < 2) return false;
		temp[0] = "KEY";
		mDictionary.Clear();
		while (temp != null)
		{
			AddCSV(temp);
			temp = reader.ReadCSV();
		}

		return true;
	}
	static bool SelectLanguage (string language)
	{
		mLanguageIndex = -1;
		if (mDictionary.Count == 0) return false;
		string[] keys;

		if (mDictionary.TryGetValue("KEY", out keys))
		{  
			for (int i = 0; i < keys.Length; ++i)
			{   
				if (keys[i] == language)
				{
					mLanguageIndex = i;
					mLanguage = language;
					PlayerPrefs.SetString("Language", mLanguage);
					UIRoot.Broadcast("OnLocalize");
					return true;
				}
			}
		}
		return false;
	}
	static void AddCSV (BetterList<string> values)
	{
		if (values.size < 2) return;
		string[] temp = new string[values.size - 1];
		for (int i = 1; i < values.size; ++i) temp[i - 1] = values[i];

		if(mDictionary.ContainsKey(values[0]))
			Debug.LogWarning(values[0]);

			mDictionary.Add(values[0], temp);
	}
	static public string Get (string key)
	{
		// Ensure we have a language to work with
		string[] vals;
		if (mLanguageIndex != -1 && mDictionary.TryGetValue(key, out vals))
		{
			if (mLanguageIndex < vals.Length)
				return vals[mLanguageIndex];
		}
		#if UNITY_EDITOR
			Debug.LogWarning("Localization key not found: '" + key + "'");
		#endif
		return key;
	}

	/// <summary>
	/// Localize the specified value.
	/// </summary>

	[System.Obsolete("Use Localization.Get instead")]
	static public string Localize (string key) { return Get(key); }

	/// <summary>
	/// Returns whether the specified key is present in the localization dictionary.
	/// </summary>

	static public bool Exists (string key)
	{
		if (mLanguageIndex != -1) return mDictionary.ContainsKey(key);
		return false;
	}
}
