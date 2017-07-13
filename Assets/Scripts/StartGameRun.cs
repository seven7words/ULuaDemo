using UnityEngine;
using System.Collections;

public class StartGameRun : MonoBehaviour 
{
   
    public TextAsset luaUpdate;
    public GameObject obj;
    LuaScriptMgr mgr;
	void Start() 
    {
        mgr = new LuaScriptMgr();
        if (luaUpdate != null)
        {
            mgr.DoStringFile(luaUpdate.name, luaUpdate.text);
            mgr.CallLuaFunction("Test",obj);
            mgr.Start();
            
        }
	}

}
