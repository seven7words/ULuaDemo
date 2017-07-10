test_controller = newclass(Lua_view_controller)
require "game/modular/model/test_model"
require "game/modular/view/test_view"

function test_controller:awake()

    self.modelLuaObj:test_model_init()
end
function test_controller:onEnable()
    -- body
end
function test_controller:start()

end
function test_controller:onDisable()
    -- body
end
