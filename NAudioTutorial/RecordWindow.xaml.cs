using System.Collections.Generic;
using System.Windows;
using NAudio.Wave;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;

namespace NAudioTutorial
{
    public partial class RecordWindow : Window
    {
        // Microphone or so
        private WaveIn source = null;

        // An object to write new *.wav file
        private WaveFileWriter waveWriter = null;

        // Timer when recording started
        private Timer timer = null;
        private Stopwatch stopwatch;

        public RecordWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // There has to be precisely one wavein input device selected
            if (WaveIn.DeviceCount > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "WAV files (*.wav)|*.wav";
                
                // User has to pick a file
                if (save.ShowDialog() == true)
                {
                    // Setting wavein device
                    source = new WaveIn();
                    source.DeviceNumber = 0;
                    source.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(0).Channels);

                    // Initialize writer to create new .wav file with PATH and SOURCE (microphone...)
                    waveWriter = new WaveFileWriter(save.FileName, source.WaveFormat);

                    // When stops recording
                    source.DataAvailable += Source_DataAvailable;
                    source.StartRecording();

                    // Setting timers
                    stopwatch = Stopwatch.StartNew();
                    timer = new Timer((o) =>
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            this.lblTime.Content = (o as Stopwatch).Elapsed.ToString(@"hh\:mm\:ss");
                        });
                    },
                    stopwatch, 0, 1000);
                }
            }
            else
            {
                MessageBox.Show("There is no sound input devices.", this.Title);
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            // If something is being recorded
            if (source != null && stopwatch != null && stopwatch.IsRunning)
            {
                source.StopRecording();
                stopwatch.Stop();
                timer.Dispose();

                MessageBox.Show("The file was successfully saved as " + waveWriter.Filename, this.Title);
            }
        }

        private void Source_DataAvailable(object sender, WaveInEventArgs e)
        {
            waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
            waveWriter.Flush();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Dispose of everything
            if (source != null)
            {
                source.Dispose();
                source = null;
            }

            if (waveWriter != null)
            {
                waveWriter.Dispose();
                waveWriter = null;
            }

            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }
    }
}
