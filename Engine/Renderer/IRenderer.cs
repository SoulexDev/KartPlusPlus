namespace KartPlusPlus.Renderer {
    internal interface IRenderer {
        public void Draw(Camera camera);
        public void Draw(Camera camera, Shader shader);
    }
}
