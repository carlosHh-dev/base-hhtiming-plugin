namespace HHTiming.Base.Graph
{
    partial class GraphDisplay
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.graphRibbonBar = new DevComponents.DotNetBar.RibbonBar();
            this.itemContainer1 = new DevComponents.DotNetBar.ItemContainer();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.tb_referenceCar = new DevComponents.DotNetBar.TextBoxItem();

            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            // 
            // graphRibbonBar
            // 
            this.graphRibbonBar.AutoOverflowEnabled = true;
            // 
            // 
            // 
            this.graphRibbonBar.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.graphRibbonBar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.graphRibbonBar.ContainerControlProcessDialogKey = true;
            this.graphRibbonBar.DragDropSupport = true;
            this.graphRibbonBar.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainer1});
            this.graphRibbonBar.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.graphRibbonBar.Location = new System.Drawing.Point(70, 72);
            this.graphRibbonBar.Name = "graphRibbonBar";
            this.graphRibbonBar.Size = new System.Drawing.Size(930, 107);
            this.graphRibbonBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.graphRibbonBar.TabIndex = 1;
            this.graphRibbonBar.Text = "Graph Options";
            // 
            // 
            // 
            this.graphRibbonBar.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.graphRibbonBar.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;

            // itemContainer1
            // 
            // 
            // 
            // 
            this.itemContainer1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer1.Name = "itemContainer1";
            this.itemContainer1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1,
            this.tb_referenceCar});
            // 
            // 
            // 
            this.itemContainer1.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;

            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "Reference Car:";
            this.labelItem1.Width = 80;
            // 
            // tb_referenceCar
            // 
            this.tb_referenceCar.Name = "tb_referenceCar";
            this.tb_referenceCar.Text = "textBoxItem1";
            this.tb_referenceCar.WatermarkColor = System.Drawing.SystemColors.GrayText;
        }

        protected DevComponents.DotNetBar.RibbonBar graphRibbonBar;
        protected DevComponents.DotNetBar.ItemContainer itemContainer1;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        protected DevComponents.DotNetBar.TextBoxItem tb_referenceCar;

        #endregion
    }
}
