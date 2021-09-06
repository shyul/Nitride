/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Dyson Project - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************


namespace Nitride.Chart
{
    public interface IChartAnalysis
    {
        string AreaName { get; }

        bool ChartEnabled { get; set; }

        int DrawOrder { get; set; }
    }
}
