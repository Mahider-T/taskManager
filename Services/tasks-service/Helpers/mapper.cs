using AutoMapper;
using TaskManager.DTOs;
namespace TaskManager.Helpers;

public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<updateTaskDTO, Task>();
    }
}