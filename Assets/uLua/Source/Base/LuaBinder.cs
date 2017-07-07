﻿using System;
using System.Collections.Generic;

public static class LuaBinder
{
	public static void Bind(IntPtr L, string type = null)
	{
		AnimationBlendModeWrap.Register(L);
		AnimationClipWrap.Register(L);
		AnimationStateWrap.Register(L);
		AnimationWrap.Register(L);
		AppConstWrap.Register(L);
		ApplicationWrap.Register(L);
		AssetBundleWrap.Register(L);
		AsyncOperationWrap.Register(L);
		AudioClipWrap.Register(L);
		AudioSourceWrap.Register(L);
		BehaviourWrap.Register(L);
		BlendWeightsWrap.Register(L);
		BoxColliderWrap.Register(L);
		CameraClearFlagsWrap.Register(L);
		CameraWrap.Register(L);
		CharacterControllerWrap.Register(L);
		ColliderWrap.Register(L);
		ComponentWrap.Register(L);
		DebuggerWrap.Register(L);
		DelegateFactoryWrap.Register(L);
		DelegateWrap.Register(L);
		EnumWrap.Register(L);
		GameObjectWrap.Register(L);
		IEnumeratorWrap.Register(L);
		InputWrap.Register(L);
		KeyCodeWrap.Register(L);
		LightTypeWrap.Register(L);
		LightWrap.Register(L);
		LocalizationWrap.Register(L);
		MaterialWrap.Register(L);
		MeshColliderWrap.Register(L);
		MeshRendererWrap.Register(L);
		MonoBehaviourWrap.Register(L);
		NGUIToolsWrap.Register(L);
		ObjectWrap.Register(L);
		ParticleAnimatorWrap.Register(L);
		ParticleEmitterWrap.Register(L);
		ParticleRendererWrap.Register(L);
		ParticleSystemWrap.Register(L);
		PhysicsWrap.Register(L);
		PlayModeWrap.Register(L);
		QualitySettingsWrap.Register(L);
		QueueModeWrap.Register(L);
		RenderSettingsWrap.Register(L);
		RenderTextureWrap.Register(L);
		RendererWrap.Register(L);
		ScreenWrap.Register(L);
		SkinnedMeshRendererWrap.Register(L);
		SleepTimeoutWrap.Register(L);
		SpaceWrap.Register(L);
		SphereColliderWrap.Register(L);
		System_ObjectWrap.Register(L);
		TextureWrap.Register(L);
		TimeWrap.Register(L);
		TouchPhaseWrap.Register(L);
		TrackedReferenceWrap.Register(L);
		TransformWrap.Register(L);
		TweenPositionWrap.Register(L);
		TweenRotationWrap.Register(L);
		TweenScaleWrap.Register(L);
		TweenWidthWrap.Register(L);
		TypeWrap.Register(L);
		UIAtlasWrap.Register(L);
		UIBasicSpriteWrap.Register(L);
		UIButtonWrap.Register(L);
		UICameraWrap.Register(L);
		UICenterOnChildWrap.Register(L);
		UIGridWrap.Register(L);
		UIInputWrap.Register(L);
		UILabelWrap.Register(L);
		UIProgressBarWrap.Register(L);
		UIRectWrap.Register(L);
		UIScrollViewWrap.Register(L);
		UISliderWrap.Register(L);
		UISpriteWrap.Register(L);
		UITextureWrap.Register(L);
		UIToggleWrap.Register(L);
		UITweenerWrap.Register(L);
		UIWidgetContainerWrap.Register(L);
		UIWidgetWrap.Register(L);
		UtilWrap.Register(L);
		stringWrap.Register(L);
	}
}
