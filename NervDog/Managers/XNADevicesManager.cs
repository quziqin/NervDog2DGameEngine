using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NervDog.Managers
{
    public class XNADevicesManager
    {
        #region Fields

        private static readonly XNADevicesManager _instance = new XNADevicesManager();

        #endregion

        #region Properties

        public static XNADevicesManager Instance
        {
            get { return _instance; }
        }

        public GraphicsDevice GraphicsDevice { get; set; }

        public ContentManager ContentManager { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        #endregion

        #region Constructors

        private XNADevicesManager()
        {
            SpriteBatch = null;
            ContentManager = null;
            GraphicsDevice = null;
        }

        #endregion
    }
}