using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace Visualizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openWAVEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Wave File (*.wav)|*.wav";
            if (open.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.chart1.Series.Add("wave");
            this.chart1.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            this.chart1.Series["wave"].ChartArea = "ChartArea1";

            WaveChannel32 wave = new WaveChannel32(new WaveFileReader(open.FileName));

            byte[] buffer = new byte[16384];
            int read = 0;

            while(wave.Position < wave.Length)
            {
                read = wave.Read(buffer, 0, 16384);
                for (int i = 0; i < read / 4; i++)
                {
                    this.chart1.Series["wave"].Points.Add(BitConverter.ToSingle(buffer, i * 4));
                }
            }
        }
    }
}
