namespace AINav
{
    partial class View
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View));
            this.buttonStart = new System.Windows.Forms.Button();
            this.percentWalkable = new System.Windows.Forms.NumericUpDown();
            this.labelWalkablePercentage = new System.Windows.Forms.Label();
            this.labelNumberOfAgents = new System.Windows.Forms.Label();
            this.numberOfAgents = new System.Windows.Forms.NumericUpDown();
            this.labelAIAlgorithmUsed = new System.Windows.Forms.Label();
            this.algorithmType = new System.Windows.Forms.ComboBox();
            this.labelFieldOfView = new System.Windows.Forms.Label();
            this.fieldOfView = new System.Windows.Forms.NumericUpDown();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.simulationListView = new System.Windows.Forms.ListView();
            this.viewableAgent = new System.Windows.Forms.ComboBox();
            this.labelActiveAgent = new System.Windows.Forms.Label();
            this.buttonClearResults = new System.Windows.Forms.Button();
            this.buttonEmptyMap = new System.Windows.Forms.Button();
            this.buttonRandomMap = new System.Windows.Forms.Button();
            this.heuristicMethod = new System.Windows.Forms.ComboBox();
            this.labelAIHeuristicMethod = new System.Windows.Forms.Label();
            this.labelMaxSteps = new System.Windows.Forms.Label();
            this.maxSteps = new System.Windows.Forms.NumericUpDown();
            this.labelStepDelay = new System.Windows.Forms.Label();
            this.stepDelay = new System.Windows.Forms.NumericUpDown();
            this.labelMapSize = new System.Windows.Forms.Label();
            this.mapSize = new System.Windows.Forms.NumericUpDown();
            this.panelMap = new System.Windows.Forms.Panel();
            this.groupAiParameters = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.agentCooperation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fovMethod = new System.Windows.Forms.ComboBox();
            this.labelPersistence = new System.Windows.Forms.Label();
            this.pathPersistence = new System.Windows.Forms.ComboBox();
            this.groupMapGenerator = new System.Windows.Forms.GroupBox();
            this.labelEmpty = new System.Windows.Forms.Label();
            this.labelRandom = new System.Windows.Forms.Label();
            this.groupMap = new System.Windows.Forms.GroupBox();
            this.labelEdit = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButtonEditFinish = new System.Windows.Forms.RadioButton();
            this.radioButtonEditStart = new System.Windows.Forms.RadioButton();
            this.radioButtonEditNonWalkable = new System.Windows.Forms.RadioButton();
            this.radioButtonEditWalkable = new System.Windows.Forms.RadioButton();
            this.buttonResetMap = new System.Windows.Forms.Button();
            this.buttonExportCSV = new System.Windows.Forms.Button();
            this.groupPreferences = new System.Windows.Forms.GroupBox();
            this.labelVisualizationDelay = new System.Windows.Forms.Label();
            this.visualizationDelay = new System.Windows.Forms.NumericUpDown();
            this.labelVisualizations = new System.Windows.Forms.Label();
            this.visualizations = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSimulation = new System.Windows.Forms.TabPage();
            this.tabPageDataLog = new System.Windows.Forms.TabPage();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.percentWalkable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfAgents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldOfView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapSize)).BeginInit();
            this.groupAiParameters.SuspendLayout();
            this.groupMapGenerator.SuspendLayout();
            this.groupMap.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupPreferences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visualizationDelay)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSimulation.SuspendLayout();
            this.tabPageDataLog.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Enabled = false;
            this.buttonStart.Location = new System.Drawing.Point(8, 18);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(50, 25);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            // 
            // percentWalkable
            // 
            this.percentWalkable.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.percentWalkable.Location = new System.Drawing.Point(131, 37);
            this.percentWalkable.Name = "percentWalkable";
            this.percentWalkable.Size = new System.Drawing.Size(104, 20);
            this.percentWalkable.TabIndex = 6;
            // 
            // labelWalkablePercentage
            // 
            this.labelWalkablePercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWalkablePercentage.Location = new System.Drawing.Point(24, 37);
            this.labelWalkablePercentage.Name = "labelWalkablePercentage";
            this.labelWalkablePercentage.Size = new System.Drawing.Size(101, 20);
            this.labelWalkablePercentage.TabIndex = 7;
            this.labelWalkablePercentage.Text = "% Walkable";
            this.labelWalkablePercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelNumberOfAgents
            // 
            this.labelNumberOfAgents.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumberOfAgents.Location = new System.Drawing.Point(24, 57);
            this.labelNumberOfAgents.Name = "labelNumberOfAgents";
            this.labelNumberOfAgents.Size = new System.Drawing.Size(101, 20);
            this.labelNumberOfAgents.TabIndex = 9;
            this.labelNumberOfAgents.Text = "# of Agents";
            this.labelNumberOfAgents.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numberOfAgents
            // 
            this.numberOfAgents.Location = new System.Drawing.Point(131, 57);
            this.numberOfAgents.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numberOfAgents.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfAgents.Name = "numberOfAgents";
            this.numberOfAgents.Size = new System.Drawing.Size(104, 20);
            this.numberOfAgents.TabIndex = 8;
            this.numberOfAgents.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelAIAlgorithmUsed
            // 
            this.labelAIAlgorithmUsed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAIAlgorithmUsed.Location = new System.Drawing.Point(24, 14);
            this.labelAIAlgorithmUsed.Name = "labelAIAlgorithmUsed";
            this.labelAIAlgorithmUsed.Size = new System.Drawing.Size(101, 20);
            this.labelAIAlgorithmUsed.TabIndex = 10;
            this.labelAIAlgorithmUsed.Text = "Algorithm";
            this.labelAIAlgorithmUsed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // algorithmType
            // 
            this.algorithmType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.algorithmType.FormattingEnabled = true;
            this.algorithmType.Location = new System.Drawing.Point(131, 14);
            this.algorithmType.Name = "algorithmType";
            this.algorithmType.Size = new System.Drawing.Size(104, 21);
            this.algorithmType.TabIndex = 11;
            // 
            // labelFieldOfView
            // 
            this.labelFieldOfView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFieldOfView.Location = new System.Drawing.Point(24, 56);
            this.labelFieldOfView.Name = "labelFieldOfView";
            this.labelFieldOfView.Size = new System.Drawing.Size(101, 20);
            this.labelFieldOfView.TabIndex = 15;
            this.labelFieldOfView.Text = "Field of View";
            this.labelFieldOfView.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fieldOfView
            // 
            this.fieldOfView.Location = new System.Drawing.Point(131, 56);
            this.fieldOfView.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.fieldOfView.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.fieldOfView.Name = "fieldOfView";
            this.fieldOfView.Size = new System.Drawing.Size(104, 20);
            this.fieldOfView.TabIndex = 14;
            this.fieldOfView.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // buttonCancel
            // 
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(58, 18);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(50, 25);
            this.buttonCancel.TabIndex = 16;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // simulationListView
            // 
            this.simulationListView.AllowColumnReorder = true;
            this.simulationListView.BackColor = System.Drawing.SystemColors.Control;
            this.simulationListView.Location = new System.Drawing.Point(3, 6);
            this.simulationListView.Name = "simulationListView";
            this.simulationListView.Size = new System.Drawing.Size(680, 417);
            this.simulationListView.TabIndex = 27;
            this.simulationListView.UseCompatibleStateImageBehavior = false;
            this.simulationListView.View = System.Windows.Forms.View.Details;
            // 
            // viewableAgent
            // 
            this.viewableAgent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewableAgent.ForeColor = System.Drawing.SystemColors.WindowText;
            this.viewableAgent.FormattingEnabled = true;
            this.viewableAgent.Location = new System.Drawing.Point(130, 17);
            this.viewableAgent.Name = "viewableAgent";
            this.viewableAgent.Size = new System.Drawing.Size(104, 21);
            this.viewableAgent.TabIndex = 28;
            // 
            // labelActiveAgent
            // 
            this.labelActiveAgent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelActiveAgent.Location = new System.Drawing.Point(23, 17);
            this.labelActiveAgent.Name = "labelActiveAgent";
            this.labelActiveAgent.Size = new System.Drawing.Size(101, 20);
            this.labelActiveAgent.TabIndex = 29;
            this.labelActiveAgent.Text = "Viewable Agent";
            this.labelActiveAgent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonClearResults
            // 
            this.buttonClearResults.Location = new System.Drawing.Point(6, 429);
            this.buttonClearResults.Name = "buttonClearResults";
            this.buttonClearResults.Size = new System.Drawing.Size(74, 25);
            this.buttonClearResults.TabIndex = 36;
            this.buttonClearResults.Text = "Clear All";
            this.buttonClearResults.UseVisualStyleBackColor = true;
            // 
            // buttonEmptyMap
            // 
            this.buttonEmptyMap.Image = global::AINav.Properties.Resources.Empty;
            this.buttonEmptyMap.Location = new System.Drawing.Point(200, 82);
            this.buttonEmptyMap.Name = "buttonEmptyMap";
            this.buttonEmptyMap.Size = new System.Drawing.Size(35, 35);
            this.buttonEmptyMap.TabIndex = 25;
            this.buttonEmptyMap.UseVisualStyleBackColor = true;
            // 
            // buttonRandomMap
            // 
            this.buttonRandomMap.Image = global::AINav.Properties.Resources.Random;
            this.buttonRandomMap.Location = new System.Drawing.Point(90, 82);
            this.buttonRandomMap.Name = "buttonRandomMap";
            this.buttonRandomMap.Size = new System.Drawing.Size(35, 35);
            this.buttonRandomMap.TabIndex = 0;
            this.buttonRandomMap.UseVisualStyleBackColor = true;
            // 
            // heuristicMethod
            // 
            this.heuristicMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.heuristicMethod.FormattingEnabled = true;
            this.heuristicMethod.Location = new System.Drawing.Point(131, 35);
            this.heuristicMethod.Name = "heuristicMethod";
            this.heuristicMethod.Size = new System.Drawing.Size(104, 21);
            this.heuristicMethod.TabIndex = 37;
            // 
            // labelAIHeuristicMethod
            // 
            this.labelAIHeuristicMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAIHeuristicMethod.Location = new System.Drawing.Point(24, 35);
            this.labelAIHeuristicMethod.Name = "labelAIHeuristicMethod";
            this.labelAIHeuristicMethod.Size = new System.Drawing.Size(101, 20);
            this.labelAIHeuristicMethod.TabIndex = 38;
            this.labelAIHeuristicMethod.Text = "Heuristic Method";
            this.labelAIHeuristicMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMaxSteps
            // 
            this.labelMaxSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxSteps.Location = new System.Drawing.Point(23, 81);
            this.labelMaxSteps.Name = "labelMaxSteps";
            this.labelMaxSteps.Size = new System.Drawing.Size(101, 20);
            this.labelMaxSteps.TabIndex = 40;
            this.labelMaxSteps.Text = "Max Steps";
            this.labelMaxSteps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maxSteps
            // 
            this.maxSteps.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.maxSteps.Location = new System.Drawing.Point(130, 81);
            this.maxSteps.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.maxSteps.Name = "maxSteps";
            this.maxSteps.Size = new System.Drawing.Size(104, 20);
            this.maxSteps.TabIndex = 39;
            // 
            // labelStepDelay
            // 
            this.labelStepDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStepDelay.Location = new System.Drawing.Point(23, 101);
            this.labelStepDelay.Name = "labelStepDelay";
            this.labelStepDelay.Size = new System.Drawing.Size(101, 20);
            this.labelStepDelay.TabIndex = 42;
            this.labelStepDelay.Text = "Step Delay";
            this.labelStepDelay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // stepDelay
            // 
            this.stepDelay.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.stepDelay.Location = new System.Drawing.Point(130, 101);
            this.stepDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.stepDelay.Name = "stepDelay";
            this.stepDelay.Size = new System.Drawing.Size(104, 20);
            this.stepDelay.TabIndex = 41;
            // 
            // labelMapSize
            // 
            this.labelMapSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMapSize.Location = new System.Drawing.Point(24, 17);
            this.labelMapSize.Name = "labelMapSize";
            this.labelMapSize.Size = new System.Drawing.Size(101, 20);
            this.labelMapSize.TabIndex = 44;
            this.labelMapSize.Text = "Map Size";
            this.labelMapSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mapSize
            // 
            this.mapSize.Location = new System.Drawing.Point(131, 17);
            this.mapSize.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.mapSize.Name = "mapSize";
            this.mapSize.Size = new System.Drawing.Size(104, 20);
            this.mapSize.TabIndex = 43;
            // 
            // panelMap
            // 
            this.panelMap.AutoScroll = true;
            this.panelMap.AutoSize = true;
            this.panelMap.Location = new System.Drawing.Point(10, 16);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(400, 400);
            this.panelMap.TabIndex = 1;
            // 
            // groupAiParameters
            // 
            this.groupAiParameters.Controls.Add(this.label2);
            this.groupAiParameters.Controls.Add(this.agentCooperation);
            this.groupAiParameters.Controls.Add(this.label1);
            this.groupAiParameters.Controls.Add(this.fovMethod);
            this.groupAiParameters.Controls.Add(this.labelAIAlgorithmUsed);
            this.groupAiParameters.Controls.Add(this.algorithmType);
            this.groupAiParameters.Controls.Add(this.fieldOfView);
            this.groupAiParameters.Controls.Add(this.labelFieldOfView);
            this.groupAiParameters.Controls.Add(this.labelAIHeuristicMethod);
            this.groupAiParameters.Controls.Add(this.heuristicMethod);
            this.groupAiParameters.Location = new System.Drawing.Point(6, 6);
            this.groupAiParameters.Name = "groupAiParameters";
            this.groupAiParameters.Size = new System.Drawing.Size(245, 126);
            this.groupAiParameters.TabIndex = 46;
            this.groupAiParameters.TabStop = false;
            this.groupAiParameters.Text = "AI Parameters";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 20);
            this.label2.TabIndex = 42;
            this.label2.Text = "Agent Cooperation";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // agentCooperation
            // 
            this.agentCooperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.agentCooperation.FormattingEnabled = true;
            this.agentCooperation.Location = new System.Drawing.Point(131, 97);
            this.agentCooperation.Name = "agentCooperation";
            this.agentCooperation.Size = new System.Drawing.Size(104, 21);
            this.agentCooperation.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 20);
            this.label1.TabIndex = 40;
            this.label1.Text = "Field of View Method";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fovMethod
            // 
            this.fovMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fovMethod.FormattingEnabled = true;
            this.fovMethod.Location = new System.Drawing.Point(131, 76);
            this.fovMethod.Name = "fovMethod";
            this.fovMethod.Size = new System.Drawing.Size(104, 21);
            this.fovMethod.TabIndex = 39;
            // 
            // labelPersistence
            // 
            this.labelPersistence.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPersistence.Location = new System.Drawing.Point(23, 38);
            this.labelPersistence.Name = "labelPersistence";
            this.labelPersistence.Size = new System.Drawing.Size(101, 20);
            this.labelPersistence.TabIndex = 46;
            this.labelPersistence.Text = "Persistence";
            this.labelPersistence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pathPersistence
            // 
            this.pathPersistence.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pathPersistence.FormattingEnabled = true;
            this.pathPersistence.Location = new System.Drawing.Point(130, 38);
            this.pathPersistence.Name = "pathPersistence";
            this.pathPersistence.Size = new System.Drawing.Size(104, 21);
            this.pathPersistence.TabIndex = 45;
            // 
            // groupMapGenerator
            // 
            this.groupMapGenerator.Controls.Add(this.labelEmpty);
            this.groupMapGenerator.Controls.Add(this.labelRandom);
            this.groupMapGenerator.Controls.Add(this.buttonRandomMap);
            this.groupMapGenerator.Controls.Add(this.buttonEmptyMap);
            this.groupMapGenerator.Controls.Add(this.labelMapSize);
            this.groupMapGenerator.Controls.Add(this.numberOfAgents);
            this.groupMapGenerator.Controls.Add(this.mapSize);
            this.groupMapGenerator.Controls.Add(this.percentWalkable);
            this.groupMapGenerator.Controls.Add(this.labelNumberOfAgents);
            this.groupMapGenerator.Controls.Add(this.labelWalkablePercentage);
            this.groupMapGenerator.Location = new System.Drawing.Point(6, 133);
            this.groupMapGenerator.Name = "groupMapGenerator";
            this.groupMapGenerator.Size = new System.Drawing.Size(245, 120);
            this.groupMapGenerator.TabIndex = 47;
            this.groupMapGenerator.TabStop = false;
            this.groupMapGenerator.Text = "Map Generator";
            // 
            // labelEmpty
            // 
            this.labelEmpty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEmpty.Location = new System.Drawing.Point(156, 89);
            this.labelEmpty.Name = "labelEmpty";
            this.labelEmpty.Size = new System.Drawing.Size(41, 20);
            this.labelEmpty.TabIndex = 46;
            this.labelEmpty.Text = "Empty";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelRandom
            // 
            this.labelRandom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRandom.Location = new System.Drawing.Point(30, 89);
            this.labelRandom.Name = "labelRandom";
            this.labelRandom.Size = new System.Drawing.Size(57, 20);
            this.labelRandom.TabIndex = 45;
            this.labelRandom.Text = "Random";
            this.labelRandom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupMap
            // 
            this.groupMap.Controls.Add(this.labelEdit);
            this.groupMap.Controls.Add(this.panel1);
            this.groupMap.Controls.Add(this.panelMap);
            this.groupMap.Location = new System.Drawing.Point(257, 6);
            this.groupMap.Name = "groupMap";
            this.groupMap.Size = new System.Drawing.Size(422, 454);
            this.groupMap.TabIndex = 49;
            this.groupMap.TabStop = false;
            this.groupMap.Text = "Map";
            // 
            // labelEdit
            // 
            this.labelEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEdit.Location = new System.Drawing.Point(143, 424);
            this.labelEdit.Name = "labelEdit";
            this.labelEdit.Size = new System.Drawing.Size(106, 21);
            this.labelEdit.TabIndex = 49;
            this.labelEdit.Text = "Map Node Editor";
            this.labelEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButtonEditFinish);
            this.panel1.Controls.Add(this.radioButtonEditStart);
            this.panel1.Controls.Add(this.radioButtonEditNonWalkable);
            this.panel1.Controls.Add(this.radioButtonEditWalkable);
            this.panel1.Location = new System.Drawing.Point(255, 419);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(155, 30);
            this.panel1.TabIndex = 55;
            // 
            // radioButtonEditFinish
            // 
            this.radioButtonEditFinish.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonEditFinish.Location = new System.Drawing.Point(119, 0);
            this.radioButtonEditFinish.Name = "radioButtonEditFinish";
            this.radioButtonEditFinish.Size = new System.Drawing.Size(30, 30);
            this.radioButtonEditFinish.TabIndex = 55;
            this.radioButtonEditFinish.TabStop = true;
            this.radioButtonEditFinish.Text = "F";
            this.radioButtonEditFinish.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonEditFinish.UseVisualStyleBackColor = true;
            // 
            // radioButtonEditStart
            // 
            this.radioButtonEditStart.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonEditStart.Location = new System.Drawing.Point(81, 0);
            this.radioButtonEditStart.Name = "radioButtonEditStart";
            this.radioButtonEditStart.Size = new System.Drawing.Size(30, 30);
            this.radioButtonEditStart.TabIndex = 54;
            this.radioButtonEditStart.TabStop = true;
            this.radioButtonEditStart.Text = "S";
            this.radioButtonEditStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonEditStart.UseVisualStyleBackColor = true;
            // 
            // radioButtonEditNonWalkable
            // 
            this.radioButtonEditNonWalkable.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonEditNonWalkable.Image = global::AINav.Properties.Resources.NonWalkable;
            this.radioButtonEditNonWalkable.Location = new System.Drawing.Point(43, 0);
            this.radioButtonEditNonWalkable.Name = "radioButtonEditNonWalkable";
            this.radioButtonEditNonWalkable.Size = new System.Drawing.Size(30, 30);
            this.radioButtonEditNonWalkable.TabIndex = 53;
            this.radioButtonEditNonWalkable.TabStop = true;
            this.radioButtonEditNonWalkable.UseVisualStyleBackColor = true;
            // 
            // radioButtonEditWalkable
            // 
            this.radioButtonEditWalkable.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonEditWalkable.Image = global::AINav.Properties.Resources.Walkable;
            this.radioButtonEditWalkable.Location = new System.Drawing.Point(5, 0);
            this.radioButtonEditWalkable.Name = "radioButtonEditWalkable";
            this.radioButtonEditWalkable.Size = new System.Drawing.Size(30, 30);
            this.radioButtonEditWalkable.TabIndex = 52;
            this.radioButtonEditWalkable.TabStop = true;
            this.radioButtonEditWalkable.UseVisualStyleBackColor = true;
            // 
            // buttonResetMap
            // 
            this.buttonResetMap.Location = new System.Drawing.Point(108, 19);
            this.buttonResetMap.Name = "buttonResetMap";
            this.buttonResetMap.Size = new System.Drawing.Size(50, 24);
            this.buttonResetMap.TabIndex = 47;
            this.buttonResetMap.Text = "Reset";
            this.buttonResetMap.UseVisualStyleBackColor = true;
            // 
            // buttonExportCSV
            // 
            this.buttonExportCSV.Location = new System.Drawing.Point(86, 429);
            this.buttonExportCSV.Name = "buttonExportCSV";
            this.buttonExportCSV.Size = new System.Drawing.Size(74, 25);
            this.buttonExportCSV.TabIndex = 37;
            this.buttonExportCSV.Text = "Export .csv";
            this.buttonExportCSV.UseVisualStyleBackColor = true;
            // 
            // groupPreferences
            // 
            this.groupPreferences.Controls.Add(this.labelVisualizationDelay);
            this.groupPreferences.Controls.Add(this.visualizationDelay);
            this.groupPreferences.Controls.Add(this.labelVisualizations);
            this.groupPreferences.Controls.Add(this.visualizations);
            this.groupPreferences.Controls.Add(this.labelMaxSteps);
            this.groupPreferences.Controls.Add(this.labelPersistence);
            this.groupPreferences.Controls.Add(this.labelStepDelay);
            this.groupPreferences.Controls.Add(this.viewableAgent);
            this.groupPreferences.Controls.Add(this.stepDelay);
            this.groupPreferences.Controls.Add(this.pathPersistence);
            this.groupPreferences.Controls.Add(this.maxSteps);
            this.groupPreferences.Controls.Add(this.labelActiveAgent);
            this.groupPreferences.Location = new System.Drawing.Point(6, 254);
            this.groupPreferences.Name = "groupPreferences";
            this.groupPreferences.Size = new System.Drawing.Size(245, 148);
            this.groupPreferences.TabIndex = 0;
            this.groupPreferences.TabStop = false;
            this.groupPreferences.Text = "Simulation Preferences";
            // 
            // labelVisualizationDelay
            // 
            this.labelVisualizationDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisualizationDelay.Location = new System.Drawing.Point(23, 121);
            this.labelVisualizationDelay.Name = "labelVisualizationDelay";
            this.labelVisualizationDelay.Size = new System.Drawing.Size(101, 20);
            this.labelVisualizationDelay.TabIndex = 50;
            this.labelVisualizationDelay.Text = "Visualization Delay";
            this.labelVisualizationDelay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // visualizationDelay
            // 
            this.visualizationDelay.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.visualizationDelay.Location = new System.Drawing.Point(130, 121);
            this.visualizationDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.visualizationDelay.Name = "visualizationDelay";
            this.visualizationDelay.Size = new System.Drawing.Size(104, 20);
            this.visualizationDelay.TabIndex = 49;
            // 
            // labelVisualizations
            // 
            this.labelVisualizations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisualizations.Location = new System.Drawing.Point(23, 59);
            this.labelVisualizations.Name = "labelVisualizations";
            this.labelVisualizations.Size = new System.Drawing.Size(101, 20);
            this.labelVisualizations.TabIndex = 48;
            this.labelVisualizations.Text = "Visualizations";
            this.labelVisualizations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // visualizations
            // 
            this.visualizations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.visualizations.FormattingEnabled = true;
            this.visualizations.Location = new System.Drawing.Point(130, 59);
            this.visualizations.Name = "visualizations";
            this.visualizations.Size = new System.Drawing.Size(104, 21);
            this.visualizations.TabIndex = 47;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonResetMap);
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Controls.Add(this.buttonCancel);
            this.groupBox1.Location = new System.Drawing.Point(6, 408);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 52);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSimulation);
            this.tabControl1.Controls.Add(this.tabPageDataLog);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(697, 492);
            this.tabControl1.TabIndex = 58;
            // 
            // tabPageSimulation
            // 
            this.tabPageSimulation.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageSimulation.Controls.Add(this.groupAiParameters);
            this.tabPageSimulation.Controls.Add(this.groupBox1);
            this.tabPageSimulation.Controls.Add(this.groupMapGenerator);
            this.tabPageSimulation.Controls.Add(this.groupMap);
            this.tabPageSimulation.Controls.Add(this.groupPreferences);
            this.tabPageSimulation.Location = new System.Drawing.Point(4, 22);
            this.tabPageSimulation.Name = "tabPageSimulation";
            this.tabPageSimulation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSimulation.Size = new System.Drawing.Size(689, 466);
            this.tabPageSimulation.TabIndex = 0;
            this.tabPageSimulation.Text = "Simulation";
            // 
            // tabPageDataLog
            // 
            this.tabPageDataLog.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDataLog.Controls.Add(this.simulationListView);
            this.tabPageDataLog.Controls.Add(this.buttonExportCSV);
            this.tabPageDataLog.Controls.Add(this.buttonClearResults);
            this.tabPageDataLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataLog.Name = "tabPageDataLog";
            this.tabPageDataLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataLog.Size = new System.Drawing.Size(689, 466);
            this.tabPageDataLog.TabIndex = 1;
            this.tabPageDataLog.Text = "Data Log";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip.Location = new System.Drawing.Point(0, 509);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(715, 22);
            this.statusStrip.TabIndex = 59;
            this.statusStrip.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(39, 17);
            this.status.Text = "Status";
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 531);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "View";
            this.Text = "AI Cooperative Path Finding Simulator by Brett Henne";
            ((System.ComponentModel.ISupportInitialize)(this.percentWalkable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfAgents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldOfView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapSize)).EndInit();
            this.groupAiParameters.ResumeLayout(false);
            this.groupMapGenerator.ResumeLayout(false);
            this.groupMap.ResumeLayout(false);
            this.groupMap.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupPreferences.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.visualizationDelay)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSimulation.ResumeLayout(false);
            this.tabPageDataLog.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRandomMap;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.NumericUpDown percentWalkable;
        private System.Windows.Forms.Label labelWalkablePercentage;
        private System.Windows.Forms.Label labelNumberOfAgents;
        private System.Windows.Forms.NumericUpDown numberOfAgents;
        private System.Windows.Forms.Label labelAIAlgorithmUsed;
        private System.Windows.Forms.ComboBox algorithmType;
        private System.Windows.Forms.Label labelFieldOfView;
        private System.Windows.Forms.NumericUpDown fieldOfView;
        private System.Windows.Forms.Button buttonEmptyMap;
        private System.Windows.Forms.ListView simulationListView;
        private System.Windows.Forms.ComboBox viewableAgent;
        private System.Windows.Forms.Label labelActiveAgent;
        private System.Windows.Forms.Button buttonClearResults;
        private System.Windows.Forms.ComboBox heuristicMethod;
        private System.Windows.Forms.Label labelAIHeuristicMethod;
        private System.Windows.Forms.Label labelMaxSteps;
        private System.Windows.Forms.NumericUpDown maxSteps;
        private System.Windows.Forms.Label labelStepDelay;
        private System.Windows.Forms.NumericUpDown stepDelay;
        private System.Windows.Forms.Label labelMapSize;
        private System.Windows.Forms.NumericUpDown mapSize;
        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.GroupBox groupAiParameters;
        private System.Windows.Forms.GroupBox groupMapGenerator;
        private System.Windows.Forms.GroupBox groupMap;
        private System.Windows.Forms.Label labelPersistence;
        private System.Windows.Forms.ComboBox pathPersistence;
        private System.Windows.Forms.GroupBox groupPreferences;
        private System.Windows.Forms.Label labelEmpty;
        private System.Windows.Forms.Label labelRandom;
        private System.Windows.Forms.Button buttonResetMap;
        protected internal System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox fovMethod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox agentCooperation;
        private System.Windows.Forms.RadioButton radioButtonEditWalkable;
        private System.Windows.Forms.RadioButton radioButtonEditNonWalkable;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButtonEditFinish;
        private System.Windows.Forms.RadioButton radioButtonEditStart;
        private System.Windows.Forms.Label labelEdit;
        private System.Windows.Forms.Button buttonExportCSV;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelVisualizations;
        private System.Windows.Forms.ComboBox visualizations;
        private System.Windows.Forms.Label labelVisualizationDelay;
        private System.Windows.Forms.NumericUpDown visualizationDelay;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSimulation;
        private System.Windows.Forms.TabPage tabPageDataLog;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel status;
    }
}

