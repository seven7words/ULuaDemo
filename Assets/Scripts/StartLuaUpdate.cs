using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLuaUpdate : MonoBehaviour {
    public TextAsset StartluaText;
    public GameObject obj;
    LuaScriptMgr mgr;
	// Use this for initialization
	void Start () {
        mgr = new LuaScriptMgr();
			mgr.DoStringFile(StartluaText.name, StartluaText.text);
			mgr.CallLuaFunction("Test",gameObject);
			mgr.Start();
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
