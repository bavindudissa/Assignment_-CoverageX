using System;
using AutoMapper;
using to_do_api.Dtos;

namespace to_do_api.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Models.Task, TaskDto>();

        CreateMap<TaskDto, Models.Task>();

    }
}
