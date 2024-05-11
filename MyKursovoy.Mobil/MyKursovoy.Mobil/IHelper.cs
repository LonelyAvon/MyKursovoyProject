using System;
using MyKursovoy.Domain.Models;

namespace MyKursovoy.Mobil
{
	public interface IHelper
	{
		public static string guid { get; set; }

		public static Client? Client { get; set; }

		public static HttpClient GetHttpClient()
		{
            var handler = new HttpClientHandler();

            // Set the ServerCertificateCustomValidationCallback property to ignore SSL certificate validation
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            HttpClient client = new HttpClient(handler);
			client.BaseAddress = new Uri("https://localhost:7208/");
			return client;
        }
	}
}

