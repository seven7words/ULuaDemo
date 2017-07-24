using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Linq;
using System.Net;
using Ionic.Zlib;
public class Export {
    #region [CSV转Lua表]
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
    #endregion
    #region 资源assetbundle更新

    [MenuItem("Export/资源/字体")]
    static void UpdateFont()
    {
        DirectoryInfo info = new DirectoryInfo(PathUtil.fontPath);
        Dictionary<string, string[]> dic = VersionUtil.getVerDic;
        List<string> oldDic = new List<string>();
        foreach (string key in dic.Keys)
        {
            if (key.Contains("_lang/font/"))
            {
                string path = PathUtil.appPath + key;
                if (!File.Exists(path + ".ttf"))
                {
                    oldDic.Add("删除空节点：" + key);
                }
            }
        }
        dic = deleteDic(oldDic, dic);
        foreach (FileInfo file in info.GetFiles("*.ttf"))
        {
            string atlas = PathUtil.GetAssetPath(file.FullName);
            string[] atlass = AssetDatabase.GetDependencies(new string[] { atlas });
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < atlass.Length; i++)
            {
                bytes.AddRange(File.ReadAllBytes(PathUtil.GetUtterAssetPath(atlass[i])));
            }
            string md5 = MD5Util.GetMD5Hash(bytes.ToArray());
            //更新配置表ab值
            string atlasPath = PathUtil.GetFileNameWithoutExt(atlas);
            string[] data = new string[] { atlasPath, md5, "0", "0" };
            dic[atlasPath] = data;
            string name = PathUtil.GetAbName(atlasPath);
            Debug.Log(name);
            SetAssetbundleNames(atlass, "font/" + name);
        }
        AssetDatabaseUtil.Clear();
        VersionUtil.writeVersion(dic);
        Debug.Log("更新字体值完毕");

    }

    [MenuItem("Export/资源/图集")]
    static void UpdateAtlas()
    {
        DirectoryInfo info = new DirectoryInfo(PathUtil.atlasPath);
        Dictionary<string, string[]> dic = VersionUtil.getVerDic;
        List<string> oldDic = new List<string>();
        foreach (string key in dic.Keys)
        {
            if (key.Contains("prefabs/atlas/"))
            {
                string path = PathUtil.appPath + key;
                if (!File.Exists(path + ".prefab"))
                {
                    Debug.LogError("删除空节点： " + key);
                    oldDic.Add(key);
                }
            }
        }
        dic = deleteDic(oldDic, dic);

        foreach (FileInfo file in info.GetFiles("*.prefab"))
        {
            string atlas = PathUtil.GetAssetPath(file.FullName);

            string[] atlass = AssetDatabase.GetDependencies(new string[] { atlas });
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < atlass.Length; i++)
            {
               
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

    

    [MenuItem("Export/资源/精灵图片")]
    static void UpdateTexture()
    {
        Dictionary<string, string[]> vDic = VersionUtil.getVerDic;
        DirectoryInfo info = new DirectoryInfo(PathUtil.texPath);
        List<string> oldDic = new List<string>();
        foreach (string key in vDic.Keys)
        {
            if (key.Contains("textures/"))
            {
                string path = PathUtil.appPath + key;
                if (!File.Exists(path + ".png"))
                {
                    Debug.LogError("删除空节点："+key);
                    oldDic.Add(key);
                }
            }
        }
        vDic = deleteDic(oldDic, vDic);
        foreach (FileInfo file in info.GetFiles("*.png",SearchOption.AllDirectories))
        {
            string path = file.FullName;
            string assetPath = PathUtil.GetAssetPath(path);
            AssetDatabaseUtil.SetName(assetPath,"");//meta上ab会影响打包，先清理掉
            List<byte> list = new List<byte>();
            //贴图可能会更改压缩格式，这时需要重新打包
            string metaFullPath = path + ".meta";
            byte[] metaDatas = File.ReadAllBytes(metaFullPath);
            list.AddRange(metaDatas);
            byte[] datas = File.ReadAllBytes(path);
            list.AddRange(datas);
            string hash = MD5Util.GetMD5Hash(list.ToArray());
            string fileName = PathUtil.GetFileNameWithoutExt(assetPath);
            string[] itemArr = new string[]{fileName,hash,"0","0"};
            string name = PathUtil.GetAbName(fileName);
            AssetDatabaseUtil.SetName(assetPath,"tex/"+name);
            vDic[fileName] = itemArr;
        }
        AssetDatabaseUtil.Clear();
        VersionUtil.writeVersion(vDic);
        Debug.Log("更新精灵图片Assetbundle值完毕！");
    }

    [MenuItem("Export/资源/模块预设")]
    static void UpdatePrefabs()
    {
        DirectoryInfo info = new DirectoryInfo(PathUtil.modulesPath);
        Dictionary<string, string[]> dic = VersionUtil.getVerDic;
        List<string> oldDic = new List<string>();
        foreach (string key in dic.Keys)
        {
            if (key.Contains("prefabs/modules/"))
            {
                string path = PathUtil.appPath + key;
                if (!File.Exists(path + ".prefab"))
                {
                    Debug.LogError("删除空节点："+key);
                    oldDic.Add(key);
                }
            }
        }
        dic = deleteDic(oldDic, dic);
        foreach (FileInfo file in info.GetFiles("*.prefab"))
        {
            string atlas = PathUtil.GetAssetPath(file.FullName);
            string[] atlass = AssetDatabase.GetDependencies(new string[] {atlas});
            List<byte> bytes = new List<byte>();
            List<string> modules = new List<string>();
            for (int i = 0; i < atlass.Length; i++)
            {
                if (atlass[i].IndexOf(".prefab") != -1 && atlass[i].IndexOf("Prefabs/atlas") == -1 &&
                    atlass[i].IndexOf(".png") == -1)
                {
                    modules.Add(atlass[i]);
                    bytes.AddRange(File.ReadAllBytes(PathUtil.GetUtterAssetPath(atlass[i])));
                }
            }
            string md5 = MD5Util.GetMD5Hash(bytes.ToArray());
            //更新配置表ab值
            string atlasPath = PathUtil.GetFileNameWithoutExt(atlas);
            string[] data = new string[] { atlasPath, md5, "0", "0" };
            dic[atlasPath] = data;
            string name = PathUtil.GetAbName(atlasPath);
            SetAssetbundleNames(modules.ToArray(), "modules/" + name);
        }
        AssetDatabaseUtil.Clear();
        VersionUtil.writeVersion(dic);
        Debug.Log("更新模块预设Assetbundle值完毕！");
    }

    [MenuItem("Export/Lua/更新AB值")]

    static void UpdateLua()
    {
        if (Directory.Exists(PathUtil.luaAssetPath))
        {
            Directory.CreateDirectory(PathUtil.luaAssetPath);
            //Directory.Delete(PathUtil.luaAssetPath,true);
        }

        DirectoryInfo info = new DirectoryInfo(PathUtil.luaPath);
        Dictionary<string, string[]> vDic = VersionUtil.getVerDic;
        List<string> oldDic = new List<string>();
        foreach (string key in vDic.Keys)
        {
            if (key.Contains("Lua/"))
            {
                string path = PathUtil.appPath + key;
                if (!File.Exists(path + ".lua") && !File.Exists(path + ".pb") && !File.Exists(path + ".proto"))
                {
                    oldDic.Add("删除空节点： " + key);
                }
            }
        }
        vDic = deleteDic(oldDic, vDic);

        List<string> hashs = new List<string>();
        foreach (FileInfo file in info.GetFiles("*.lua", SearchOption.AllDirectories))
        {
            if (!file.FullName.Contains(".meta"))
            {
                hashs.Add(AddLuaScriptAsset(file.FullName, vDic));
            }
        }
        foreach (FileInfo file in info.GetFiles("*.pb", SearchOption.AllDirectories))
        {
            if (!file.FullName.Contains(".meta"))
            {
                hashs.Add(AddLuaScriptAsset(file.FullName, vDic));
            }
        }

        //清理已删除的脚本的asset文件
        info = new DirectoryInfo(PathUtil.luaAssetPath);
        foreach (FileInfo file in info.GetFiles("*.asset", SearchOption.AllDirectories))
        {
            if (!hashs.Contains(file.Name))
            {
                string p = string.Format("Assets/prefabs/lua/{0}", file.Name);
                AssetDatabase.DeleteAsset(p);
                Debug.Log("delete asset  " + file.Name);
            }
        }

        VersionUtil.writeVersion(vDic);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private static Dictionary<string, string[]> deleteDic(List<string> keys, Dictionary<string, string[]> dic)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            dic.Remove(keys[i]);
        }
        return dic;
    }

    private static string AddLuaScriptAsset(string filePath, Dictionary<string, string[]> dic)
    {
        //1.得到md5
        //2.判断是否已写入配置表
        //3.如果写入 判断lua脚本是否发生改变，如果md5相等则返回hash
        //4.生成asset文件到Asset/prefab/lua目录下，并设置引用
        //5.更新配置信息键值
        string path = PathUtil.GetFileNameWithoutExt(PathUtil.GetAssetPath(filePath)) ;
        byte[] data = File.ReadAllBytes(filePath);
        //byte[] meta = File.ReadAllBytes(filePath+".meta");
        List<byte> datas = new List<byte>();
        datas.AddRange(data);
       // datas.AddRange(meta);
        string hash = MD5Util.GetMD5Hash(datas.ToArray());
        if (dic.ContainsKey(path))
        {
            string[] news = dic[path];
            if (hash == news[1])
            {
                string old1Path = string.Format("Assets/prefabs/lua/{0}.asset", hash);

                if (File.Exists(old1Path))
                {

                    return hash+".asset";
                }

            }
            else
            {
                string oldPath = string.Format("Assets/prefabs/lua/{0}.asset", news[1]);
                AssetDatabase.DeleteAsset(oldPath);
                Debug.Log(path + "oladddd" + oldPath);
            }
            
        }
        ScriptAsset asset = new ScriptAsset();
        asset.fileName = path;
        asset.data = data;
        int size = data.Length;
        string assetPath = string.Format("Assets/prefabs/lua/{0}.asset",hash);
       // Debug.Log(path+"      生成"+size);
        AssetDatabase.CreateAsset(asset,assetPath);
        AssetDatabaseUtil.SetName(assetPath,"lua/"+hash);
        string[] item = new string[]
        {
            path,
            hash,
            "0",
            "0"
        };
        dic[path] = item;
        Debug.Log(path);
        return hash + ".asset";
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
    #endregion

    #region 打包压缩

    [MenuItem("Export/更新配置文件")]
    private static void abInit()
    {
        UpdateFont();
        UpdateAtlas();
        UpdatePrefabs();
        UpdateTexture();
        UpdateLua();
    }
    [MenuItem("Export/Build/Window")]
    private static void WindowToAB()
    {
        abInit();
        BuildABs("Window", BuildTarget.StandaloneWindows);
        UpdateABsSize("Window");
        ClearAbs("Window");
        toWindowAB();
    }
    [MenuItem("Export/清理/Window")]
    public static void ClearWindow()
    {
        ClearAbs("Window");
    }
    [MenuItem("Export/Build/Android")]
    private static void AndroidToAB()
    {
        abInit();
        BuildABs("Android",BuildTarget.Android);
        UpdateABsSize("Android");
        ClearAbs("Android");
    }
  

    [MenuItem("Export/整包更新/Android")]
    private static void WindowToAb()
    {
        abInit();
        AndroidToAB();
    }
    #endregion
    /// <summary>
    /// 压缩导出所有ab文件
    /// </summary>
    /// <param name="platform"></param>
    static void compressAndExportAllAssetBundle(string platform)
    {
        //1.判断该目录下是否含有文件路径，没有就创建
        string platformPath = Application.dataPath.Replace("Assets", string.Format("Release/{0}/datas/", platform));
        if (!Directory.Exists(platformPath))
        {
            Directory.CreateDirectory(platformPath);
        }
        Directory.Delete(platformPath, true);
        Directory.CreateDirectory(platformPath);

        Dictionary<string, string[]> vDic = VersionUtil.getHashDic;
          
            //获取ab文件路径
            DirectoryInfo info = new DirectoryInfo(PathUtil.abs+"/"+platform);
            //1.路径下所有文件夹遍历获取，没有就创建platformab文件压缩路径
            foreach (DirectoryInfo directoryInfo in info.GetDirectories("*.*",SearchOption.AllDirectories))
            {
                //2.遍历文件夹下所有AB文件
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                 
                    //3获取AB文件
                    if (file.Extension != ".meta" && file.Extension != ".manifest")
                    {
                        string path = platformPath + file.Name;
                        byte[] data;
                        //path该路径是压缩后生成的AB资源路径地址
                        if (File.Exists(path))
                        {
                            //获取大小主要是为了更新我们的资源配置信息
                            data = File.ReadAllBytes(path);
                            Debug.Log(data.Length + "386-------");
                        }
                        else
                        {
                            //压缩文件 AB文件并生成到相应路径
                            data = ZlibStream.CompressBuffer(File.ReadAllBytes(file.FullName));
                            File.WriteAllBytes(path, data);
                            Debug.Log(data.Length + "393-------");
                        }
                        if (!vDic.ContainsKey(file.Name))
                        {
                            Debug.LogError("" + file.Name + "呦吼使这里把");
                        }
                        string[] item = vDic[file.Name];
                        item[2] = data.Length.ToString();
                        item[3] = "1";
                        vDic[file.Name] = item;
                    }
                }
            }
            VersionUtil.writeVersion(vDic);
            AssetDatabase.Refresh();
    }
      [MenuItem("Export/Test")]
    private static void test(){
        //BuildABs("ABs/Android",BuildTarget.Android);
        //compressAndExportAllAssetBundle("Android");
        toWindowAB();
        //UpdateABsSize("ABs/Android");
        //ClearAbs("ABs/Android");
    }

          /// <summary>
          /// 跟新资源名字
          /// 这里主要是用来处理资源名
          /// 当前生成的ab资源名字不是md5这里进行纠正
          /// 除了lua文件
          /// </summary>
          static void UpdateAssetBundleNames(string outPath)
          {
              Dictionary<string, string[]> vKeyDic = VersionUtil.getLowerVerDic;

              string path = PathUtil.abs + outPath;
              DirectoryInfo infos = new DirectoryInfo(path);
              path = path.Replace("/", "\\");
              string extension = ".manifest";
              foreach (FileInfo info in infos.GetFiles("*", SearchOption.AllDirectories))
              {
                  if (path != info.DirectoryName && string.IsNullOrEmpty(info.Extension))
                  {
                      string name = info.Name;
                      if (name.Contains("@") && name != null)
                      {
                          name = PathUtil.RestoreAbName(name);
                          if (vKeyDic.ContainsKey(name))
                          {
                              string abPath = info.DirectoryName + "\\" + vKeyDic[name][1];
                              if (!File.Exists(abPath))
                              {
                                  FileUtil.CopyFileOrDirectory(info.FullName, abPath);
                                  File.Delete(info.FullName);
                                  FileUtil.CopyFileOrDirectory(info.FullName + extension, abPath + extension);
                                  File.Delete(info.FullName + extension);
                              }
                              else
                              {
                                  File.Delete(info.FullName);
                                  File.Delete(info.FullName + extension);
                              }
                          }
                      }
                  }
              }
          }

    [MenuItem("Export/Clear")]
    private static void Clear()
    {

        BuildABs("ABs/Android",BuildTarget.Android);
        //UpdateABsSize("ABs/Android");
        ClearAbs("ABs/Android");
    }
    /// <summary>
    /// window版本专业AB包
    /// </summary>
    static void toWindowAB()
    {
        string platform = "Window";
        //1.判断该目录下是否含有文件路径，没有就创建
        string platformPath = Application.dataPath.Replace("Assets", string.Format("Release/{0}/datas/", platform));
        if (!Directory.Exists(platformPath))
        {
            Directory.CreateDirectory(platformPath);
        }
        Directory.Delete(platformPath, true);
        Directory.CreateDirectory(platformPath);

        Dictionary<string, string[]> vDic = VersionUtil.getHashDic;
        //获取ab文件路径
        DirectoryInfo info = new DirectoryInfo(PathUtil.abs + "/" + platform);
        foreach (DirectoryInfo directoryInfo in info.GetDirectories("*.*", SearchOption.AllDirectories))
        {
            //2.遍历文件夹下所有AB文件
            foreach (FileInfo file in directoryInfo.GetFiles())
            {

                //3获取AB文件
                if (file.Extension != ".meta" && file.Extension != ".manifest")
                {
                   File.Copy(file.FullName,platformPath+file.Name);
                }
            }
        }
        File.Copy(PathUtil.verPath,platformPath+"windowABs");
    }
    /// <summary>
    /// windows发布 用于游戏开发起见测试
    /// </summary>
    [MenuItem("Export/发布/window")]
    static void releaseWindow()
    {
        PlayerSettings.defaultScreenWidth = 1136;
        PlayerSettings.defaultScreenHeight = 640;
        PlayerSettings.defaultIsFullScreen = false;
        PlayerSettings.defaultIsNativeResolution = false;
        PlayerSettings.runInBackground = false;
        PlayerSettings.resizableWindow = false;
        PlayerSettings.visibleInBackground = false;
        PlayerSettings.usePlayerLog = false;
        Screen.fullScreen = false;
        PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;
        BuildPipeline.BuildPlayer(getLevels(), Application.dataPath.Replace("Assets", "Window/start.exe"),
            BuildTarget.StandaloneWindows, BuildOptions.ShowBuiltPlayer);
    }

    private static string[] getLevels()
    {
        List<string> levels = new List<string>();
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.enabled)
            {
                levels.Add(scene.path);
            }
        }
        return levels.ToArray();
    }

    public static void BuildABs(string outPath, BuildTarget target)
	{
		string path = PathUtil.abs + outPath;
        Debug.Log(path);
        if (!Directory.Exists(path)){
            Directory.CreateDirectory(path);
        }
        BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle;
		BuildPipeline.BuildAssetBundles("Assets/ABs/" + outPath, options, target);
		AssetDatabase.Refresh();
	    UpdateAssetBundleNames(outPath);
        UpdateABsSize(outPath);
	   

    }
    /// <summary>
    /// 更新AB文件大小--配置表
    /// </summary>
    /// <param name="outPath"></param>
    private static void UpdateABsSize(string outPath)
    {
        string path = PathUtil.abs + outPath;
        // 1 获取该路径下的所有的文件名 ---  文件的MD5
        // 2 获取配置表  md5 - 配置信息 值
        List<string> abNames = new List<string>();
        Dictionary<string, string[]> hashList = VersionUtil.getHashDic;
        Dictionary<string, string[]> keyList = VersionUtil.getLowerVerDic;
        DirectoryInfo infos = new DirectoryInfo(path);
        path = path.Replace("/", "\\");
        foreach (FileInfo info in infos.GetFiles("*", SearchOption.AllDirectories))
        {
            if (path != info.DirectoryName && string.IsNullOrEmpty(info.Extension))
            {
                abNames.Add(info.FullName);
            }
        }
        Dictionary<string, string[]> haseSize = new Dictionary<string, string[]>();
        foreach (string abPath in abNames)
        {
            FileInfo info = new FileInfo(abPath);
            string name = info.Name;
            if (name.Contains("@"))
            {
                Debug.Log(name);
                name = PathUtil.RestoreAbName(name);

                if (keyList.ContainsKey(name))
                {
                    string[] cfg = keyList[name];
                    cfg = hashList[cfg[1]];
                    cfg[2] = File.ReadAllBytes(abPath).Length + "";
                    cfg[3] = "0";
                    haseSize.Add(cfg[0], cfg);
                }
            }
            else
            {
                if (hashList.ContainsKey(name))
                {
                    string[] cfg = hashList[name];
                    cfg[2] = File.ReadAllBytes(abPath).Length + "";
                    cfg[3] = "0";
                    haseSize.Add(cfg[0], cfg);
                }
            }

        }
        VersionUtil.writeVersion(haseSize);
        AssetDatabase.Refresh();
    }

    static void ClearAbs(string outPath)
    {
        string path = Application.dataPath + "/" + outPath;
        AssetDatabaseUtil.Clear();
        if (!Directory.Exists(path))
        {
            Debug.Log("AB 文件 还未被创建！！");
            return;
        }
        Dictionary<string, string[]> keyDic = VersionUtil.getLowerVerDic;
        //获取真确的打包数据
        string[] abData = AssetDatabase.GetAllAssetBundleNames();
        Dictionary<string, bool> abDataList = new Dictionary<string, bool>();
        foreach (string name in abData)
        {
            //Debug.Log(name);
            string ab = name.Split('/')[1];
            if (ab.Contains("@"))
            {
                ab = PathUtil.RestoreAbName(ab);
                if (keyDic.ContainsKey(ab))
                {
                    ab = keyDic[ab][1];
                }
            }
            abDataList.Add(ab, true);
        }
        //更新files 文件
        VersionUtil.ClearVersion(abDataList);
        int count = 0;
        DirectoryInfo infos = new DirectoryInfo(path);
        path = path.Replace("/", "\\");
        foreach (FileInfo info in infos.GetFiles("*", SearchOption.AllDirectories))
        {
            if (path != info.DirectoryName && string.IsNullOrEmpty(info.Extension))
            {

                if (!abDataList.ContainsKey(info.Name))
                {
                    count++;
                    Debug.Log("删除废弃AB文件：" + info.FullName);
                    File.Delete(info.FullName + ".manifest");
                    File.Delete(info.FullName);
                    
                }
            }
        }
        Debug.Log("清理废弃AB文件:" + count);
        AssetDatabase.Refresh();
    }
    }
