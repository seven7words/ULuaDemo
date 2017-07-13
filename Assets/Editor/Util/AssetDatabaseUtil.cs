using UnityEngine;
using UnityEditor;
using System.Collections;
public class AssetDatabaseUtil
{
	public static void SetName(string path, string name)
	{
		AssetImporter imp = AssetImporter.GetAtPath(path);
		imp.assetBundleName = name;
	}

	public static void SetName(Object obj, string name)
	{
		SetName(AssetDatabase.GetAssetPath(obj), name);
	}

	public static void Clear()
	{
		AssetDatabase.RemoveUnusedAssetBundleNames();
	}


}
