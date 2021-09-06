/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Dyson Project - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Drawing;

namespace Nitride.Chart
{
    /// <summary>
    /// Advance and decline series
    /// </summary>
    public interface IAdvanceDeclineSeries : IOrdered
    {
        /// <summary>
        /// Gain data column
        /// </summary>
        public NumericColumn Gain_Column { get; }

        Color LowerColor { get; }

        /// <summary>
        /// Theme for down trend
        /// </summary>
        ColorTheme LowerTheme { get; }

        /// <summary>
        /// Theme for down trend text
        /// </summary>
        ColorTheme LowerTextTheme { get; }
    }
}
