/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Dyson Project - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;

namespace Nitride
{
    public interface ICoordinatable
    {
        Rectangle Bounds { get; set; }

        Point Location { get; set; }

        Size Size { get; set; }

        int Top { get; }

        int Bottom { get; }

        int Left { get; }

        int Right { get; }

        Point Center { get; }
    }
}
