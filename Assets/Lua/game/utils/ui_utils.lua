local m_UISpriteType   = UISprite.GetClassType()
local m_UITexture      = UITexture.GetClassType()
local m_UILabel        = UILabel.GetClassType()
local m_UIToggle       = UIToggle.GetClassType()
local m_UISlider       = UISlider.GetClassType()
local m_UIGrid         = UIGrid.GetClassType()
local m_UIInput        = UIInput.GetClassType()
local m_UIScrollView   = UIScrollView.GetClassType()
local m_UIButton       = UIButton.GetClassType()
local m_UITweener      = UITweener.GetClassType()
local m_UIAtlas        = UIAtlas.GetClassType()
local m_UIWidget       = UIWidget.GetClassType()
--local m_UITable          = UITable.GetClassType()


local m_NGUITools      = NGUITools
local m_Util           = Util
-------------获取ui组件
function ui_get_sprite(go)
    local comp = go:GetComponent(m_UISpriteType)
    return comp
end

function ui_get_texture(go)
    local comp = go:GetComponent(m_UITexture)
    return comp
end

function ui_get_label(go)
    local comp = go:GetComponent(m_UILabel)
    return comp
end

function ui_get_btn(go)

    local comp = go:GetComponent(m_UIButton)
    return comp
end

function ui_get_toggle(go)
    local comp = go:GetComponent(m_UIToggle)
    return comp
end

function ui_get_slider( go )
    local comp = go:GetComponent(m_UISlider)
    return comp
end

function ui_get_grid(go)
    local comp = go:GetComponent(m_UIGrid)
    return comp
end

function ui_get_input(go)
    local comp = go:GetComponent(m_UIInput)
    return comp
end

function ui_get_scroll_view(go)
    local comp = go:GetComponent(m_UIScrollView)
    return comp
end

function ui_get_tweener(go)
    local comp = go:GetComponent(m_UITweener)
    return comp
end

function ui_get_atlas(go)
    local comp = go:GetComponent(m_UIAtlas)
    return comp
end

function ui_get_transform(go)
    return go.transform
end

function ui_get_widget(go)
    local comp = go:GetComponent(m_UIWidget)
end

-- function ui_get_table(go)
--  local comp = go:GetComponent(m_UITable)
-- end
---向按钮注册单个事件
function ui_set_click(go, func, param)
    print("uiset")
    if param == nil then
        m_Util.setButtonClick(go, func)
    else
        local function callback()
            func(param)
        end
        m_Util.setButtonClick(go, callback)
    end
end

--添加按钮点击回调
function ui_add_click(go, func, param)
    if param == nil then
        m_Util.addButtonClick(go, func)
    else
        local function callback()
            func(param)
        end
        m_Util.addButtonClick(go, callback)
    end
end

--排序
function grid_reposition(parent)
    local grid = ui_get_grid(parent)
    if grid == nil then
        error("没有UIGrid脚本")
    end
    grid:Reposition()
    return grid
end

--添加子对象到父节点
function ui_add_child(child, parent)
    local rect = child.transform
    rect:SetParent(parent.transform, false)
    return rect
end

--创建对象并加到父节点
function ui_add_item(prefab, parent)
    local child = newGo(prefab)
    return ui_add_child(child, parent), child
end
