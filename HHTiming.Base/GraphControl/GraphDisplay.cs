using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HHTiming.WinFormsControls.Filtering;
using HHTiming.WinFormsControls.Scoreboards;
using HHTiming.Core.Definitions.UIUpdate.Interfaces;
using HHTiming.Desktop.Definitions.Worksheet;
using HHTiming.Core.Definitions.UIUpdate.Database;
using HHDev.ProjectFramework.Definitions;
using System.Xml;
using HHTiming.Desktop.Definitions.PlugInFramework;
using HHTiming.Core.Definitions.UIUpdate.Implementations.Messages;
using HHTiming.Core.Definitions.UIUpdate.Implementations;
using HHTiming.Core.Definitions.Enums;
using HHTiming.WinFormsControls.Graphing;
using HHTiming.WinFormsControls.ElementConfiguration;
using HHDev.Core.NETStandard.Helpers;
using HHDev.Core.NETStandard.Extensions;
using DevComponents.DotNetBar;
using HHTiming.WinFormsControls.Workbook;

namespace HHTiming.Base.Graph
{
    public partial class GraphDisplay : 
        UserControl,
        IWorksheetControlInternal,
        IUIUpdateControl,
        IFilterableControl
    {
        //This is an example of how to create a basic graph control
        //In this case we are going to create a filterable graph that will show the amount of cars passed per lap
        //We will add a ribbon bar with a text box to set a reference car. The line for the reference car will be thicker than the rest

        protected HHOxyPlot _graph;
        protected HHOxyPlotLegendController _legendController;
        protected HHOxyPlotAxisLimitsController _axisLimitsController;
        private UICarManager<BasicGraphCar> _carManager;

        private List<BasicGraphCar> _cars = new List<BasicGraphCar>();

        public GraphDisplay()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Name = "Base Plugin Graph";

            _graph = new HHOxyPlot();
            _graph.Dock = DockStyle.Fill;
            Controls.Add(_graph);

            _graph.XAxis.PositionAtZeroCrossing = true;

            _graph.ChartTitle = "HH Timing Basic Graph"; //change the chart title
            _graph.XAxisTitle = "Lap #"; //change x axis label
            _graph.YAxisTitle = "Gap to reference lap time"; //change y axis label

            _legendController = new HHOxyPlotLegendController(_graph.PlotModel);
            _legendController.RefreshChart += HandleRefreshChart;

            _axisLimitsController = new HHOxyPlotAxisLimitsController(_graph);
            _axisLimitsController.RefreshChart += HandleRefreshChart;

            _carManager = new UICarManager<BasicGraphCar>(Color.Black, _cars, (x) =>
            {
                return new BasicGraphCar(x.ItemID, x.CarColor, x.CategoryString, x.TyreManufacturer);
            });  //the car manager is in charge of adding items to _cars list

            //Add data binding to handle the reference car
            tb_referenceCar.DataBindings.Add("Text", this, nameof(ReferenceCar), true, DataSourceUpdateMode.OnPropertyChanged);
        }

        #region Graph
        private void HandleRefreshChart(object sender, EventArgs e)
        {
            _graph.RefreshChart();
        }

        protected void RefreshGraph(bool aForceImmediateRefresh)
        {
            if (aForceImmediateRefresh)
            {
                _graph.RefreshChart();
            }
            else
            {
                _graph.SetTimerRefreshFlag();
            }
        }

        private string _referenceCar = "5";

        public string ReferenceCar
        {
            get => _referenceCar;
            set
            {
                if (_referenceCar == value)
                {
                    return;
                }

                _referenceCar = value;

                UpdateReferenceCar();
            }
        }

        private void UpdateReferenceCar()
        {
            foreach(var car in _cars)
            {
                car.UpdateLineThickness(car.CarID == _referenceCar); 
            }

            RefreshGraph(true);
        }

        #endregion

        #region IUIUpdateControl

        private Guid _controlID = Guid.NewGuid();

        public Guid ControlID
        {
            get
            {
                return _controlID;
            }
        }

        public bool ReinitializationFlag { get; set; } = false;


        public bool RequiresPaint
        {
            get
            {
                return false;
            }
        }

        public bool RunsInUIThread
        {
            get
            {
                return true;
            }
        }

        public bool UseBulkInitialization
        {
            get
            {
                return true;
            }
        }

        public DatabaseRequest[] GetDatabaseRequests()
        {
            //these are the messages that the control will receive when it is opened
            return new DatabaseRequest[] { new DatabaseRequest(eDatabaseRequestType.AllLapsWithSectorsAllCars, new string[] { }, _controlID) };
        }

        public List<IUIUpdateMessage> GetInitializationMessages(Guid aTargetControlID)
        {
            return null;
        }

