using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Contents
{
    public class ProgressByteArrayContent : HttpContent
    {
        private const int DefaultBufferSize = 4096;
        private readonly Action<double> _onProgressChanged;
        private readonly CancellationToken _cancellationToken;
        private byte[] _content;

        private int _bufferSize;

        public ProgressByteArrayContent(byte[] content, Action<double> onProgressChanged, CancellationToken cancellationToken = default) : this(content, DefaultBufferSize, onProgressChanged, cancellationToken)
        {
        }

        public ProgressByteArrayContent(byte[] content, int bufferSize, Action<double> onProgressChanged, CancellationToken cancellationToken = default)
        {
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            _content = content ?? throw new ArgumentNullException(nameof(content));
            _content = content;
            _bufferSize = bufferSize;
            _onProgressChanged = onProgressChanged;
            _cancellationToken = cancellationToken;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Contract.Assert(stream != null);

            return Task.Run(() =>
            {
                var size = _content.LongLength;

                var uploaded = 0;
                while (true)
                {
                    if (_cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var buffer = _content.Skip(uploaded).Take(_bufferSize).ToArray();
                    if (buffer.Length <= 0)
                    {
                        break;
                    }

                    uploaded += buffer.Length;
                    _onProgressChanged?.Invoke(uploaded / (double)size * 100);
                    stream.Write(buffer, 0, buffer.Length);
                }
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _content.LongLength;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _content = null;
            }

            base.Dispose(disposing);
        }
    }
}
