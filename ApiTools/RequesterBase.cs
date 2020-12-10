using Newtonsoft.Json;
using System;

using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTools.Requester
{
    public abstract class RequesterBase : IDisposable
    {
            protected readonly HttpClient _Client;

        protected RequesterBase( string BaseAdress)
            {
                _Client = new HttpClient();
                _Client.BaseAddress =new Uri(BaseAdress);
            }

        protected async Task<TResult> GetAsync<TResult>(string uri, CancellationToken token = default)
            {
                using (HttpResponseMessage message = await _Client.GetAsync(uri, token))
                {
                    if (!message.IsSuccessStatusCode) { throw new HttpRequestException(); }
                    string json = await message.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResult>(json);
                }
            }

        protected async Task<TResult> PostAsync<TResult, TBody>(string uri, TBody body, CancellationToken token = default)
            {
                string jsonBody = JsonConvert.SerializeObject(body);
                using (HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
                {
                    using (HttpResponseMessage message = await _Client.PostAsync(uri, content, token))
                    {
                        if (!message.IsSuccessStatusCode) { throw new HttpRequestException(); }
                        string json = await message.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TResult>(json);
                    }
                }
            }

        protected async Task<TResult> PutAsync<TResult, TBody>(string uri, TBody body, CancellationToken token = default)
            {
                string jsonBody = JsonConvert.SerializeObject(body);
                using (HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
                {
                    using (HttpResponseMessage message = await _Client.PutAsync(uri, content, token))
                    {
                        if (!message.IsSuccessStatusCode) { throw new HttpRequestException(); }
                        string json = await message.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TResult>(json);
                    }
                }
            }

        protected async Task<TResult> DeleteAsync<TResult>(string uri, CancellationToken token = default)
            {
                using (HttpResponseMessage message = await _Client.DeleteAsync(uri, token))
                {
                    if (!message.IsSuccessStatusCode) { throw new HttpRequestException(); }
                    string json = await message.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResult>(json);
                }
            }

            private void AddHeaders(string title, string content)
            {
                _Client.DefaultRequestHeaders.Add(title, content);
            }

            private bool _Disposed = false;

            protected virtual void Dispose(bool disposing)
            {
                if (_Disposed) return;
                if (disposing)
                {
                    _Client.Dispose();
                }
                _Disposed = true;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }

 
