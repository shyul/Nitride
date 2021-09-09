/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

namespace Nitride.Chart
{
    public interface IIndexArea : IOrdered, ICoordinatable
    {
        ChartWidget Chart { get; }

        ColorTheme Theme { get; }

        int StopPt { get; }

        int StartPt { get; }

        int IndexToPixel(int index);

        IndexAxis AxisX { get; }

        ContinuousAxis AxisY(AlignType side);

        int RightCursorX { get; }

        int LeftCursorX { get; }
    }
}
