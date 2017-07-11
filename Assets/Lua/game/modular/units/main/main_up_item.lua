main_up_item = newclass(base_behaviour)
local cacheObjs ={}
local m_ui_get_sprite = ui_get_sprite
local asss = 1
local function click(itemId)
    print(itemId)
    asss = asss + 1
    print(asss)
end
function main_up_item:initItem(tableData)
    ui_get_sprite(cacheObjs.sprIcon).spriteName = tableData.iconName
    if tableData.itemId ~= 2 then

        ui_set_click(cacheObjs.objAdd,click,tableData.itemId)
    else
        setActive(cacheObjs.objAdd,false)
    end
end
function main_up_item:awake()
    cacheObjs = self.cacheObjs
end
