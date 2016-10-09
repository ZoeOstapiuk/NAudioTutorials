using System;
using NAudio.Wave;

namespace NAudioTutorialMethods
{
    public static class Methods
    {
        public static void PlayMP3(string path)
        {
            WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(path));
            BlockAlignReductionStream stream = new BlockAlignReductionStream(pcm);

            DirectSoundOut output = new DirectSoundOut();
            output.Init(stream);
            output.Play();
        }

        public static void PlayWAV(string path)
        {
            WaveFileReader wave = new WaveFileReader(path);

            DirectSoundOut output = new DirectSoundOut();
            output.Init(wave);
            output.Play();
        }

        public static void MP3toWAV(string mp3Path, string wavPath)
        {
            using (Mp3FileReader mp3 = new Mp3FileReader(mp3Path))
            {
                using (WaveStream wave = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(wavPath, wave);
                }
            }
        }
    }
}
