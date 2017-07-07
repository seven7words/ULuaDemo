--本地化键值数据表
local localizations ={}
local function localizationsInit()
   localizations = get_cfg_localization()
end
localizationsInit()
function get_localization_by_key(key)
    local value =localizations[key]
    if value ~= nil and value ~= "" then
        return value
    else
        error("警告"..key.."该key在localization里没有")
    end
end
