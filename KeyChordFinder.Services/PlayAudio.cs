using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace KeyChordFinder.Services
{
    public class PlayAudio
    {
        private string _audioFile = "C:\\MRLN\\fl studio - kity\\! Výběr\\HH\\Southside Hi Hat.wav"; // Ensure this path points to a valid audio file
        private CancellationTokenSource _cancellationTokenSource;
        private int _bpm = 120;
        private bool _isPlaying = false;
        private double _volume = 1.0;

        public async Task PlayMetronomeAsync(CancellationToken cancellationToken)
        {
            using (var audioFile = new AudioFileReader(_audioFile))
            {
                var volumeProvider = new VolumeSampleProvider(audioFile.ToSampleProvider())
                {
                    Volume = (float)_volume
                };

                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(volumeProvider);

                    Console.WriteLine("Playing audio...");

                    int interval = 60000 / _bpm; // Délka jednoho cyklu v ms
                    long nextTick = Environment.TickCount; // Absolutní čas dalšího cyklu

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        // Nastav start přehrávání
                        long startTick = Environment.TickCount;

                        // Resetuj pozici a přehraj vzorek
                        audioFile.Position = 0;
                        outputDevice.Play();

                        // Počkej na dokončení nebo zkrácení podle intervalu
                        int trackLengthMs = (int)audioFile.TotalTime.TotalMilliseconds;
                        await Task.Delay(Math.Min(trackLengthMs, interval), cancellationToken);

                        outputDevice.Stop(); // Stopni přehrávač (pro jistotu)

                        // Spočítej čas, který zbývá do příštího cyklu
                        nextTick += interval;
                        int sleepTime = (int)(nextTick - Environment.TickCount);

                        // Pokud zbývá nějaký čas, počkej; jinak přejdi k další iteraci
                        if (sleepTime > 0)
                        {
                            await Task.Delay(sleepTime, cancellationToken);
                        }
                        else
                        {
                            Console.WriteLine("Missed tick, adjusting...");
                        }
                    }
                }
            }
        }
        public void PlayMetronome(double volume)
        {
            _volume = volume;
            _cancellationTokenSource = new CancellationTokenSource();
            _isPlaying = true;
            Task.Run(() => PlayMetronomeAsync(_cancellationTokenSource.Token));
        }
        public void StopMetronome()
        {
            _cancellationTokenSource?.Cancel();
            _isPlaying = false;
        }


        public void UpdateBpm(int bpm)
        {
            _bpm = bpm;
            if (_isPlaying)
            {
                Restart();
            }
        }
        public void UpdateSample(string audioFile)
        {
            _audioFile = audioFile;
            if (_isPlaying)
            {
                Restart();
            }
        }
        public void UpdateVolume(double volume)
        {
            _volume = volume;
            if (_isPlaying)
            {
                Restart();
            }
        }
        private void Restart()
        {
            StopMetronome();
            PlayMetronome(_volume); // Restart with the current volume
        }


        public async Task PlaySample(string path)
        {
            using (var audioFile = new AudioFileReader(path))
            {
                var volumeProvider = new VolumeSampleProvider(audioFile.ToSampleProvider())
                {
                    Volume = (float)_volume
                };
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(volumeProvider);
                    outputDevice.Play();
                    await Task.Delay((int)audioFile.TotalTime.TotalMilliseconds);
                    outputDevice.Stop();
                }
            }
        }
    }
}
