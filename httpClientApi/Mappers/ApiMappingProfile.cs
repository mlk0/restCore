using AutoMapper;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile(){
        CreateMap<AlbumDto, AlbumResponse>();
        CreateMap<PostDto, PostResponse>();

        CreateMap<PostRequest, PostDto>()
        .ForMember(d=>d.id, opt=>opt.Ignore());
    }
}