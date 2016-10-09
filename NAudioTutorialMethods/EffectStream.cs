using System;
using NAudio.Wave;

namespace NAudioTutorialMethods
{
    public class EffectStream : WaveStream
    {
        public WaveStream SourceStream { get; set; }

        public EffectStream(WaveStream source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            this.SourceStream = source;
        }

        public override long Length
        {
            get
            {
                return SourceStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return SourceStream.Position;
            }

            set
            {
                SourceStream.Position = value;
            }
        }

        public override WaveFormat WaveFormat
        {
            get
            {
                return SourceStream.WaveFormat;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = SourceStream.Read(buffer, offset, count);

            for (int i = 0; i < read / 4; i++)
            {
                float sample = BitConverter.ToSingle(buffer, i * 4);
                sample *= 0.5f;
                byte[] bytes = BitConverter.GetBytes(sample);

                buffer[i * 4 + 0] = bytes[0];
                buffer[i * 4 + 1] = bytes[1];
                buffer[i * 4 + 2] = bytes[2];
                buffer[i * 4 + 3] = bytes[3];
            }

            return read;
        }
    }
}
