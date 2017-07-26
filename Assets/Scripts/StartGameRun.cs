using UnityEngine;
using System.Collections;

public class StartGameRun : MonoBehaviour 
{
   
    public TextAsset luaUpdate;
    public GameObject obj;
    LuaScriptMgr mgr;
	void Start()
	{
	    mgr = LuaScriptMgr.Instance;
        if (luaUpdate != null)
        {
            mgr.DoStringFile(luaUpdate.name, luaUpdate.text);
            mgr.CallLuaFunction("Test",obj);
            mgr.Start();
        }
	}

}
