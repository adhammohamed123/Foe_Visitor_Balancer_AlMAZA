
using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Service.DTOs.BlackListDtos;
using Service.DTOs.CardDtos;
using Service.DTOs.DepartmentDtos;
using Service.DTOs.FloorDtos;
using Service.DTOs.UserDtos;
using Service.DTOs.Visit;
using Service.DTOs.Visitor;
using System.Data;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Card Model
        CreateMap<CardForCreationDto, Card>();
        CreateMap<Card, CardForReturnDto>();
        #endregion

        #region Department Model
        CreateMap<DepartmentForCreationDto, Department>();
		CreateMap<Department, DepartmentForReturnDto>().ReverseMap();
		#endregion


		#region User Model
		CreateMap<User, UserForReturnDto>().ForMember(dto=>dto.DepartmentName,opt=>opt.MapFrom(u=>u.Department.Name));
        CreateMap<User, UserDto>();
        CreateMap<UserForRegistrationDto, User>();
        #endregion

        #region Floor Model
        CreateMap<FloorForCreationDto, Floor>();
        CreateMap<Floor, FloorForReturnDto>().ReverseMap();
        #endregion


        #region Visit Model
        CreateMap<VisitForCreationDto, Visit>();
        CreateMap<VisitForCreationFromSecertaryDto, Visit>();
        CreateMap<Visit, VisitForReturnDto>().ForMember(v=>v.DeptName,opt=>opt.MapFrom(s=>s.CreatedUser.Department.Name)) ;
        CreateMap<VisitStatusChangeFromPoliceDto, Visit>();
        CreateMap<VisitStatusChangeFromDeptDto, Visit>();
        #endregion

        #region Visitor Model
        CreateMap<VisitorForCreationDto, Core.Entities.Visitor>();
        CreateMap<Core.Entities.Visitor, VisitorForReturnDto>().ForMember(v=>v.CardNumber,opt=>opt.MapFrom(v => v.Card.CardNumber));
        //  CreateMap<VisitorForUpdatingEntryLeaveTimeDto, Core.Entities.Visitor>();
        #endregion

        CreateMap<VisitorBlockedForCreationDto, VisitorBlackList>();
    
        CreateMap<VisitorBlackList, VisitorBlockedDto>();
           

    }
}