main_model = newclass(Lua_base_model)
--模块名字对应funId表
local model_name_cfg = {}
--funId对应模块数据表
local game_cfg = {}

local UP_ITEM_DATAS = {
    power = {itemId = 1,iconName = "label_3"},
    gold = {itemId = 2,iconName = "label_8"},
    money = {itemId = 3,iconName = "label_16"},
}

function main_model:ModelInit()
    local l_cfg = {}
    local cfg = get_game_cfg()
    for k,v in pairs(cfg) do
        model_name_cfg[v.name] = v.funId
        game_cfg[v.funId] = v
        if v.paterId ~= nil then
            l_cfg[v.paterId] = v
        end
    end
    for k,v in pairs(l_cfg) do
        if game_cfg[k] ~= nil then
            if game_cfg[k].childs == nil then
                game_cfg[k].childs = {}
            end
            game_cfg[k].childs[k] = v
        end
    end
    self.upItemDatas = UP_ITEM_DATAS
end

function main_model:test_model_init()

end
function main_model:getGameCfg()

    return game_cfg
end
