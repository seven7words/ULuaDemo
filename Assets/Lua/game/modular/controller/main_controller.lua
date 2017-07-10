main_controller = newclass(Lua_view_controller)
require "game/modular/model/main_model"
require "game/modular/view/main_view"
require "game/modular/units/main/main_up_item"
function main_controller:awake(luaObj)
    luaObj:awake()
    self.modelLuaObj:getGameCfg()
end
function main_controller:onEnable()
    -- body
end
function main_controller:start(luaObj)
    luaObj:start()
end
function main_controller:onDisable()
    -- body
end
