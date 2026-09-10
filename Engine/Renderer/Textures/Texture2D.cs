using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System.Runtime.CompilerServices;

namespace KartPlusPlus.Renderer {
    public class Texture2D {
        public uint ID;
        public int Width;
        public int Height;

        private PixelInternalFormat internalPixelFormat;
        private PixelFormat pixelFormat;
        private PixelType pixelType;

        public static Texture2D Create(string filePath) {
            return new Texture2D(filePath);
        }
        public Texture2D(uint id) {
            ID = id;
        }
        public Texture2D(
            string filePath,
            TextureWrapMode textureWrapMode = TextureWrapMode.Repeat,
            TextureMinFilter minFilter = TextureMinFilter.Linear,
            TextureMagFilter magFilter = TextureMagFilter.Linear,
            bool generateMipMap = true) {
            GL.GenTextures(1, out ID);
            GL.BindTexture(TextureTarget.Texture2D, ID);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            AssignData(filePath);

            if (generateMipMap)
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public Texture2D(
            int width,
            int height,
            PixelInternalFormat internalPixelFormat = PixelInternalFormat.Rgba,
            PixelFormat pixelFormat = PixelFormat.Rgba,
            PixelType pixelType = PixelType.UnsignedByte,
            TextureWrapMode textureWrapMode = TextureWrapMode.Repeat,
            TextureMinFilter minFilter = TextureMinFilter.Linear,
            TextureMagFilter magFilter = TextureMagFilter.Linear) {
            this.internalPixelFormat = internalPixelFormat;
            this.pixelFormat = pixelFormat;
            this.pixelType = pixelType;

            GL.GenTextures(1, out ID);
            GL.BindTexture(TextureTarget.Texture2D, ID);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalPixelFormat, width, height, 0, pixelFormat, pixelType, 0);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        internal void Resize(int width, int height) {
            Width = width;
            Height = height;

            GL.BindTexture(TextureTarget.Texture2D, ID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, this.Width, this.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AssignData(string filePath) {
            StbImage.stbi_set_flip_vertically_on_load(1);

            using (Stream stream = File.OpenRead(filePath)) {
                ImageInfo? info = ImageInfo.FromStream(stream);

                if (info == null) {
                    Console.WriteLine("Could not load image because image info could not be obtained.");
                    return;
                }
                ImageResult image = ImageResult.FromStream(stream, info.Value.ColorComponents);
                if (image.SourceComp == ColorComponents.RedGreenBlue) {
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0,
                        PixelFormat.Rgb, PixelType.UnsignedByte, image.Data);
                }
                else if (image.SourceComp == ColorComponents.RedGreenBlueAlpha) {
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
                        PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                }

                Width = image.Width;
                Height = image.Height;
            }
        }
        public void Bind(TextureUnit unit = TextureUnit.Texture0) {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, ID);
        }
        public void Unbind(TextureUnit unit = TextureUnit.Texture0) {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        //operators
        public static implicit operator uint(Texture2D texture) => texture.ID;
    }
}
