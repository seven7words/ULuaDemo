--控制器基类
Lua_view_controller = newclass(base_behaviour)

function Lua_view_controller:GameInitData()
    self.LuaObjs = {}
end

function Lua_view_controller:GameControllerInit(className,gameObject,insID,cacheObjs,cachePrefabs)
    local class = _G[className]
    local Obj = class.new()
    Obj:init(gameObject,insID,cacheObjs,cachePrefabs)
    self.LuaObjs[insID] = Obj
end
function Lua_view_controller:UnityToLuaClick(insID,clickName)
    local luaObj = self.LuaObjs[insID]
    if luaObj ~= nil then
        print(luaObj)
        if clickName == UNITY_TO_LUA_CLICK.OnEnable then
            self:onEnable(luaObj)
        elseif clickName == UNITY_TO_LUS_CLICK.OnDisable then
            self:onDisable(luaObj)
        elseif clickName == UNITY_TO_LUS_CLICK.Awake then
            self:awake(luaObj)
        elseif clickName == UNITY_TO_LUS_CLICK.Start then
            self:start(luaObj)
        elseif clickName == UNITY_TO_LUS_CLICK.OnDestroy then
            self:onDestroy(luaObj)
        end
    else
        error("错误： "..insID.. "脚本名字： "..clickName)
    end
end
function Lua_view_controller:awake(luaObj)
    luaObj:awake()
end
function Lua_view_controller:onEnable(luaObj)
    luaObj:onEnable()
end
function Lua_view_controller:start(luaObj)
    luaObj:start()
end
function Lua_view_controller:onDisable(luaObj)
    luaObj:onDisable()
end
function Lua_view_controller:onDestroy(luaObj)
    luaObj:onDestroy()
end
function Lua_view_controller:GetLuaObjFormInsId(insID)
    local luaObj = self.LuaObjs[insID]
    if luaObj ~= nil then
        return luaObj
    end
    return nil
end
function Lua_view_controller:GetLuaObjFormObj(gameObject)
    for k,v in pairs(self.LuaObjs) do
        if v.gameObject == gameObject then
            return v
        end
    end
    return nil
end

