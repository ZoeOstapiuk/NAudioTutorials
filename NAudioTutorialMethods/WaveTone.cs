using System;
using NAudio.Wave;

namespace NAudioTutorialMethods
{
    public class WaveTone : WaveStream
    {
        private double time;

        public WaveTone(double freq, double amp)
        {
            this.time = 0;
            this.Frequency = freq;
            this.Amplitude = amp;
        }

        public double Frequency { get; set; }

        public double Amplitude { get; set; }

        public override long Length
        {
            get
            {
                return long.MaxValue;
            }
        }

        public override long Position { get; set; }

        public override WaveFormat WaveFormat
        {
            get
            {
                return new WaveFormat(44100, 16, 1);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int samples = count / 2;
            for (int i = 0; i < samples; i++)
            {
                double sine = Amplitude * Math.Sin(Math.PI * 2 * Frequency * time);
                time += 1.0 / 44100;
                short truncated = (short)Math.Round(sine * (Math.Pow(2, 15) - 1));
                buffer[i * 2] = (byte)(truncated & 0x00ff);
                buffer[i * 2 + 1] = (byte)(truncated & 0xff00);
            }

            return count;
        }
    }
}
