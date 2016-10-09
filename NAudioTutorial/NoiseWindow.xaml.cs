using System.Windows;
using NAudio.Wave;
using NAudioTutorialMethods;

namespace NAudioTutorial
{
    public partial class NoiseWindow : Window
    {
        private DirectSoundOut output;
        private BlockAlignReductionStream stream;
        private WaveTone waveTone;

        public NoiseWindow()
        {
            InitializeComponent();

            waveTone = new WaveTone(this.sldFreq.Value, this.sldAmpl.Value);
            stream = new BlockAlignReductionStream(waveTone);

            output = new DirectSoundOut();
            output.Init(stream);
        }

        private void btnMakeNoise_Click(object sender, RoutedEventArgs e)
        {
            waveTone.Frequency = this.sldFreq.Value;
            waveTone.Amplitude = this.sldAmpl.Value;

            output.Play();
        }

        private void btnStopNoise_Click(object sender, RoutedEventArgs e)
        {
            output.Stop();
        }

        private void sldFreq_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (waveTone != null)
            {
                waveTone.Frequency = this.sldFreq.Value; 
            }
        }

        private void sldAmpl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (waveTone != null)
            {
                waveTone.Amplitude = this.sldAmpl.Value;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (output != null)
            {
                output.Dispose();
                output = null;
            }

            if (waveTone != null)
            {
                waveTone.Dispose();
                waveTone = null;
            }

            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }
    }
}
