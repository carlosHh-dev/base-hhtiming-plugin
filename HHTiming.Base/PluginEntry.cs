using HHDev.ProjectFramework.Definitions;
using HHTiming.Base.Graph;
using HHTiming.Base.Properties;
using HHTiming.Base.Scoreboard;
using HHTiming.Core.Definitions.UIUpdate.Interfaces;
using HHTiming.DAL;
using HHTiming.Desktop.Definitions.PlugInFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHTiming.Base
{
    public class PluginEntry : IHHTimingPlugin
    {
        public string Name => "Base Plugin";

        public Guid PluginID => Guid.Parse("c8fd10e5-dff2-48d9-a1ae-b03681e125e6");

        public IOptionsObject Options => null;

        public Func<IHHTimingContext> HHTimingContextFactory { set { } }
        public Func<string, IProjectObject> OpenProjectObject { set { } }

        public bool LoadSuccessful => true;

        public List<LapNumericTagItemDefinition> LapNumericTagsDefinitions => null;

        public event EventHandler<NewWorksheetEventArgs> AddNewWorksheet;
        public event EventHandler<CreateNewProjectObjectEventArgs> AddNewProjectItem;

        public PluginEntry()
        {
            BuildRibbonBar();
        }

        public List<IUIUpdateControl> GetAllBackgroundUIUpdateControls()
        {
            return null;
        }

        public List<Type> GetDataImporters()
        {
            return null;
        }

        public List<MessageParserDefinition> GetMessageParsers()
        {
            return null;
        }

        public IProjectObjectManager GetProjectObjectManager()
        {
            return null;
        }

        public List<HHRibbonTab> GetRibbonTabs() => _ribbonTabs;

        public IWorksheetControlManager GetWorksheetControlManager()
        {
            return null;
        }

        public void SoftwareClosing()
        {
            
        }

        private List<HHRibbonTab> _ribbonTabs;
        private void BuildRibbonBar()
        {
            _ribbonTabs = new List<HHRibbonTab>();

            var tab1 = new HHRibbonTab("Base Plugin");
            _ribbonTabs.Add(tab1);

            var bar1 = new HHRibbonBar("Scoreboards"); //each ribbon bar group the controls
            tab1.Bars.Add(bar1);

            var addScoreboardButton = new HHRibbonButton("Basic Scoreboard", Resources.Scoreboard_48x48, (x) =>
            {
                AddNewWorksheet?.Invoke(this, new NewWorksheetEventArgs() { NewWorksheet = new ScoreboardDisplay(), TargetWorkbook = x });
            });
            bar1.Buttons.Add(addScoreboardButton);

            var bar2 = new HHRibbonBar("Graphs");
            tab1.Bars.Add(bar2);

            var addQualiScoreboardButton = new HHRibbonButton("Basic Graph", Resources.Graph_48, (x) =>
            {
                AddNewWorksheet?.Invoke(this, new NewWorksheetEventArgs() { NewWorksheet = new GraphDisplay(), TargetWorkbook = x });
            });
            bar2.Buttons.Add(addQualiScoreboardButton);
        }
    }
}
