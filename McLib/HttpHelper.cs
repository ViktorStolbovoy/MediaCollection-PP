using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace MediaCollection
{
	public static class HttpHelper
	{
		public static Task<byte[]> MakeHttpRequest(string url, CancellationToken cancellationToken)
		{
			System.Diagnostics.Debug.WriteLine(url);
			var client = new HttpClient();
			try
			{
				return client.GetAsync(url, cancellationToken).ContinueWith(t => {
					HttpResponseMessage res = null;
					try
					{
						res = t.Result;
						if (res.StatusCode == System.Net.HttpStatusCode.OK)
						{
							return res.Content.ReadAsByteArrayAsync().ContinueWith<byte[]>(t1 => {
								try
								{
									return t1.Result;
								}
								finally
								{
									res.Dispose();
									client.Dispose();
								}
							});
						}
						else
						{
							return res.Content.ReadAsByteArrayAsync().ContinueWith<byte[]>(t1 => {
								try
								{
									throw new HttpException(Encoding.UTF8.GetString(t1.Result), null, res.StatusCode);
								}
								catch (Exception err)
								{
									throw new HttpException(err.Message, err, res.StatusCode);
								}
								finally
								{
									res.Dispose();
									client.Dispose();
								}
							});
						}
					}
					catch
					{
						if (res != null) res.Dispose();
						client.Dispose();
						throw;
					}
				}).Unwrap();
			}
			catch
			{
				client.Dispose();
				throw;
			}

		}
	}

	public class HttpException : Exception
	{
		public System.Net.HttpStatusCode StatusCode;
		public HttpException(string message, Exception innerException, System.Net.HttpStatusCode statusCode) : base(message, innerException)
		{
			StatusCode = statusCode;
		}
	}
}
