test_model = newclass(Lua_base_model)
function test_model:ModelInit()
    print("ModelInit---->数据初始化"..get_cfg_test(3).name)

end

function test_model:test_model_init()
    print("test-----------")
end

