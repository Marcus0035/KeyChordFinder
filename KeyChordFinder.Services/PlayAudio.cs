using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace KeyChordFinder.Services
{
    public class PlayAudio
    {
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isPlaying;
        
        private async Task PlayMetronomeAsync(string fileName, int bpm ,double volume, CancellationToken cancellationToken)
        {
            var path = GetMetronomeSamplePath(fileName);
            await using var audioFile = new AudioFileReader(path);
            var volumeProvider = new VolumeSampleProvider(audioFile.ToSampleProvider())
            {
                Volume = (float)volume
            };

            using var outputDevice = new WaveOutEvent();
            outputDevice.Init(volumeProvider);

            var interval = 60000 / bpm;
            long nextTick = Environment.TickCount;

            while (!cancellationToken.IsCancellationRequested)
            {
                long startTick = Environment.TickCount;
                audioFile.Position = 0;
                outputDevice.Play();
                var trackLengthMs = (int)audioFile.TotalTime.TotalMilliseconds;
                await Task.Delay(Math.Min(trackLengthMs, interval), cancellationToken);

                outputDevice.Stop();

                nextTick += interval;
                var sleepTime = (int)(nextTick - Environment.TickCount);

                if (sleepTime > 0)
                {
                    await Task.Delay(sleepTime, cancellationToken);
                }
            }
        }
        public void ToggleMetronome(string fileName, int bpm ,double volume)
        {
            if (_isPlaying) this.StopMetronome();
            _cancellationTokenSource = new CancellationTokenSource();
            _isPlaying = true;
            Task.Run(() => PlayMetronomeAsync(fileName, bpm, volume ,_cancellationTokenSource.Token));
        }
        public void StopMetronome()
        {
            _cancellationTokenSource?.Cancel();
            _isPlaying = false;
        }
        
        public async Task PlaySample(string sampleName)
        {
            await using var audioFile = new AudioFileReader(sampleName);
            var volumeProvider = new VolumeSampleProvider(audioFile.ToSampleProvider())
            {
                Volume = 0.8f
            };
            using var outputDevice = new WaveOutEvent();
            outputDevice.Init(volumeProvider);
            outputDevice.Play();
            await Task.Delay((int)audioFile.TotalTime.TotalMilliseconds);
            outputDevice.Stop();
        }

        private string GetMetronomeSamplePath(string sampleName)
        {
            var basePath = AppContext.BaseDirectory;
            var relativePath = Path.Combine(basePath, @"..\..\..\..\..\..\..\KeyChordFinder.Data\MetronomeSamples", $"{sampleName}.wav");
            return Path.GetFullPath(relativePath);
        }

        public static List<string?> GetMetronomeSampleNames()
        {
            var basePath = AppContext.BaseDirectory;
            var relativePath = Path.Combine(basePath, @"..\..\..\..\..\..\..\KeyChordFinder.Data\MetronomeSamples");
            var filePaths = Directory.GetFiles(relativePath);
            return filePaths.Select(Path.GetFileNameWithoutExtension).ToList();
        }
        public string GetPianoSamplePath(string note, int octave)
        {
            var basePath = AppContext.BaseDirectory;
            var relativePath = Path.Combine(basePath, @"..\..\..\..\..\..\..\KeyChordFinder.Data\PianoSamples", $"{note}{octave}.wav");
            return Path.GetFullPath(relativePath);
        }
    }
}
