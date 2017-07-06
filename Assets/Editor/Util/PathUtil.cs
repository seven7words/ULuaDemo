using UnityEngine;
using System.IO;
using System.Collections.Generic;
//路径相关工具
public class PathUtil
{
    public static string Prefab = "prefab";
    public static string SuffixPrefab = ".prefab";

    public static string Png = "png";
    public static string SuffixPng = ".png";

    public static string Assets = "Assets";

    public static string appPath = Application.dataPath + "/";

    public static string luaPath = Application.dataPath + "/Lua/";

    public static string csvPath = Application.dataPath + "/_lang/csv/";

	public static string fontPath = Application.dataPath + "/_lang/font/";

	public static string atlasPath = Application.dataPath + "/Prefabs/Atlas/";

	public static string modulesPath = Application.dataPath + "/Prefabs/Modules";

	public static string UIPath = Application.dataPath + "/Prefabs/UI";

	public static string texPath = Application.dataPath + "/Textures/";

    public static string verPath{
        get{
            return appPath + "ABs/files.txt";
        }
    }
    public static string GetAssetPath(string path){
        path = path.Replace("\\", "/");
        string localPath = Application.dataPath.Replace("Assets", "");
        return path.Replace(localPath, "");
    }
    public static string GetFileNameWithoutExt(string assetPath){
        string fileName = assetPath.Replace("Assets/", "");
        int index = fileName.IndexOf(".");
        if(index!=-1){
            fileName = fileName.Substring(0, index);//去掉后缀

        }
        return fileName;
    }
    public static string GetWithoutExt(string fileName){
		int index = fileName.IndexOf(".");
		if (index != -1)
		{
			fileName = fileName.Substring(0, index);//去掉后缀

		}
		return fileName;
    }
    public static string GetFileName(string assetPath){
        int s = assetPath.LastIndexOf("/") + 1;
        int e = assetPath.LastIndexOf(".");
        string fileName = assetPath.Substring(s, e - s);
        return fileName;
    }
    public static string GetRoute(string assetPath){
        string d = assetPath.Replace("Assets/Textures/", "");
        d = d.Substring(0, d.LastIndexOf("."));
        return d;
    }
    public static FileInfo[] GetFiles(string path,string pattern){
        List<FileInfo> list = new List<FileInfo>();
        DirectoryInfo dir = new DirectoryInfo(path);
        foreach (FileInfo file in dir.GetFiles(pattern,SearchOption.AllDirectories)){
            if (file.Extension!=".meta"){
                list.Add(file);
            }
        }
        return list.ToArray();
    }

}
