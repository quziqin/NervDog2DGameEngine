using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NervDog.Render
{
    public interface IEffect : IEffectMatrices
    {
        string Name { set; get; }

        bool ZWriteEnable { set; get; }

        Vector3 Color { set; get; }

        Texture2D Texture { set; get; }

        TextureAddressMode TextureMode { set; get; }

        float Alpha { set; get; }

        void Apply();
    }
}