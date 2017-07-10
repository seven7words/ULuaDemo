--控制器类
local LOAD_LUA_SCRITE = {
    -----------基类加载---------
    "game/core/base_behaviour",
    "game/core/Lua_base_model",
    "game/core/Lua_view_controller",
    "game/core/Lua_behaviour",
    -----------UI界面相关脚本加载---------
    "game/core/View",
    -------------UI控制器类加载------------
    "game/modular/controller/main_controller",
    ---------资源管理类加载---------------
}

function ContainersInit()
   LoadLuas(LOAD_LUA_SCRITE)
end

--外部Lua脚本加载--
function LoadLuas(LuasList)
    if LuasList ~= nil then
        for k,v in pairs(LuasList) do
            require(v);
        end
    end
end
