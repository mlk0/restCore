using AutoMapper;

public class ApiMappingProfile : Profile{
    public ApiMappingProfile(){
        CreateMap<AlbumDto, AlbumResponse>();
        CreateMap<PostDto, PostResponse>();
    }
}