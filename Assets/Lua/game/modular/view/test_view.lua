test_view = newclass(base_behaviour)
function  test_view:awake()
    print(self.cacheObjs.Main_Camera.name.."------awake------"
        ..get_localization_by_key("test"))

end
function  test_view:start()
    print(self.cacheObjs.Main_Camera.name.."------start------"
        ..self.gameObject.name)
end
