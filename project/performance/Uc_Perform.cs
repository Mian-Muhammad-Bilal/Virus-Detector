using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace project.performance
{
    public partial class Uc_Perform : UserControl
    {
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;

        public Uc_Perform()
        {
            InitializeComponent();
            InitializeCounters();
            InitializeChart();
        }

        private void InitializeCounters()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
        }

        private void InitializeChart()
        {
            chart1.Series.Clear();
            Series cpuSeries = new Series("CPU");
            cpuSeries.ChartType = SeriesChartType.Line;
            cpuSeries.BorderWidth = 2;
            cpuSeries.Color = System.Drawing.Color.Blue;

            Series ramSeries = new Series("RAM");
            ramSeries.ChartType = SeriesChartType.Line;
            ramSeries.BorderWidth = 2;
            ramSeries.Color = System.Drawing.Color.Green;

            chart1.Series.Add(cpuSeries);
            chart1.Series.Add(ramSeries);

            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;

            chart1.ChartAreas[0].AxisX.Title = "Time";
            chart1.ChartAreas[0].AxisY.Title = "Usage (%)";
        }

        private void Uc_Perform_Load(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            float cpuUsage = cpuCounter.NextValue();
            float ramUsage = GetMemoryUsage();

            UpdateCPUUsage(cpuUsage);
            UpdateRAMUsage(ramUsage);

            AddDataToChart(cpuUsage, ramUsage);
        }

        private float GetMemoryUsage()
        {
            // Call NextValue twice to get a stable reading
            ramCounter.NextValue();
            return ramCounter.NextValue();
        }

        private void UpdateCPUUsage(float value)
        {
            int cpuUsage = (int)value;
            gunaProgressBarCPU.Value = cpuUsage;
            lbCPU.Text = $"{cpuUsage:F2}%";
        }

        private void UpdateRAMUsage(float value)
        {
            int ramUsage = (int)value;
            gunaProgressBarRAM.Value = ramUsage;
            lbRam.Text = $"{ramUsage:F2}%";
        }

        private void AddDataToChart(float cpuUsage, float ramUsage)
        {
            chart1.Series["CPU"].Points.AddY(cpuUsage);
            chart1.Series["RAM"].Points.AddY(ramUsage);

            if (chart1.Series["CPU"].Points.Count > 20)
            {
                chart1.Series["CPU"].Points.RemoveAt(0);
                chart1.Series["RAM"].Points.RemoveAt(0);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Uc_dash uc_Dash = new Uc_dash();
            uc_Dash.Show();
            this.Hide();
        }
    }
}
