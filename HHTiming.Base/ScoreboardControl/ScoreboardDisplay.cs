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

namespace HHTiming.Base.Scoreboard
{
    public partial class ScoreboardDisplay : 
        UserControl,
        IWorksheetControl,
        IUIUpdateControl,
        IConfigurableScoreboardAppearance,
        IFilterableControl
    {
        //This is an example of how to create a control that contains a scoreboard
        //The scoreboard on this example display last sector crossing time and the amount of cars passed in the last lap for a given car
        private ScoreboardControl myScoreboardControl;

        public ScoreboardDisplay()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            myScoreboardControl = new ScoreboardControl();
            this.Controls.Add(myScoreboardControl.ScoreboardDGV);
            this.Name = "Base Plugin Scoreboard";
        }

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
            //this void handles all the messages coming from HHtiming,
            //In this case we let the scoreboard control handle the messages of interest
            myScoreboardControl.HandleUIUpdateMessage(anUpdateMessage);
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
            myScoreboardControl.LoadFromXML(parentXMLElement);
        }

        public void SaveToXML(XmlElement parentXMLElement)
        {
            myScoreboardControl.SaveToXML(parentXMLElement);
        }

        #endregion

        public ScoreboardAppearanceViewModel ScoreboardAppearanceViewModel
        {
            get
            {
                return myScoreboardControl.ScoreboardAppearanceViewModel;
            }
        }


        public void RefreshFilter()
        {
            myScoreboardControl.UpdateFilter();
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

        public FilterObject FilterObject
        {
            get
            {
                return myScoreboardControl.FilterObject;
            }
        }

        public Control WorksheetControl
        {
            get
            {
                return this;
            }
        }
    }
}
