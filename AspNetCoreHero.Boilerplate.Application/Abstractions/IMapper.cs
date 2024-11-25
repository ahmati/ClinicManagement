using AutoMapper;

namespace CardoAI.Core.PFM.Application.Abstractions
{
    public interface IMapper<TSource>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(TSource), GetType());
    }

    public interface IReverseMapper<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType()).ReverseMap();
    }
}
