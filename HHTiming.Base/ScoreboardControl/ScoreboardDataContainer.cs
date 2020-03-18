using HHTiming.WinFormsControls.ElementConfiguration;
using HHTiming.WinFormsControls.Filtering;
using HHTiming.WinFormsControls.Scoreboards;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHTiming.Base.Scoreboard
{
    // The scoreboard data container is the class in charge of storing the data. All the display properties should be defined here
    // each data container = one row of the datagrid
    public class ScoreboardDataContainer :
          BaseUICar,
        IIdentifiableDataContainer,
        IHighlightRow,
        IFilterableItem,
        INotifyColumnChanged
    {

        public TimeDataType Position { get; set; } = new TimeDataType(nameof(Position), true, false, false, false, "", 0, false, false, false, 1, true);
        
        public int PositionInClass { get; set; }
       
        public string CarNumber
        {
            get { return myCarID; }
            set { myCarID = value; }
        }

        public string TeamName { get; set; }
        public string CarMake { get; set; }

        public string Category
        {
            get { return myCategoryID; }
            set { myCategoryID = value; }
        }

        [DisplayName("Car Passed")] //use display name to change the column header name on the dgv
        public int CarPassed { get; set; }

        [DisplayName("Sector Exit Time")]
        public TimeDataType LastSectorExitTime { get; set; } = new TimeDataType(nameof(LastSectorExitTime), false, true, false, false, "0.0", TimeDataType.RefDisplayMode.Value, false, true, false, 1, false);

        [Browsable(false)]
        public string ItemID
        {
            get { return CarNumber; }
        }

        [Browsable(false)]
        public string SecondarySortID
        {
            get { return Position.ToString(); }
        }

        public ScoreboardDataContainer(string aCarID, Color aColor, string aCategory) : base(aCarID, aColor, aCategory, "")
        {

        }

        [Browsable(false)] // use browsable false to avoid displaying the property on the datagridview
        public string CarID
        {
            get { return myCarID; }
        }

        [Browsable(false)]
        public string CategoryID
        {
            get { return myCategoryID; }
        }

        [Browsable(false)]
        public bool IsClassLeader
        {
            get { return PositionInClass == 1; }
        }

        [Browsable(false)]
        public bool IsRowHightLighted { get; set; }

        [Browsable(false)]
        public Color RowColour
        {
            get { return myCarColour; }

            set { }
        }

        public event NotifyColumnChangedEventEventHandler NotifyColumnChangedEvent;
        public void RaiseNotifyColumnChangedEvent(string aName)
        {
            NotifyColumnChangedEvent?.Invoke(aName);
        }

        [Browsable(false)]
        public double BestTheoBySector { get; set; } = double.MaxValue;
    }
    }
