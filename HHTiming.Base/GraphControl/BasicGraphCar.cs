using HHDev.Core.NETStandard.Graphing;
using HHTiming.Core;
using HHTiming.WinFormsControls.ElementConfiguration;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHTiming.Base.Graph
{
    public class BasicGraphCar : BaseUICar
    {

        public HHOxyLineSeries Series { get; set; }
        public BasicGraphCar(string aCarID, Color aCarColour, string aCategoryID, string aTyreManufacturerID) : base(aCarID, aCarColour, aCategoryID, aTyreManufacturerID)
        {
            Series = new HHOxyLineSeries
            {
                Color = OxyPlotHelpers.GetOxyColor(CarColor),
                StrokeThickness = 2,
                Title = CarID,
                BrokenLineStyle = LineStyle.None
            };
        }

        protected override void OnCarColourChanged()
        {
            base.OnCarColourChanged();

            Series.Color = OxyPlotHelpers.GetOxyColor(CarColor);
        }

        public void UpdateLineThickness(bool isReference)
        {
            if (isReference)
            {
                Series.StrokeThickness = 4;
            }
            else
            {
                Series.StrokeThickness = 2;
            }
        }
    }
}
