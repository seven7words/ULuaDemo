using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    void Awake()
    {
       GameObject obj = new GameObject("_Resource");
       ResourceManager mgr =  obj.AddComponent<ResourceManager>();
        mgr.init();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
