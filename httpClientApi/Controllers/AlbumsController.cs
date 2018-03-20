using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace httpClientApi.Controllers
{
    [Route("api/[controller]")]
    public class AlbumsController : Controller
    {
        private readonly IJsonPlaceholderClient service;
        private readonly IMapper mapper;

        public AlbumsController(IJsonPlaceholderClient service, IMapper mapper){
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAlbums(){

            
           // service.GetAlbums3().Wait();

            var albums = service.LoadAlbums().Result;
            var response = this.mapper.Map<List<AlbumDto>, List<AlbumResponse>>(albums);

            return Ok(response);
        }

    }
}