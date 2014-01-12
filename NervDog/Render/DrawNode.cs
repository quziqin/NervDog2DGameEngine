using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NervDog.Render
{
    public class DrawNode
    {
        #region Fields

        private static readonly IComparer<DrawNode> _comparer = new DrawOrderComparer();
        protected List<DrawNode> _children = null;
        private float _drawOrder;
        private bool _isChildrenOrderChange;

        protected bool _isVisible = true;

        protected Matrix _mWorld = Matrix.Identity;
        protected DrawNode _parent = null;

        #endregion

        #region Properties

        public float DrawOrder
        {
            set
            {
                _drawOrder = value;
                if (_parent != null)
                {
                    _parent.NotifyChildrenOrderChange();
                }
            }
            get { return _drawOrder; }
        }

        public bool IsVisible
        {
            set { _isVisible = value; }
            get { return _isVisible; }
        }

        public DrawNode Parent
        {
            get { return _parent; }
        }

        public Matrix World
        {
            get { return _mWorld; }
        }

        #endregion

        #region Constructors

        #endregion

        #region Private Functions

        private void NotifyChildrenOrderChange()
        {
            _isChildrenOrderChange = true;
        }

        #endregion

        #region Public Functions

        public void Add(DrawNode draw)
        {
            if (draw._parent != null)
            {
                draw._parent.Remove(draw);
            }
            draw._parent = this;

            if (_children == null)
            {
                _children = new List<DrawNode>();
            }
            _children.Add(draw);

            _isChildrenOrderChange = true;
        }

        public void Remove(DrawNode draw)
        {
            if (_children != null)
            {
                if (_children.Remove(draw))
                {
                    draw._parent = null;
                }
            }
        }

        public void Clear()
        {
            if (_children != null)
            {
                int count = _children.Count;
                for (int i = 0; i < count; i++)
                {
                    _children[i]._parent = null;
                }
                _children.Clear();
            }
        }

        public virtual void Draw()
        {
            DrawSelf();
            DrawChildren();
        }

        public virtual void DrawChildren()
        {
            if (_children != null)
            {
                if (_isChildrenOrderChange)
                {
                    _isChildrenOrderChange = false;
                    _children.Sort(_comparer);
                }

                int count = _children.Count;
                for (int i = 0; i < count; i++)
                {
                    DrawNode d = _children[i];
                    if (d._isVisible)
                    {
                        d.Draw();
                    }
                }
            }
        }

        public virtual void DrawSelf()
        {
            //Define the concrete draw logic in derived class
        }

        #endregion
    }

    public class DrawOrderComparer : IComparer<DrawNode>
    {
        public int Compare(DrawNode a, DrawNode b)
        {
            return (a.DrawOrder < b.DrawOrder ? -1 : 1);
        }
    }
}