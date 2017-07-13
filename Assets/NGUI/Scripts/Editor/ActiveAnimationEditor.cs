//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(ActiveAnimation))]
public class ActiveAnimationEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		NGUIEditorTools.SetLabelWidth(80f);
		ActiveAnimation aa = target as ActiveAnimation;
		GUILayout.Space(3f);
		NGUIEditorTools.DrawEvents("On Finished", aa, aa.onFinished);
	}
}
