using Serilog.Core;
using Serilog.Events;
using System.Text;
using System.Threading;

namespace BlazorHybrid.Logging.Sinks;

internal sealed class AppdataFileSink : ILogEventSink
{
    private readonly IFileSystem _fileSystem;
    private readonly Lock _sync = new();
    private readonly StringBuilder _buffer = new();

    private FileStream? _stream;
    private StreamWriter? _writer;
    private string _filePath = string.Empty;

    public AppdataFileSink(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        AppDomain.CurrentDomain.ProcessExit += Flush;
    }

    public AppdataFileSink Init()
    {
        try
        {
            _filePath = Path.Join(_fileSystem.AppDataDirectory, "logs", $"{DateTime.Now:yyyyMMdd}.log");
            string? dirName = Path.GetDirectoryName(_filePath);
            ArgumentException.ThrowIfNullOrWhiteSpace(dirName);
            Directory.CreateDirectory(dirName);

            _stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(_stream, Encoding.UTF8)
            {
                AutoFlush = false
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to initialize file sink: {ex.Message}");
        }

        return this;
    }

    public void Emit(LogEvent logEvent)
    {
        string message = $"[{logEvent.Timestamp:O} {logEvent.Level}] {logEvent.RenderMessage()}";

        lock (_sync)
        {
            _buffer.AppendLine(message);
        }

        if (logEvent.Level >= LogEventLevel.Error)
        {
            Flush();
        }
    }

    private void Flush(object? _, EventArgs _2) => Flush();
    private void Flush()
    {
        lock (_sync)
        {
            if (_buffer.Length == 0 || _writer is null)
            {
                return;
            }

            _writer.Write(_buffer.ToString());
            _writer.Flush();
            _buffer.Clear();
        }
    }
}
