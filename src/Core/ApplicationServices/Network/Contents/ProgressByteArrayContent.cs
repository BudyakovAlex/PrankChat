using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Contents
{
    public class ProgressByteArrayContent : ByteArrayContent
    {
        private const int ChunkSize = 4096;

        private readonly byte[] _bytes;
        private readonly Action<double> _onUploadChanged;
        private readonly CancellationToken _cancellationToken;

        public ProgressByteArrayContent(byte[] content, Action<double> onUploadChanged, CancellationToken cancellationToken = default) : base(content)
        {
            _onUploadChanged = onUploadChanged;
            _cancellationToken = cancellationToken;
            _bytes = content;
        }

        public ProgressByteArrayContent(byte[] content, int offset, int count, Action<double> onUploadChanged, CancellationToken cancellationToken = default) : base(content, offset, count)
        {
            _onUploadChanged = onUploadChanged;
            _cancellationToken = cancellationToken;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            for (int i = 0; i < _bytes.Length; i += ChunkSize)
            {
                await stream.WriteAsync(_bytes, i, Math.Min(ChunkSize, _bytes.Length - i), _cancellationToken);
                _onUploadChanged?.Invoke(100.0 * i / _bytes.Length);
            }

            stream.Flush();
        }
    }
}
