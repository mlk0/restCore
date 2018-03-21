using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace httpClientApi.Controllers
{
    [Route("api/posts")]
    public class PostsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IJsonPlaceholderClient client;

        public PostsController(IMapper mapper, IJsonPlaceholderClient client)
        {
            this.mapper = mapper;
            this.client = client;
        }


        [HttpGet]
        public IActionResult GetPosts()
        {
            var posts = this.client.LoadPosts().Result;
            var postResponse = this.mapper.Map<List<PostDto>, List<PostResponse>>(posts);
            return Ok(postResponse);

        }

        [HttpGet("all")]
        public IActionResult GetPosts2()
        {
            var posts = this.client.LoadPostsX().Result;
            var postResponse = this.mapper.Map<List<PostDto>, List<PostResponse>>(posts);
            return Ok(postResponse);

        }



        [HttpGet("{postId}", Name = "GetPostByPostId")]
        public IActionResult GetPost(int postId)
        {
            var post = this.client.LoadPost(postId).Result;            
            var postResponse = this.mapper.Map<PostDto, PostResponse>(post);
            return Ok(postResponse);

        }

//        [HttpPost]
        public IActionResult PostPost([FromBody] PostRequest newPost)
        {
            var postDto = this.mapper.Map<PostRequest, PostDto>(newPost);
            var result = this.client.SavePost(postDto).Result;
            var postResponse = this.mapper.Map<PostDto, PostResponse>(result);

            //return the 201 with the location header to the route that is associted with the name cityPointOfInterest
            return CreatedAtRoute("GetPostByPostId", new { postId = postResponse.id }, postResponse);

        }




        [HttpPost]
        public IActionResult PostPost2([FromBody] PostRequest newPost)
        {
            var postDto = this.mapper.Map<PostRequest, PostDto>(newPost);
            var result = this.client.SavePost2(postDto).Result;
            var postResponse = this.mapper.Map<PostDto, PostResponse>(result);

            //return the 201 with the location header to the route that is associted with the name cityPointOfInterest
            return CreatedAtRoute("GetPostByPostId", new { postId = postResponse.id }, postResponse);

        }

        

    }
}