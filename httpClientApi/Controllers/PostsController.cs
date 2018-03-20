using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace httpClientApi.Controllers
{
    [Route("api/pst")]
    public class PostsController : Controller{
        private readonly IMapper mapper;
        private readonly IJsonPlaceholderClient client;

        public PostsController(IMapper mapper, IJsonPlaceholderClient client){
            this.mapper = mapper;
            this.client = client;
        }

 
        [HttpGet]
        public IActionResult GetPosts(){
            var posts =  this.client.LoadPosts().Result;
            var postResponse = this.mapper.Map<List<PostDto>, List<PostResponse>>(posts);
            return Ok(postResponse);

        }
    }
}