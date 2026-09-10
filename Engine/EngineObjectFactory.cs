namespace KartPlusPlus.Engine {
    public class EngineObjectFactory {
        public static EngineObject Instantiate(string name) {
            EngineObject obj = new EngineObject(name);
            KartPlusPlus.EngineObjects.Add(obj);
            //SceneManager.activeScene.engineObjects.Add(obj);
            return obj;
        }
    }
}
