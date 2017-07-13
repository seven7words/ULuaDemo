using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
public class Export {
    #region csv转换成lua表
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
#endregion
    
    [MenuItem("Export/资源/图集更新")]
    static void UpdateAtlas()
    {
        DirectoryInfo info = new DirectoryInfo(PathUtil.atlasPath);
        foreach (FileInfo file in info.GetFiles("*.prefab"))
        {
            string atlas = PathUtil.GetAssetPath(file.FullName);
            string[] atlases = AssetDatabase.GetDependencies(new string[]{atlas});
            List<byte> bytes = new List<byte>();
            foreach (var path in atlases)
            {
                if (path.Contains(".cs"))
                {
                  bytes.AddRange(File.ReadAllBytes(PathUtil.GetUtterAssetPath(path))); 
                }
            }
            string md5 = MD5Util.GetMD5Hash(bytes.ToArray()) ;
            Debug.Log(md5);
            md5 = MD5Util.GetMD5Hash(File.ReadAllBytes(file.FullName));
            Debug.Log(md5);
            bool m = MD5Util.checkMD5Hash(File.ReadAllBytes(file.FullName), md5);
            Debug.Log(m);
        }
    }
}
