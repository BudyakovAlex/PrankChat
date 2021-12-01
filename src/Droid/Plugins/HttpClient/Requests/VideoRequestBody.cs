using System;
using PrankChat.Mobile.Droid.Plugins.HttpClient.Builders;
using Square.OkHttp3;
using Square.OkIO;

namespace PrankChat.Mobile.Droid.Plugins.HttpClient.Requests
{
    public class VideoRequestBody : RequestBody
    {
        private readonly Action<double, double> _onChangedProgressAction;
        private readonly FormDataBuilder _formData;
        private readonly string _contentType;

        public VideoRequestBody(
            Action<double, double> onChangedProgressAction,
            FormDataBuilder formData,
            string contentType)
        {
            _onChangedProgressAction = onChangedProgressAction;
            _formData = formData;
            _contentType = contentType;
        }

        public override MediaType ContentType()
        {
            return MediaType.Parse(_contentType);
        }

        public override void WriteTo(IBufferedSink sink)
        {
            _formData.WriteTo(sink, _onChangedProgressAction);
        }
    }
}
