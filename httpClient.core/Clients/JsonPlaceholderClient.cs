using System.Collections.Generic;

using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using Microsoft.Extensions.Logging;
using System.Runtime.Serialization.Json;
using System.Net;
using AutoMapper;

public class JsonPlaceholderClient : IJsonPlaceholderClient
{
    private readonly ILogger<JsonPlaceholderClient> logger;
    private readonly IMapper mapper;

    public JsonPlaceholderClient(ILogger<JsonPlaceholderClient> logger, IMapper mapper)
    {
        this.logger = logger;
        this.mapper = mapper;
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

    public async Task<List<AlbumDto>> LoadAlbums()
    {
        List<AlbumDto> result = new List<AlbumDto>();

        var client = new HttpClient();
        var httpResponseMessageTask =  client.GetAsync("https://jsonplaceholder.typicode.com/albums");
        var httpResponseMessage = await httpResponseMessageTask;
        if(httpResponseMessage.StatusCode == HttpStatusCode.OK){
            var contentStream = httpResponseMessage.Content.ReadAsStreamAsync();
            var serializer = new DataContractJsonSerializer(typeof(List<AlbumClientResponse>));
            var clientResponse = serializer.ReadObject(await contentStream) as List<AlbumClientResponse>;
            result = this.mapper.Map<List<AlbumClientResponse>, List<AlbumDto>>(clientResponse);
        }
  
        return result;
    }

    public async Task<List<PostDto>> LoadPosts()
    {
        List<PostDto> posts = new List<PostDto>();
        var client = new HttpClient();
        var httpResponseMessage = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");
        if(httpResponseMessage.StatusCode == HttpStatusCode.OK){
            var contentStream = httpResponseMessage.Content.ReadAsStreamAsync();
            var serializer = new DataContractJsonSerializer(typeof(List<PostClientResponse>));
            var postClientResponseList = serializer.ReadObject(await contentStream) as List<PostClientResponse>;
            posts = this.mapper.Map<List<PostClientResponse>, List<PostDto>>(postClientResponseList);
        }

        return posts;
    }
}

public interface IJsonPlaceholderClient
{
     Task GetAlbums();
     Task GetAlbums2();

     Task GetAlbums3();
    Task<List<AlbumDto>> LoadAlbums();
    Task<List<PostDto>> LoadPosts();
}

 