test_view = newclass(base_behaviour)
function  test_view:awake()
    print(self.cacheObjs.Main_Camera.name.."------------"
        ..self.gameObject.name)
end
