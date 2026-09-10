namespace KartPlusPlus.Engine {
    public class Component {
        public string Name;
        public EngineObject EngineObject;
        public Transform ObjTransform => EngineObject.Transform;

        public bool Enabled;

        /// <summary>
        /// Init is called when the component has been attached to an object
        /// </summary>
        public virtual void Init() {

        }
        /// <summary>
        /// Update is called every frame
        /// </summary>
        public virtual void Update() {

        }
        /// <summary>
        /// LateUpdate is called after Update
        /// </summary>
        public virtual void LateUpdate() {

        }
        /// <summary>
        /// PhysicsTick is called just before the physics simulation is timestepped
        /// </summary>
        public virtual void PhysicsTick() {

        }
        /// <summary>
        /// OnDestroy is called before the EngineObject is destroyed
        /// </summary>
        public virtual void OnDestroy() {

        }
        public virtual void OnComponentAdded(Component component) {

        }
        public virtual void OnComponentRemoved(Component component) {

        }
    }
}
