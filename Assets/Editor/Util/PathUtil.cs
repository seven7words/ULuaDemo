using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// 注释：路径相关工具
/// </summary>
public class PathUtil
{
	/// <summary>
	/// 公共图集
	/// </summary>
	public static string[] Atlases =
	{
		"comm_Atlas",
		"base_Atlas",
		"ui_battle_Atlas",
		"General_icon_Atlas"
	};
	public static string[] atlasHZ =
	{
		".prefab",
		".png",
		".mat"
	};

	//------------------------------后缀---------------------------------------------
	public static string Prefab = "prefab";
	public static string SuffixPrefab = ".prefab";

	public static string Png = "png";
	public static string SuffixPng = ".png";

	public static string Assets = "Assets";

	//------------------------------地址----------------------------------------------
	public static string appPath = Application.dataPath + "/";

	public static string luaPath = appPath + "Lua/";

	public static string csvPath = appPath + "_lang/csv/";

	public static string fontPath = appPath + "_lang/font/";

	public static string atlasPath = appPath + "prefabs/atlas/";

	public static string modulesPath = appPath + "prefabs/modules/";

	public static string UIPath = appPath + "prefabs/ui/";

	public static string texPath = appPath + "textures/";

	public static string luaAssetPath = appPath + "prefabs/lua/";
	public static string protobufCSVPath = appPath + "_lang/csv/protobuf_pbs.csv";
	public static string protobufPbPath = appPath + "Lua/game/network/";
	/// <summary>ABs/</summary>
	public static string abs = appPath + "ABs/";
	public static string verPath
	{
		get
		{
			return appPath + "ABs/files.txt";
		}
	}
	/// <summary>
	/// 获取unity下相对路径
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string GetAssetPath(string path)
	{
		path = path.Replace("\\", "/");
		string localPath = Application.dataPath.Replace("Assets", "");

		return path.Replace(localPath, "");
	}
	/// <summary>
	/// 获取系统绝对路径
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string GetUtterAssetPath(string path)
	{
		string localPath = Application.dataPath.Replace("Assets", "");
		return localPath + path;
	}
	/// <summary>
	/// 去掉后缀
	/// </summary>
	/// <param name="assetPath"></param>
	/// <returns></returns>
	public static string GetFileNameWithoutExt(string assetPath)
	{
		string fileName = assetPath.Replace("Assets/", "");
		int index = fileName.IndexOf(".");
		if (index != -1)
			fileName = fileName.Substring(0, index);//去掉后缀
		return fileName;
	}

	public static string GetWithoutExt(string fileName)
	{
		int index = fileName.IndexOf(".");
		if (index != -1)
			fileName = fileName.Substring(0, index);//去掉后缀
		return fileName;
	}

	public static string GetFileName(string assetPath)
	{
		assetPath = assetPath.Replace("\\", "/");
		int s = assetPath.LastIndexOf("/") + 1;
		int e = assetPath.LastIndexOf(".");
		string fileName = assetPath.Substring(s, e - s);
		return fileName;
	}

	public static string GetRoute(string assetPath)
	{
		string d = assetPath.Replace("Assets/Textures/", "");
		d = d.Substring(0, d.LastIndexOf("."));
		return d;
	}
	public static string GetAbName(string name)
	{
		return name.Replace("/", "@").ToLower();
	}
	public static string RestoreAbName(string abName)
	{
		return abName.Replace("@", "/");
	}
	public static FileInfo[] GetFiles(string path, string pattern)
	{
		List<FileInfo> list = new List<FileInfo>();
		DirectoryInfo dir = new DirectoryInfo(path);
		foreach (FileInfo file in dir.GetFiles(pattern, SearchOption.AllDirectories))
		{
			if (file.Extension != ".meta")
				list.Add(file);
		}
		return list.ToArray();
	}

	/// <summary>
	/// 是否是公共图集
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static bool isCommAtlas(string path)
	{

		if (path.Contains(SuffixPng) || path.Contains(".mat"))
			return false;
		for (int i = 0; i < Atlases.Length; i++)
		{
			if (path.Contains(Atlases[i]))
			{
				return true;
			}
		}
		return false;
	}

}
