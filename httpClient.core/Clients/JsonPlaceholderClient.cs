using System.Collections.Generic;

using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using Microsoft.Extensions.Logging;
using System.Runtime.Serialization.Json;
using System.Net;

public class JsonPlaceholderClient : IJsonPlaceholderClient
{
    private readonly ILogger<JsonPlaceholderClient> logger;

    public JsonPlaceholderClient(ILogger<JsonPlaceholderClient> logger)
    {
        this.logger = logger;
    }

    public async Task GetAlbums(){
       HttpClient client = new HttpClient();

        var serializer = new DataContractJsonSerializer(typeof(List<AlbumDto>));    //using System.Runtime.Serialization.Json;

        //client.DefaultRequestHeaders.Accept.Clear();
        //client.DefaultRequestHeaders.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        var albumsTask = client.GetStringAsync("https://jsonplaceholder.typicode.com/albums");

        var albumsString = await albumsTask;

        Console.WriteLine(albumsString);
        logger.LogDebug(albumsString);
        
    }

    public async Task GetAlbums2()
    {
        var client = new HttpClient();
        var serializer = new DataContractJsonSerializer(typeof(List<AlbumDto>));
        var streamTask = client.GetStreamAsync("https://jsonplaceholder.typicode.com/albums");
        var albums = serializer.ReadObject(await streamTask) as List<AlbumDto>;
        albums.ForEach(a=>Console.WriteLine( $"{a.id} | {a.title} | {a.userId}" ));
    }



    public async Task GetAlbums3()
    {
        var httpClient = new HttpClient();
        var httpResponseMessageTask = httpClient.GetAsync("https://jsonplaceholder.typicode.com/albums");
        var httpResponseMessage = await httpResponseMessageTask;
        if(httpResponseMessage.StatusCode == HttpStatusCode.OK){
            var contentStream = httpResponseMessage.Content.ReadAsStreamAsync();

            var serializer = new DataContractJsonSerializer(typeof(List<AlbumDto>));
            var contentObject = serializer.ReadObject(await contentStream);
            var content = contentObject as List<AlbumDto>;
            content.ForEach(a=>Console.WriteLine( $"{a.id} | {a.title} | {a.userId}" ));


            // httpResponseMessage.Headers.GetValues
        }
    }

    public List<AlbumDto> LoadAlbums()
    {
        List<AlbumDto> result = new List<AlbumDto>();

        //1. call the external service

        //2. map the response to dto

        return result;
    }
}

public interface IJsonPlaceholderClient
{
     Task GetAlbums();
     Task GetAlbums2();

     Task GetAlbums3();
    List<AlbumDto> LoadAlbums();
}

 