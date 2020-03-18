using HHDev.Core.NETStandard.Extensions;
using HHTiming.Core.Definitions;
using HHTiming.Core.Definitions.Enums;
using HHTiming.Core.Definitions.UIUpdate.Implementations.Messages;
using HHTiming.Core.Definitions.UIUpdate.Interfaces;
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
    public class ScoreboardControl :
        BaseScoreboardControl<ScoreboardDataContainer>
    {
        private AdvBindingList<ScoreboardDataContainer> myList;
        private ScoreboardDGV<ScoreboardDataContainer> myDGV;
        private UICarManager<ScoreboardDataContainer> myUICarManager;

        private double _sessionLength = 21600;

        protected override ScoreboardDataContainer GetNewDataContainer() => null;

        public ScoreboardControl()
        {
            myList = new AdvBindingList<ScoreboardDataContainer>(new DefaultScoreboardSorter(ListSortDirection.Ascending), nameof(ScoreboardDataContainer.Position), new ScoreboardDataContainer("", Color.White, ""));
            myUICarManager = new UICarManager<ScoreboardDataContainer>(Color.White, myList, (x) => new ScoreboardDataContainer(x.ItemID, x.CarColor, x.CategoryString));
            myDGV = new ScoreboardDGV<ScoreboardDataContainer>(myList, "\\g", (x) => myUICarManager.GetCategoryColour(x), true, false, false);

            base.SetListAndDGV(myList, myDGV);
            base.SetAppearanceOptions();
            base.InitializeFiltering();
        }

        #region Message Handling

        public void HandleUIUpdateMessage(IUIUpdateMessage anUpdateMessage)
        {
            if (anUpdateMessage is BulkRefreshDataUIUpdateMessage)
            {
                // bulk messages is the list of messages the control receives when it is loaded
                //i.e. if the user opens the control in the middle of the race, it will receive the messages from 
                //the beginning to the current time
                var b = (BulkRefreshDataUIUpdateMessage)anUpdateMessage;

                foreach (var item in b.ListOfUIUpdateMessages)
                {
                    HandleUIUpdateMessage(item, false);
                }

                myDGV.AutoResizeRows();
                ResetBS();

            }
            else
            {
                //Handle a single message
                HandleUIUpdateMessage(anUpdateMessage, true);
            }
        }

        public void HandleUIUpdateMessage(IUIUpdateMessage anUpdateMessage, bool anAllowRefresh)
        {
            if (anUpdateMessage is CarUIUpdateMessage)
            {
                myUICarManager.HandleCarUIUpdateMessage((CarUIUpdateMessage)anUpdateMessage);
            }
            else if (anUpdateMessage is TrackUIUpdateMessage)
            {
                base.HandleTrackUIUpdateMessage((TrackUIUpdateMessage)anUpdateMessage);
            }
            else if (anUpdateMessage is PositionUIUpdateMessage)
            {
                PositionUIUpdateMessage m = (PositionUIUpdateMessage)anUpdateMessage;

                foreach (ScoreboardDataContainer item in myList.OriginalList) // to access all the rows in the dgv you have to loop through myList.OriginalList
                {
                    if (item.CarID == m.ItemID)
                    {
                        if (m.PosType == PositionUIUpdateMessage.PositionType.Overall)
                        {
                            UpdateTimeDataTypeValue(item.Position, item, nameof(item.Position), m.Position);
                        }
                        else
                        {
                            item.PositionInClass = m.Position;
                        }

                        return;

                    }

                }

            }
            else if (anUpdateMessage is CategoryUIUpdateMessage)
            {
                HandleCategoryUIUpdateMessage((CategoryUIUpdateMessage)anUpdateMessage);
            }
            else if (anUpdateMessage is ResetUIUpdateMessage)
            {
                base.HandleResetUIUpdateMessage();
                myUICarManager.Reset();
            }
            else if (anUpdateMessage is UserOptionsUIUpdateMessage)
            {
                HandleOptionsUIMessage((UserOptionsUIUpdateMessage)anUpdateMessage);
            }

            else if (anUpdateMessage is LapUIUpdateMessage lapMsg)
                HandleLapUIUpdateMessage(lapMsg);

            else if (anUpdateMessage is SectorUIUpdateMessage sectorMsg)
                HandleSectorUIUpdateMessage(sectorMsg);

            else if (anUpdateMessage is SessionStatusUIUpdateMessage sessionMsg)
                HandleSessionStatusUIUpdateMessage(sessionMsg);

            else if (anUpdateMessage is UserDefinedSessionLengthUIUpdateMessage sessionLengthMsg)
                HandleUserDefinedSessionLengthUIUpdateMessage(sessionLengthMsg);
        }

        private void HandleUserDefinedSessionLengthUIUpdateMessage(UserDefinedSessionLengthUIUpdateMessage sessionLengthMsg)
        {
            _sessionLength = sessionLengthMsg.SessionLengthHours * 3600;
        }

        private void HandleSectorUIUpdateMessage(SectorUIUpdateMessage sectorMsg)
        {
            // use this void to process sector information
            //in this case we are going to find the car for the current message and update the sector exit time

            var car = myList.OriginalList.FirstOrDefault(x => x.CarID == sectorMsg.ItemID);

            if (car == null || sectorMsg.SectorExitTime == -1)
            {
                return; //there is no car on the list that correspond to the item in the message
            }

            car.LastSectorExitTime.SetValue(sectorMsg.SectorExitTime);

        }

        private void HandleSessionStatusUIUpdateMessage(SessionStatusUIUpdateMessage sessionMsg)
        {

        }

        private void HandleLapUIUpdateMessage(LapUIUpdateMessage lapMsg)
        {
            // use this void to process lap information 
            //in this case we are going to update the number of car passed at the end of each lap
            var car = myList.OriginalList.FirstOrDefault(x => x.CarID == lapMsg.ItemID);

            if (car == null || lapMsg.NumberOfCarsPassed == int.MaxValue)
            {
                return; //there is no car on the list that correspond to the item in the message
                // or the number of car passed has not been set
            }

            car.CarPassed = lapMsg.NumberOfCarsPassed;
        }


        public void HandleCategoryUIUpdateMessage(CategoryUIUpdateMessage message)
        {
            myUICarManager.SetCategoryColour(message.ItemID, message.CategoryColour);
        }

        public void HandleOptionsUIMessage(UserOptionsUIUpdateMessage anOptionsUIMessage)
        {
            myDGV.HandleOptionsUIMessage(anOptionsUIMessage);
        }

        public override void HandleResetUIUpdateMessage()
        {
            base.HandleResetUIUpdateMessage();
            myUICarManager.Reset();
        }

        #endregion

    }
}
