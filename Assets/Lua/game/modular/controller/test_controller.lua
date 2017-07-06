test_controller = newclass(Lua_view_controller)
local test_model = require "game/modular/model/test_model"
require "game/modular/view/test_view"

function test_controller:awake()
    print("awake----------------"..test_model)
end
function test_controller:onEnable()
    -- body
end
function test_controller:start()
    print("start-------")
end
function test_controller:onDisable()
    -- body
end
