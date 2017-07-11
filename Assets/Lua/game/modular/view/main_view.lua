main_view = newclass(base_behaviour)
local cacheObjs = {}
local cachePrefabs = {}
local parent_table = {}
local modelLuaObj
local m_find_luaObject_by_insID = find_luaObject_by_insID
local m_get_instance_id = get_instance_id
local m_ui_get_sprite = ui_get_sprite
local m_tween_xz
local m_tween_sf
local m_tween_kg = true
local function open_view_click(vo)
    print(vo.name.."------")
end
local function anim_xz()
    if m_tween_kg == true then
        m_tween_xz:PlayForward()
        m_tween_sf:PlayForward()
        m_tween_kg = false
    else
        m_tween_xz:PlayReverse()
        m_tween_sf:PlayReverse()
        m_tween_kg = true
    end
end
-- function buttonTest()
--     print("按钮相应")
-- end
-- local function buttonsr(str)
--     print("----->"..str)
-- end
function init()
    print("init")
    print(cacheObjs.up_data.name)
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
        ui_set_click(obj,open_view_click,v)
        table.insert( childs, tran )
    end
    local upItemDatas = modelLuaObj.upItemDatas
    for k,v in pairs(upItemDatas) do
        local tran,obj = ui_add_item(cachePrefabs.up_item,parent_table[6])
        obj.name = v.itemId
        m_find_luaObject_by_insID(m_get_instance_id(obj)):initItem(v)
        table.insert(childs,tran)
    end
    for k,v in pairs(childs) do
        v:SetSiblingIndex(v.name-1)
    end
    for k,v in pairs(parent_table) do
        grid_reposition(v)
    end
    childs = nil
    m_tween_xz = ui_get_tweener(cacheObjs.objRelease)
    m_tween_sf = ui_get_tweener(cacheObjs.right_down)
    ui_set_click(cacheObjs.objRelease,anim_xz)
end
function main_view:start()

    cacheObjs = self.cacheObjs
    cachePrefabs = self.cachePrefabs
    modelLuaObj = self.modelLuaObj
    init()
end
