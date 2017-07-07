main_view = newclass(base_behaviour)
local cacheObjs = {}
function buttonTest()
    print("按钮相应")
end
local function buttonsr(str)
    print("----->"..str)
end
function init()
    print("init")
    ui_add_click(cacheObjs.BtnMail,buttonTest)
    ui_add_click(cacheObjs.BtnMail,buttonsr,"宝宝撒拉嘿")
end
function main_view:awake()
    cacheObjs = self.cacheObjs
    print("awake......main -view")
    init()
end
