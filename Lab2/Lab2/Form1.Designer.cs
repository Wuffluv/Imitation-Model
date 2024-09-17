namespace Lab2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartHistogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblProcessed = new System.Windows.Forms.Label();
            this.lblRejected = new System.Windows.Forms.Label();
            this.lblAverageWait = new System.Windows.Forms.Label();
            this.btnStartSimulation = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartHistogram)).BeginInit();
            this.SuspendLayout();
            // 
            // chartHistogram
            // 
            this.chartHistogram.BackColor = System.Drawing.Color.DimGray;
            this.chartHistogram.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartArea1";
            this.chartHistogram.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartHistogram.Legends.Add(legend1);
            this.chartHistogram.Location = new System.Drawing.Point(83, 3);
            this.chartHistogram.Name = "chartHistogram";
            this.chartHistogram.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartHistogram.Series.Add(series1);
            this.chartHistogram.Size = new System.Drawing.Size(844, 309);
            this.chartHistogram.TabIndex = 0;
            this.chartHistogram.Text = "chart1";
            // 
            // lblProcessed
            // 
            this.lblProcessed.AutoSize = true;
            this.lblProcessed.Location = new System.Drawing.Point(80, 325);
            this.lblProcessed.Name = "lblProcessed";
            this.lblProcessed.Size = new System.Drawing.Size(0, 16);
            this.lblProcessed.TabIndex = 1;
            // 
            // lblRejected
            // 
            this.lblRejected.AutoSize = true;
            this.lblRejected.Location = new System.Drawing.Point(80, 378);
            this.lblRejected.Name = "lblRejected";
            this.lblRejected.Size = new System.Drawing.Size(0, 16);
            this.lblRejected.TabIndex = 2;
            // 
            // lblAverageWait
            // 
            this.lblAverageWait.AutoSize = true;
            this.lblAverageWait.Location = new System.Drawing.Point(80, 435);
            this.lblAverageWait.Name = "lblAverageWait";
            this.lblAverageWait.Size = new System.Drawing.Size(0, 16);
            this.lblAverageWait.TabIndex = 3;
            this.lblAverageWait.Click += new System.EventHandler(this.lblAverageWait_Click);
            // 
            // btnStartSimulation
            // 
            this.btnStartSimulation.BackColor = System.Drawing.Color.DimGray;
            this.btnStartSimulation.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnStartSimulation.Location = new System.Drawing.Point(701, 336);
            this.btnStartSimulation.Name = "btnStartSimulation";
            this.btnStartSimulation.Size = new System.Drawing.Size(226, 79);
            this.btnStartSimulation.TabIndex = 4;
            this.btnStartSimulation.Text = "Запуск симуляции";
            this.btnStartSimulation.UseVisualStyleBackColor = false;
            this.btnStartSimulation.Click += new System.EventHandler(this.btnStartSimulation_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaGreen;
            this.ClientSize = new System.Drawing.Size(981, 533);
            this.Controls.Add(this.btnStartSimulation);
            this.Controls.Add(this.lblAverageWait);
            this.Controls.Add(this.lblRejected);
            this.Controls.Add(this.lblProcessed);
            this.Controls.Add(this.chartHistogram);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chartHistogram)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartHistogram;
        private System.Windows.Forms.Label lblProcessed;
        private System.Windows.Forms.Label lblRejected;
        private System.Windows.Forms.Label lblAverageWait;
        private System.Windows.Forms.Button btnStartSimulation;
    }
}

