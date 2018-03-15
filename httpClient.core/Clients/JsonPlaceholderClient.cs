using System.Collections.Generic;

using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using Microsoft.Extensions.Logging;

public class JsonPlaceholderClient : IJsonPlaceholderClient
{
    private readonly ILogger<JsonPlaceholderClient> logger;

    public JsonPlaceholderClient(ILogger<JsonPlaceholderClient> logger)
    {
        this.logger = logger;
    }

    public async Task GetAlbums(){
       HttpClient client = new HttpClient();

        //client.DefaultRequestHeaders.Accept.Clear();
        //client.DefaultRequestHeaders.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        var albumsTask = client.GetStringAsync("https://jsonplaceholder.typicode.com/albums");

        var albumsString = await albumsTask;

        Console.WriteLine(albumsString);
        logger.LogDebug(albumsString);
        
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
    List<AlbumDto> LoadAlbums();
}

 