--接受unity脚本事件回调



local LuaObj = {} --非控制器脚本对象表
local luaController = {} --控制器类
local luaModels = {} --数据类对象
--界面view类对应控制器的注册
local VIEW_CONTROL_ENROLL = {
    test_view = "test_controller",
}
--数据类
local MODEL_CONTROL_ENROLL = {
    test_controller = "test_model",
}
--Unity初始化以及回调方法注册
UNITY_TO_LUA_CLICK = {
    Awake = "awake",
    onEnable = "onEnable",
    Start = "start",
    OnDisable = "onDisable",
    OnDestroy = "onDestroy",
}

function awake(gameObject, className, insID, objs, len, prefabs,prefabLen)
    ----------界面对象以及预设添加-----------
    local cacheObjs = {}
    for i=0,len-1 do
       local obj = objs[i]
       if obj == nil then
            error(gameObject.name .. "对象脚本为空"..i)
       end
       cacheObjs[obj.name] = obj
    end

    local cachePrefabs = {}
    for i=0,prefabLen-1 do
       local obj = objs[i]
       if obj == nil then
            error(gameObject.name .. "对象脚本为空"..i)
       end
       cachePrefabs[obj.name] = obj
    end

    local m_cont = VIEW_CONTROL_ENROLL[className]

    if m_cont == nil then
        local class = _G[className]
        local Obj = class.new()
        Obj:init(gameObject,insID,cacheObjs,cachePrefabs)
        LuaObj[insID] = Obj
        Obj:awake()
    else
        local Obj = luaController[m_cont]
        if Obj == nil then
            local modelName = MODEL_CONTROL_ENROLL[m_cont]
            local class = _G[modelName]
            local modelLuaObj = class.new()
            modelLuaObj:ModelInit()
            luaModels[modelName] = modelLuaObj
            class = _G[m_cont]
            Obj = class.new()
            luaController[m_cont] = Obj
            Obj:GameInitData(modelLuaObj)
        end
        Obj:GameControllerInit(className,gameObject,insID,cacheObjs,cachePrefabs)
        Obj:UnityToLuaClick(insID,UNITY_TO_LUA_CLICK.Awake)
    end

   local class = _G[className]
   local luaObj = class.new()
   luaObj:init(gameObject,insID,cacheObjs,cachePrefabs)
   LuaObj[insID] = luaObj
   luaObj:awake()
end
function onEnable(insID,className)

    local luaObj = luaObj[insID]
    if luaObj ~= nil then
        luaObj:onEnable()
    else
        local name = VIEW_CONTROL_ENROLL[className]
        if name ~= nil and luaController[name] ~= nil then
            luaController[name]:UnityToLuaClick(insID,UNITY_TO_LUA_CLICK.OnEnable)
        else
            error(insID.."onEnable View"..className.."--> 没有查找到该脚本对象")
        end
    end

end
function start(insID,className)
    local luaObj = luaObj[insID]
    if luaObj ~= nil then
        luaObj:start()
    else
        local name = VIEW_CONTROL_ENROLL[className]
        if name ~= nil and luaController[name] ~= nil then
            luaController[name]:UnityToLuaClick(insID,UNITY_TO_LUA_CLICK.Start)
        else
            error(insID.."Start View"..className.."--> 没有查找到该脚本对象")
        end
    end
end
function onDisable(insID,className)
    local luaObj = luaObj[insID]
    if luaObj ~= nil then
        luaObj:onDisable()
    else
        local name = VIEW_CONTROL_ENROLL[className]
        if name ~= nil and luaController[name] ~= nil then
            luaController[name]:UnityToLuaClick(insID,UNITY_TO_LUA_CLICK.OnDisable)
        else
            error(insID.."OnDisable View"..className.."--> 没有查找到该脚本对象")
        end
    end
end
function onDestroy(insID,className)
    local luaObj = luaObj[insID]
    if luaObj ~= nil then
        luaObj.isDestroy = true
        luaObj:OnDestroy()
    else
        local name = VIEW_CONTROL_ENROLL[className]
        if name ~= nil and luaController[name] ~= nil then
            luaController[name]:UnityToLuaClick(insID,UNITY_TO_LUA_CLICK.OnDestroy)
        else
            error(insID.."OnDestroy View"..className.."--> 没有查找到该脚本对象")
        end
    end
    LuaObj[insID] = nil
end
--通过游戏对象获取游戏脚本
function find_luaObject_by_object(gameObject)
    for k,v in pairs(LuaObj) do
        if v ~= nil and not v.isDestroy and v.gameObject == gameObject then
            return v
        end
    end
    local luaObj
    for k,v in pairs(luaController) do
        luaObj = v:GetLuaObjFormObj(gameObject)
        if luaObj ~= nil then
            return luaObj
        end
    end
    return nil
end
--通过唯一游戏id获取游戏脚本
function find_luaObject_by_insID(insID)
    local luaObj = LuaObj[insID]
    if luaObj ~= nil then
        return luaObj
    end
    for k,v in pairs(luaController) do
        luaObj = v:GetLuaObjFormInsId(insID)
        if luaObj ~= nil then
            return luaObj
        end
    end
    return nil
end
