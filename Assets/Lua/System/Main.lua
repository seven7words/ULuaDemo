UpdateBeat = Event("Update", true)
LateUpdateBeat = Event("LateUpdate", true)
CoUpdateBeat = Event("CoUpdate", true)
FixedUpdateBeat = Event("FixedUpdate", true)


require "containers"

function Main()
	Time:Init()
	ContainersInit()
end

function Update(deltatime, unscaledDeltaTime)
	Time:SetDeltaTime(deltatime, unscaledDeltaTime)
	UpdateBeat()
end

function LateUpdate()
	LateUpdateBeat()
	CoUpdateBeat()
end

function FixedUpdate(fixedTime)
	Time:SetFixedDelta(fixedTime)
	FixedUpdateBeat()
end

function OnLevelWasLoaded(level)
	Time.timeSinceLevelLoad = 0
end
