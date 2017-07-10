main_controller = newclass(Lua_view_controller)
require "game/modular/model/main_model"
require "game/modular/view/main_view"

function main_controller:awake()
    print("awake----------------")
end
function main_controller:onEnable()
    -- body
end
function main_controller:start()
    print("start-------")
end
function main_controller:onDisable()
    -- body
end
