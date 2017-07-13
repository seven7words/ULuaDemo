using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
public class Export {
    //--------------csv----------------
    static void changeLuaToUTF8(string luaPath){
        DirectoryInfo dir = new DirectoryInfo(luaPath);
        foreach (FileInfo file in dir.GetFiles("*.lua.*",SearchOption.AllDirectories)){
            if (file.Extension!=".meta"){
                string path = file.FullName.Replace('\\', '/');
                string content = File.ReadAllText(path);
                content = content.Replace("\r\n", "\n");
				//UTF8Encoding的参数一定要为false，表示不省略BOM，即Byte Order Mark，也即字节流标记，它是用来让应用程序识别所用的编码的
				using (var sw = new StreamWriter(path, false, new UTF8Encoding(false)))
                {
                    sw.Write(content);
                    sw.Close();

                }
                Debug.Log("Encode file::>>"+path+"OK!");
            }
        }
        Debug.Log("转换完成");
    }
    [MenuItem("Export/配置表/更新")]
    static void updateCsv(){
		//转换为Utf8编码，存入临时文件夹
		string tmpDir = Application.dataPath + "/_lang/tmp/";
        if (Directory.Exists(tmpDir))
            Directory.Delete(tmpDir, true);
        Directory.CreateDirectory(tmpDir);
        DirectoryInfo dir = new DirectoryInfo(PathUtil.csvPath);

        foreach (FileInfo file in dir.GetFiles("*.csv",SearchOption.AllDirectories)){
            using(StreamReader sr = new StreamReader(file.FullName, Encoding.Default, false)){
                byte[] bytes = Encoding.Default.GetBytes(sr.ReadToEnd());
                sr.Close();
                byte[] data = Encoding.Convert(Encoding.Default, Encoding.UTF8, bytes);
                string str = Encoding.UTF8.GetString(data);
                string writePath = tmpDir + file.Name;
                using (StreamWriter sw = new StreamWriter(writePath, false, Encoding.UTF8)){
                    sw.Write(str);
                    sw.Close();
                }
                Debug.Log("write to" +writePath);
            }
        }
        //转为lua表
        Debug.Log("start to pack to lua !!!");
        string path = Application.dataPath + "/_lang/PackTool.bat";
        System.Diagnostics.Process proc = System.Diagnostics.Process.Start(path);
        proc.WaitForExit();
        //删除临时文件
        Directory.Delete(tmpDir, true);
        changeLuaToUTF8(PathUtil.luaPath+"game/configs/");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
	[MenuItem("Export/资源/图集")]
	static void UpdateAtlas()
	{
		DirectoryInfo info = new DirectoryInfo(PathUtil.atlasPath);
		Dictionary<string, string[]> dic = VersionUtil.getVerDic;
		//List<string> oldDic = new List<string>();
		//foreach (string key in dic.Keys)
		//{
		//	if (key.Contains("prefabs/atlas/"))
		//	{
		//		string path = PathUtil.appPath + key;
		//		if (!File.Exists(path + ".prefab"))
		//		{
		//			Debug.LogError("删除空节点： " + key);
		//			oldDic.Add(key);
		//		}
		//	}
		//}

		foreach (FileInfo file in info.GetFiles("*.prefab"))
		{
			string atlas = PathUtil.GetAssetPath(file.FullName);

			string[] atlass = AssetDatabase.GetDependencies(new string[] { atlas });
			List<byte> bytes = new List<byte>();
			for (int i = 0; i < atlass.Length; i++)
			{
                if(!atlass[i].Contains(".cs"))
				    bytes.AddRange(File.ReadAllBytes(PathUtil.GetUtterAssetPath(atlass[i])));
			}
			string md5 = MD5Util.GetMD5Hash(bytes.ToArray());

			//更新配置表ab值
			string atlasPath = PathUtil.GetFileNameWithoutExt(atlas);
			string[] data = new string[] { atlasPath, md5, "0", "0" };
			dic[atlasPath] = data;
			string name = PathUtil.GetAbName(atlasPath);
			SetAssetbundleNames(atlass, "atlas/" + name);
		}
		AssetDatabaseUtil.Clear();
		VersionUtil.writeVersion(dic);
		Debug.Log("更新图集Assetbundle值完毕！");
	}
	/// <summary>
	/// 批量设置Assetbundle值
	/// </summary>
	/// <param name="paths"></param>
	/// <param name="name"></param>
	private static void SetAssetbundleNames(string[] paths, string name)
	{
		foreach (string path in paths)
		{
			if (!path.Contains(".cs"))
			{
				AssetDatabaseUtil.SetName(path, name);
			}
		}
	}
    [MenuItem("Export/Test")]
    private static void test(){
        BuildABs("ABs/Android",BuildTarget.iOS);
    }
	public static void BuildABs(string outPath, BuildTarget target)
	{
		string path = Application.dataPath + "/"+outPath;
        Debug.Log(path);
        if (!Directory.Exists(path)){
            Directory.CreateDirectory(path);
        }
        BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle;
		BuildPipeline.BuildAssetBundles("Assets/" + outPath, options, target);
		AssetDatabase.Refresh();

	}
}
