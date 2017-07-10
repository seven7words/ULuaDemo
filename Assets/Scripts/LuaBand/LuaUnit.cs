using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaUnit : MonoBehaviour {
    public string luaClassName = "";
    public GameObject[] objs;
    public GameObject[] prefabs;
    private int insID;
    private LuaScriptMgr toLua;

    void Awake(){
        toLua = LuaScriptMgr.Instance;
        insID = gameObject.GetInstanceID();
        if (string.IsNullOrEmpty(luaClassName)){
            Debug.Log(gameObject.name+"-----Lua脚本名字为空----->>", transform);
            return;
        }
        int len = objs == null ? 0 : objs.Length;
        int prefabLen = prefabs == null ? 0 : prefabs.Length;
        Debug.Log("gameObject.name");
        CallMethod("awake", gameObject, luaClassName, insID, objs, len, prefabs,prefabLen);
    }
	// Use this for initialization
	void Start () {
		
	}
	
    void OnDestroy(){
        
    }
    object[] CallMethod(string funcName,params object[] objs){
        if(toLua == null){
            return null;
        }
        return toLua.CallLuaFunction(funcName, objs);
    }
}
