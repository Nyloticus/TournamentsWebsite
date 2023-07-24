using Mapster;

namespace Application.UserImage.Dto
{
    public class UserImageDto : IRegister
    {
        public string url { get; set; }
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Auth.UserImages, UserImageDto>()
                .Map(des => des.url, src => src.Url);

        }
    }
}
