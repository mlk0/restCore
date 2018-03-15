using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace httpClientApi.Controllers
{
    [Route("api/[controller]")]
    public class AlbumsController : Controller
    {
        private readonly IJsonPlaceholderClient service;

        public AlbumsController(IJsonPlaceholderClient service){
            this.service = service;
        }

        [HttpGet]
        public IActionResult GetAlbums(){

            AlbumResponse response = new AlbumResponse();
            service.GetAlbums().Wait();
            return Ok();
        }

    }
}