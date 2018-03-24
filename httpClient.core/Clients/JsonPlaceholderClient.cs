using System.Collections.Generic;

using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using Microsoft.Extensions.Logging;
using System.Runtime.Serialization.Json;
using System.Net;
using AutoMapper;
using Newtonsoft.Json;
using System.Text;
using httpClient.core.Clients;

public class JsonPlaceholderClient : IJsonPlaceholderClient
{
    private readonly ILogger<JsonPlaceholderClient> logger;
    private readonly IMapper mapper;
    private readonly IProxiedHttpClient _httpClient;
    

    public JsonPlaceholderClient(ILogger<JsonPlaceholderClient> logger, IMapper mapper, IProxiedHttpClient httpClient)
    {
        this.logger = logger;
        this.mapper = mapper;
        this._httpClient = httpClient;
    }

    public async Task GetAlbums(){
       

        var serializer = new DataContractJsonSerializer(typeof(List<AlbumDto>));    //using System.Runtime.Serialization.Json;

        //client.DefaultRequestHeaders.Accept.Clear();
        //client.DefaultRequestHeaders.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        var albumsTask = this._httpClient.Instance.GetStringAsync("https://jsonplaceholder.typicode.com/albums");

        var albumsString = await albumsTask;

        Console.WriteLine(albumsString);
        logger.LogDebug(albumsString);
        
    }

    public async Task GetAlbums2()
    {
         
        var serializer = new DataContractJsonSerializer(typeof(List<AlbumDto>));
        var streamTask = this._httpClient.Instance.GetStreamAsync("https://jsonplaceholder.typicode.com/albums");
        var albums = serializer.ReadObject(await streamTask) as List<AlbumDto>;
        albums.ForEach(a=>Console.WriteLine( $"{a.id} | {a.title} | {a.userId}" ));
    }



    public async Task GetAlbums3()
    {
      
        var httpResponseMessageTask = this._httpClient.Instance.GetAsync("https://jsonplaceholder.typicode.com/albums");
        var httpResponseMessage = await httpResponseMessageTask;
        if(httpResponseMessage.StatusCode == HttpStatusCode.OK){
            var contentStream = httpResponseMessage.Content.ReadAsStreamAsync();

            var serializer = new DataContractJsonSerializer(typeof(List<AlbumDto>));
            var contentObject = serializer.ReadObject(await contentStream);
            var content = contentObject as List<AlbumDto>;
            content.ForEach(a=>Console.WriteLine( $"{a.id} | {a.title} | {a.userId}" ));


            
        }
    }


    

    public async Task<List<AlbumDto>> LoadAlbumsY(){

        List<AlbumDto> result = new List<AlbumDto>();
       
        var httpResponseMessageTask =  this._httpClient.Instance.GetAsync("https://jsonplaceholder.typicode.com/albums");
        var httpResponseMessage = await httpResponseMessageTask;
        logger.LogInformation($"httpResponseMessage.StatusCode : {httpResponseMessage.StatusCode}");

        if(httpResponseMessage.StatusCode == HttpStatusCode.OK){
            var contentStream = httpResponseMessage.Content.ReadAsStreamAsync();
            var serializer = new DataContractJsonSerializer(typeof(List<AlbumClientResponse>));
            var clientResponse = serializer.ReadObject(await contentStream) as List<AlbumClientResponse>;
            result = this.mapper.Map<List<AlbumClientResponse>, List<AlbumDto>>(clientResponse);
        }
        else{
                var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
                logger.LogInformation(contentString);
        }
  
        return result;
    }

    public async Task<List<AlbumDto>> LoadAlbums()
    {

        List<AlbumDto> result = new List<AlbumDto>();
        
 


        var httpResponseMessageTask =  this._httpClient.Instance.GetAsync("https://jsonplaceholder.typicode.com/albums");
        var httpResponseMessage = await httpResponseMessageTask;
        logger.LogInformation($"httpResponseMessage.StatusCode : {httpResponseMessage.StatusCode}");

        if(httpResponseMessage.StatusCode == HttpStatusCode.OK){
            var contentStream = httpResponseMessage.Content.ReadAsStreamAsync();
            var serializer = new DataContractJsonSerializer(typeof(List<AlbumClientResponse>));
            var clientResponse = serializer.ReadObject(await contentStream) as List<AlbumClientResponse>;
            result = this.mapper.Map<List<AlbumClientResponse>, List<AlbumDto>>(clientResponse);
        }
        else{
                var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
                logger.LogInformation(contentString);
        }
  
        return result;
    }
 

