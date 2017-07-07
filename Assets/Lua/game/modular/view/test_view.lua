test_view = newclass(base_behaviour)
function  test_view:awake()
    print("------awake------"
        ..get_localization_by_key("test"))
    print(self.cacheObjs.Sprite.name)
    ui_get_sprite(self.cacheObjs.Sprite).spriteName = "main_1"

end
function  test_view:start()
    print("------start------"
        ..self.gameObject.name)
end
