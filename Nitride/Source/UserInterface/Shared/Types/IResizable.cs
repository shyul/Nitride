/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Dyson Project - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace Nitride
{
    public interface IResizable
    {
        LayoutType LayoutType { get; }

        Rectangle SizeGrip { get; }

        Control Area { get; }

        Control Canvas { get; }

        void OnGetSize(int size);
    }
}