    public async Task<PostDto> LoadPost(int postId)
    {
       
       
        var postResponseJson = await this._httpClient.Instance.GetStringAsync( $"https://jsonplaceholder.typicode.com/posts/{postId}");
        var postResponse = JsonConvert.DeserializeObject<PostClientResponse>(postResponseJson);
        var postDto = this.mapper.Map<PostClientResponse, PostDto>(postResponse);
        return postDto;
    }

    public async Task<List<PostDto>> LoadPosts()
    {
        List<PostDto> posts = new List<PostDto>();
        var client = new HttpClient();
        var httpResponseMessage = await this._httpClient.Instance.GetAsync("https://jsonplaceholder.typicode.com/posts");
        if(httpResponseMessage.StatusCode == HttpStatusCode.OK){
            var contentStream = httpResponseMessage.Content.ReadAsStreamAsync();
            var serializer = new DataContractJsonSerializer(typeof(List<PostClientResponse>));
            var postClientResponseList = serializer.ReadObject(await contentStream) as List<PostClientResponse>;
            posts = this.mapper.Map<List<PostClientResponse>, List<PostDto>>(postClientResponseList);
        }

        return posts;
    }





    public async Task<List<PostDto>> LoadPostsX(){

       


        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Get;
        request.RequestUri = new Uri("https://jsonplaceholder.typicode.com/posts");
         
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "rtyguhjhgtedrtfyguhihygt45678hgftrd5r6ftg7yhuihygtf");

        this._httpClient.Instance.Timeout = new TimeSpan(0,0,3);

     
        var response = await this._httpClient.Instance.SendAsync(request);

        List<PostDto> result = new List<PostDto>();
        if(response.StatusCode == HttpStatusCode.OK){
            var responseAsJson = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<List<PostClientResponse>>(responseAsJson);
            var postDtoList = this.mapper.Map<List<PostClientResponse>, List<PostDto>>(posts);
            result = postDtoList;
        }

        return result;
    }







    public async Task<PostDto> SavePost(PostDto post)
    {
        PostDto result = null;
        var jsonContent = new StringContent(JsonConvert.SerializeObject(post), encoding: Encoding.UTF8, mediaType: "application/json");
        this.logger.LogInformation(jsonContent.ToString());

      
        HttpResponseMessage postResult = await this._httpClient.Instance.PostAsync("https://jsonplaceholder.typicode.com/posts", jsonContent);
        if (postResult.StatusCode == HttpStatusCode.Created)
        {
            this.logger.LogError("Post succeeded");
            var contentAsString = await postResult.Content.ReadAsStringAsync();
            if (!String.IsNullOrEmpty(contentAsString))
            {
                var postClientResponse = JsonConvert.DeserializeObject<PostClientResponse>(contentAsString);
                if (postClientResponse != null)
                {
                    result = this.mapper.Map<PostClientResponse, PostDto>(postClientResponse);
                }
            }

        }


        return result;
    }


    public async Task<PostDto> SavePost2(PostDto post){

        PostDto result = null;
        var request = new HttpRequestMessage(HttpMethod.Post, "https://jsonplaceholder.typicode.com/posts"){
            Content = new StringContent(JsonConvert.SerializeObject(post))
        };
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "asdasdasd");
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        var httpResponseMessage = await this._httpClient.Instance.SendAsync(request);
        if(httpResponseMessage.StatusCode == HttpStatusCode.Created){

            var jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var postClientRespone = JsonConvert.DeserializeObject<PostClientResponse>(jsonContent);
             result = this.mapper.Map<PostClientResponse, PostDto>(postClientRespone);
        }


        return result;
    }



}

public interface IJsonPlaceholderClient
{
     Task GetAlbums();
     Task GetAlbums2();

     Task GetAlbums3();
    Task<List<AlbumDto>> LoadAlbums();
    Task<List<AlbumDto>> LoadAlbumsY();
    
    Task<List<PostDto>> LoadPosts();
    Task<PostDto> LoadPost(int postId);
    

    Task<PostDto> SavePost(PostDto post);
    Task<PostDto> SavePost2(PostDto post);
    

    Task<List<PostDto>> LoadPostsX();
}

 
 