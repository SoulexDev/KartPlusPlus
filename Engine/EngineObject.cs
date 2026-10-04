namespace KartPlusPlus.Engine {
    public class EngineObject {
        public string Name = "Null Name";

        public Transform Transform;
        internal List<Component> components = new List<Component>();

        internal event Action<Component> componentAdded;
        internal event Action<Component> componentRemoved;
        internal event Action<int> physicsLayerChanged;

        private int physicsLayer = 0;
        public int PhysicsLayer {
            get { return physicsLayer; }
            set {
                if (physicsLayer != value) {
                    physicsLayer = value;

                    physicsLayerChanged?.Invoke(physicsLayer);
                }
            }
        }
        public int LightLayer;

        internal EngineObject() {
            Name = "Engine Object";
            Transform = new Transform(this);
        }
        internal EngineObject(string name) {
            Name = name;
            Transform = new Transform(this);
        }
        public T AddComponent<T>(string name = "") where T : Component {
            if (components.Exists(c => c.Name == name)) {
                Console.Write($"Components with name {name} already exists.");
                return null;
            }
            T component = (T)Activator.CreateInstance(typeof(T));

            component.Name = name == string.Empty ? nameof(component) : name;
            component.EngineObject = this;

            components.Add(component);

            component.Init();

            componentAdded?.Invoke(component);

            componentAdded += component.OnComponentAdded;
            componentRemoved += component.OnComponentRemoved;

            return component;
        }
        public T GetComponent<T>(string name) where T : Component {
            Component component = components.Find(c => c.Name == name);
            if (component != null)
                return component as T;
            else
                return null;
        }
        public List<Component> GetComponentsOfType<T>() where T : Component {
            return components.FindAll(c => c.GetType().BaseType == typeof(T));
        }
        public bool HasComponentOfType<T>() where T : Component {
            return components.Exists(c => c.GetType() == typeof(T));
        }
        public void RemoveComponent<T>(string name) where T : Component {
            Component c = components.Find(c => c.Name == name && c.GetType() == typeof(T));
            if (c != null) {
                components.Remove(c);
                componentAdded -= c.OnComponentAdded;
                componentRemoved -= c.OnComponentRemoved;
            }
        }
        public void Destroy() {
            components.ForEach(c => c.OnDestroy());
            KartPlusPlus.EngineObjects.Remove(this);
        }
    }
}
