local m_UISpriteType = UISprite.GetClassType()
local m_UITexture = UITexture.GetClassType()


function ui_get_sprite(go)
    local comp = go:GetComponent(m_UISpriteType)
    return comp
end
function ui_get_texture(go)
    local comp = go:GetComponent(m_UITexture)
    return comp
end
