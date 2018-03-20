using AutoMapper;
public class CoreMappingProfile : Profile{
    public CoreMappingProfile(){
        CreateMap<AlbumClientResponse, AlbumDto>();
        CreateMap<PostClientResponse, PostDto>();
    }
}