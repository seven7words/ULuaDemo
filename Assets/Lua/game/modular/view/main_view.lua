main_view = newclass(base_behaviour)
local cacheObjs = {}
local cachePrefabs = {}
local parent_table = {}
local modelLuaObj
-- function buttonTest()
--     print("按钮相应")
-- end
-- local function buttonsr(str)
--     print("----->"..str)
-- end
function init()
    print("init")
    print(cacheObjs.right_down.name)
    parent_table[1] = cacheObjs.right_down
    parent_table[2] = cacheObjs.left_down
    parent_table[3] = cacheObjs.left_up
    parent_table[4] = cacheObjs.up
    parent_table[5] = cacheObjs.right_up
    parent_table[6] = cacheObjs.up_data
    --ui_add_click(cacheObjs.BtnMail,buttonTest)
    --ui_add_click(cacheObjs.BtnMail,buttonsr,"宝宝撒拉嘿")
    local game_cfg = modelLuaObj:getGameCfg()
    local parent

    local prefab = cachePrefabs.btn_up
    local childs = {}
    for k,v in pairs(game_cfg) do
        parent = parent_table[v.type]

        local tran,obj = ui_add_item(prefab,parent)
        ui_get_sprite(obj).spriteName = v.icon
        obj.name = v["local"]
        table.insert( childs, tran )
    end
    for k,v in pairs(childs) do
        v:SetSiblingIndex(v.name-1)
    end
    for k,v in pairs(parent_table) do
        grid_reposition(v)
    end

end
function main_view:start()

    cacheObjs = self.cacheObjs
    cachePrefabs = self.cachePrefabs
    modelLuaObj = self.modelLuaObj
    init()
end
