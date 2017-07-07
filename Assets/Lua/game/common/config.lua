--配置表管理器

--缓存的配置列表
local config_list = {}

local function configTestCfg(cfg)
    local cfgs = {}
    for k,v in pairs(cfg) do
        cfgs[v.index] = v
    end
    return cfgs
end
local function configLocalization(cfg)
    local cfgs = {}
    for k,v in pairs(cfg) do
        cfgs[v.key] = v.value
    end
    return cfgs
end
--表配置映射
local config_map = {
    TEST = {url = Res.CSV_TEST,func = configTestCfg},
    LOCALIZATION = {url = Res.CSV_LOCALIZATION,func = configLocalization},
}
local function config_jx(tableName)
    local map = config_map[tableName]
    local url = map.url
    local func = map.func
    require("game/configs/"..map.url.."_csv")
    local cfg = CSV_TABLES[map.url]
    if func ~= nil then
        cfg = func(cfg,tableName)
        CSV_TABLES[map.url] = nil
    end
    config_list[tableName] = cfg
    return cfg
end
function config_get_cfg(tableName)
    local cfg = config_list[tableName]
    if cfg ~= nil then
        return cfg
    end
    return config_jx(tableName)
end
function get_cfg_test(id)
   return config_get_cfg("TEST")[id]
end
function get_cfg_localization()
    return config_get_cfg("LOCALIZATION")
end
