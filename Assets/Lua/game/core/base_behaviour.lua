--界面基类
base_behaviour = newclass()

function base_behaviour:init(gameObject, insID, cacheObjs, cachePrefabs)
    self.gameObject = gameObject
    self.transform = gameObject.transform
    self.name = gameObject.name
    self.insID = insID
    self.cacheObjs = cacheObjs
    self.cachePrefabs = cachePrefabs
end
function base_behaviour:initModel(modelLuaObj)
    self.modelLuaObj = modelLuaObj
end
function base_behaviour:awake()
end

function base_behaviour:onEnable()
end

function base_behaviour:start()
end

function base_behaviour:onDisable()
end

function base_behaviour:getInsID()
    return self.insID
end

function base_behaviour:getName()
    return self.name
end

function base_behaviour:OnDestroy()

end