        public void PaintControl(SessionStatusUIUpdateMessage aSessionUIUpdateMessage, bool aFlashFlag)
        {

        }

        public void ReceiveUIUpdateMessage(IUIUpdateMessage anUpdateMessage)
        {
            //this void handles all the messages coming from HHtiming
            //
            if (anUpdateMessage is BulkRefreshDataUIUpdateMessage bulkUpdateMsg)
            {
                RefreshFilter();

                foreach (IUIUpdateMessage bulkMsg in bulkUpdateMsg.ListOfUIUpdateMessages)
                {
                    ReceiveUIUpdateMessage(bulkMsg, false); //we only update the graph once the bulk messages is processed completed
                }

                RefreshGraph(true);
            }
            else
            {
                ReceiveUIUpdateMessage(anUpdateMessage, true);
            }

        }

        private void ReceiveUIUpdateMessage(IUIUpdateMessage msg, bool anAllowRefresh)
        {
            //this void process messages one by one
            if (msg is CarUIUpdateMessage carMsg)
            {
                _carManager.HandleCarUIUpdateMessage(carMsg);
            }
            else if (msg is CategoryUIUpdateMessage catMessage)
            {
                _carManager.HandleCategoryUIUpdateMessage(catMessage);
            }
            else if (msg is ResetUIUpdateMessage)
            {
                _graph.ClearSeries();
                _cars.Clear();
                _graph.RefreshChart();
            }
            else if (msg is LapUIUpdateMessage lapMsg)
            {
                HandleLapUIUpdateMessage(lapMsg, anAllowRefresh);
            }
            else if (msg is SectorUIUpdateMessage sectorMsg)
            {
                //do something with sectors information
            }
        }

        protected virtual void HandleLapUIUpdateMessage(LapUIUpdateMessage aMessage, bool anAllowRefresh)
        {
            if (!aMessage.Laptime.IsValid()) return;

            var car = _cars.FirstOrDefault((x) => x.CarID == aMessage.ItemID);

            if (car == null) return;

            car.Series.Points.Add(new OxyPlot.DataPoint(aMessage.LapNumber, aMessage.NumberOfCarsPassed));

            RefreshGraph(anAllowRefresh);
        }

        #endregion

            #region IWorksheetControl

        public bool CanBeSavedInLayout
        {
            get
            {
                return true;
            }
        }

        public bool DuplicateAllowed
        {
            get
            {
                return true;
            }
        }

        public bool IsAddedToProject
        {
            get
            {
                return false;
            }

            set
            {

            }
        }

        public bool RenameAllowed
        {
            get
            {
                return false;
            }
        }

        public string WorksheetName
        {
            get
            {
                return base.Name;
            }

            set
            {
                if (base.Name == value) return;

                base.Name = value;

            }
        }

        public Icon WorksheetIcon
        {
            get
            {
                return null;
            }
        }


        public event AddNewWorksheetEventHandler AddNewWorksheet;
        public event RequestCloseWorksheetEventHandler RequestCloseWorksheet;
        public event WorksheetNameChangedEventHandler WorksheetNameChanged;

        public bool CloseWorksheet()
        {
            return true;
        }

        public HHRibbonBar[] GetRibbonBars()
        {
            return null;
        }

        public void LoadFromXML(XmlElement parentXMLElement)
        {
            //Load values saved in the project file
            foreach (XmlNode item in parentXMLElement.ChildNodes)
            {
                switch (item.Name)
                {
                    case "ReferenceCar":
                        ReferenceCar = item.InnerText;
                        break;
                }
            }
        }

        public void SaveToXML(XmlElement parentXMLElement)
        {
            //save values to project
            XMLHelperFunctions.WriteToXML("ReferenceCar", _referenceCar, parentXMLElement);
        }

        #endregion


        public void RefreshFilter()
        {
            // check what series are allowed to be dislayed on the graph using filter information
            foreach (var car in _cars)
            {
                if (FilterObject.ShouldCarBeShown(car.CarID))
                {
                    if (!_graph.PlotModel.Series.Contains(car.Series))
                        _graph.AddSeries(car.Series);
                }
                else
                {
                    if (_graph.PlotModel.Series.Contains(car.Series))
                        _graph.RemoveSeries(car.Series);
                }
            }

            RefreshGraph(true);
        }

        public List<IUIUpdateMessage> BroadcastUIUpdateMessages()
        {
            return null;
        }

        public IProjectObject GetWorksheetProjectControl()
        {
            return null;
        }

        public List<IUIDbMessage> GetDatabaseMessages()
        {
            return null;
        }

        public RibbonBar[] GetRibbonBar()
        {
            return new RibbonBar[] { graphRibbonBar };
        }

        public FilterObject FilterObject { get; } = new FilterObject();

        public Control WorksheetControl
        {
            get
            {
                return this;
            }
        }
    }
}
