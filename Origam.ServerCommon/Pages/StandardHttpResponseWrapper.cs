#region license
/*
Copyright 2005 - 2021 Advantage Solutions, s. r. o.

This file is part of ORIGAM (http://www.origam.org).

ORIGAM is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

ORIGAM is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with ORIGAM. If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;

namespace Origam.ServerCommon.Pages
{
    internal class StandardHttpResponseWrapper : IResponseWrapper
    {
        private readonly HttpContext httpContext;
        private readonly HttpResponse response;
        private readonly Encoding encoding;

        public StandardHttpResponseWrapper(HttpContext httpContext)
        {
            this.httpContext = httpContext;
            this.response = httpContext.Response;
            encoding = Encoding.UTF8;
        }

        public bool BufferOutput
        {
            set
            {
                if (!value)
                {
                    httpContext.Features.Get<IHttpBufferingFeature>()?.DisableResponseBuffering();
                }
            }
        }

        public string ContentType
        {
            set
            {
                var mediaType = new MediaTypeHeaderValue(value);
                mediaType.Encoding = encoding;
                response.ContentType = mediaType.ToString();
            }
        }


        public bool TrySkipIisCustomErrors
        {
            set { } // probably not necessary https://stackoverflow.com/questions/49269381/response-tryskipiiscustomerrors-equivalent-for-asp-net-core
        }

        public int StatusCode
        {
            set => response.StatusCode = value;
        }

        public string Charset
        {
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException();
        }

        public void WriteToOutput(Action<TextWriter> writeAction)
        {
            TextWriter textWriter = new StringWriter();
            writeAction(textWriter);
            response.WriteAsync(textWriter.ToString()).Wait();
        }

        public void CacheSetMaxAge(TimeSpan timeSpan)
        {
            response.Headers[HeaderNames.CacheControl] = "max-age=" + timeSpan.TotalSeconds;
        }

        public void End()
        {
        }

        public void Clear()
        {
            response.Clear();
        }

        public void Write(string message)
        {
            response.WriteAsync(message).Wait();
        }

        public void AddHeader(string name, string value)
        {
            response.Headers[name]= value;
        }

        public void BinaryWrite(byte[] bytes)
        {
            response.Body.WriteAsync(bytes,0, bytes.Length).Wait();
        }

        public void Redirect(string requestUrlReferrerAbsolutePath)
        {
            response.Redirect(requestUrlReferrerAbsolutePath);
        }

        public void OutputStreamWrite(byte[] buffer, int offset, int count)
        {
            response.Body.WriteAsync(buffer, offset, count).Wait();
        }

        public void AppendHeader(string contentDisposition, string disposition)
        {
            response.Headers[contentDisposition] = disposition;
        }
    }
}