//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using AutoMapper;
using MateSharp.Framework.Dtos;
using MateSharp.Framework.Extensions;
using MateSharp.RoleClaim.Domain.Dtos;
using MateSharp.RoleClaim.Domain.Entities;
using Meti.Application.Dtos.Alarm;
using Meti.Application.Dtos.AlarmFired;
using Meti.Application.Dtos.AlarmMetric;
using Meti.Application.Dtos.Device;
using Meti.Application.Dtos.File;
using Meti.Application.Dtos.Parameter;
using Meti.Application.Dtos.Process;
using Meti.Application.Dtos.ProcessInstance;
using Meti.Application.Dtos.ProcessMacro;
using Meti.Application.Dtos.Registry;
using Meti.Application.Dtos.User;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;

namespace Meti.Infrastructure.Configurations
{
    public class AutomapperConfig
    {
        public static void InitMapper()
        {
            #region HealthRisk
            Mapper.CreateMap<HealthRisk, HealthRiskEditDto>();
            Mapper.CreateMap<HealthRisk, HealthRiskDetailDto>();
            Mapper.CreateMap<HealthRisk, HealthRiskIndexDto>();
            
            Mapper.CreateMap<HealthRiskLevel?, ItemDto<HealthRiskLevel?>>()
                .ConvertUsing(src => new ItemDto<HealthRiskLevel?>
                {
                    Id = src,
                    Text = src.ToString(),
                    Description = src.GetDescription()
                });
            Mapper.CreateMap<HealthRiskType?, ItemDto<HealthRiskType?>>()
                .ConvertUsing(src => new ItemDto<HealthRiskType?>
                {
                    Id = src,
                    Text = src.ToString(),
                    Description = src.GetDescription()
                });
            #endregion 
            #region Registry

            Mapper.CreateMap<Registry, RegistryIndexDto>();//.IgnoreAllNonExisting();
            Mapper.CreateMap<Registry, RegistryDetailDto>();
            Mapper.CreateMap<Registry, RegistryEditDto>();
            Mapper.CreateMap<File, FileDto>();
           

            Mapper.CreateMap<SexType?, ItemDto<SexType?>>()
                .ConvertUsing(src => new ItemDto<SexType?>
                {
                    Id = src,
                    Text = src.ToString(),
                    Description = src.GetDescription()
                });

            Mapper.CreateMap<RegistryType?, ItemDto<RegistryType?>>()
                .ConvertUsing(src => new ItemDto<RegistryType?>
                {
                    Id = src,
                    Text = src.ToString(),
                    Description = src.GetDescription()
                });

            Mapper.CreateMap<LifeStyle?, ItemDto<LifeStyle?>>()
              .ConvertUsing(src => new ItemDto<LifeStyle?>
              {
                  Id = src,
                  Text = src.ToString(),
                  Description = src.GetDescription()
              });

            #endregion Registry

            #region Process

            Mapper.CreateMap<Process, ProcessIndexDto>();
            Mapper.CreateMap<Process, ProcessDetailDto>();
            Mapper.CreateMap<Process, ProcessEditDto>();

            #endregion Process

            #region ProcessInstace

            Mapper.CreateMap<ProcessInstance, ProcessInstanceIndexDto>()
                //.ForMember(dest => dest.DoctorFirstname, opt => opt.MapFrom(src => src.Doctor.Firstname)).IgnoreAllPropertiesWithAnInaccessibleSetter()
                //.ForMember(dest => dest.DoctorSurname, opt => opt.MapFrom(src => src.Doctor.Surname)).IgnoreAllPropertiesWithAnInaccessibleSetter()
                //.ForMember(dest => dest.PatientFirstname, opt => opt.MapFrom(src => src.Patient.Firstname)).IgnoreAllPropertiesWithAnInaccessibleSetter()
                //.ForMember(dest => dest.PatientSurname, opt => opt.MapFrom(src => src.Patient.Surname)).IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(dest => dest.ProcessName, opt => opt.MapFrom(src => src.Process.Name)).IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<ProcessInstance, ProcessInstanceEditDto>();
            Mapper.CreateMap<ProcessInstance, ProcessInstanceDetailDto>();
            Mapper.CreateMap<ProcessInstance, GeolocationDto>()
                .ForMember(dest => dest.ProcessInstanceId, opt => opt.MapFrom(src => src.Id)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<ProcessInstance, GeolocationSwiftDto>()
                .ForMember(dest => dest.ProcessInstanceId, opt => opt.MapFrom(src => src.Id)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<ProcessInstanceState?, ItemDto<ProcessInstanceState?>>()
                  .ConvertUsing(src => new ItemDto<ProcessInstanceState?>
                  {
                      Id = src,
                      Text = src.ToString(),
                      Description = src.GetDescription()
                  });

            //.ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Patient.Surname)).IgnoreAllPropertiesWithAnInaccessibleSetter()
            //.ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Patient.Firstname)).IgnoreAllPropertiesWithAnInaccessibleSetter()
            //.ForMember(dest => dest.Lng, opt => opt.MapFrom(src => src.Patient.Longitude)).IgnoreAllPropertiesWithAnInaccessibleSetter()
            //.ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Patient.Latitude)).IgnoreAllPropertiesWithAnInaccessibleSetter();

            #endregion ProcessInstace

            #region Device

            Mapper.CreateMap<Device, DeviceDetailDto>();
            Mapper.CreateMap<Device, DeviceEditDto>();
            Mapper.CreateMap<Device, DeviceIndexDto>().IgnoreAllPropertiesWithAnInaccessibleSetter();

            #endregion Device

            #region DeviceErrorLog

            Mapper.CreateMap<DeviceErrorLog, DeviceErrorLogDto>();
            Mapper.CreateMap<DeviceErrorLogDto, DeviceErrorLog>();

            #endregion DeviceErrorLog

            #region Alarm

            Mapper.CreateMap<Alarm, AlarmDetailDto>()
                .ForMember(dest => dest.Parameter, opt => opt.MapFrom(src => src.Parameter.Id)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<Alarm, AlarmEditDto>()
                .ForMember(dest => dest.Parameter, opt => opt.MapFrom(src => src.Parameter.Id)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<Alarm, AlarmSensorDto>();

            Mapper.CreateMap<AlarmColor, ItemDto<AlarmColor?>>()
                .ConvertUsing(src => new ItemDto<AlarmColor?>
                {
                    Id = src,
                    Text = src.ToString(),
                    Description = src.GetDescription()
                });

            #endregion Alarm

            #region AlarmMetric

            Mapper.CreateMap<AlarmMetric, AlarmMetricSensorDto>()
                //.ForMember(dest => dest.Alarm, opt => opt.MapFrom(src => src.Alarm.Id)).IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(dest => dest.Mac, opt => opt.MapFrom(src => src.Device.Macaddress)).IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<AlarmMetric, AlarmMetricEditDto>()
                .ForMember(dest => dest.Alarm, opt => opt.MapFrom(src => src.Alarm.Id)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            // .ForMember(dest => dest.Device, opt => opt.MapFrom(src => src.Device.Id)).IgnoreAllPropertiesWithAnInaccessibleSetter();

            #endregion AlarmMetric

            #region AlarmFired

            Mapper.CreateMap<AlarmFired, AlarmFiredDetailDto>();
            Mapper.CreateMap<AlarmFired, AlarmFiredSwiftDto>()
                .ForMember(dest => dest.ParameterName, opt => opt.MapFrom(src => src.Parameter.Name)).IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => string.Format("{0}", src.Patient))).IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<AlarmColor, ItemDto<AlarmColor?>>()
               .ConvertUsing(src => new ItemDto<AlarmColor?>
               {
                   Id = src,
                   Text = src.ToString(),
                   Description = src.GetDescription()
               });

            #endregion AlarmFired

            #region Parameter

            Mapper.CreateMap<Parameter, ParameterDetailDto>()
                .ForMember(dest => dest.Process, opt => opt.MapFrom(src => src.Process.Id)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<Parameter, ParameterIndexDto>();
            Mapper.CreateMap<Parameter, ParameterSensorDto>();
            Mapper.CreateMap<Parameter, ParameterEditDto>()
                .ForMember(dest => dest.Process, opt => opt.MapFrom(src => src.Process.Id)).IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<FrequencyType, ItemDto<FrequencyType?>>()
                .ConvertUsing(src => new ItemDto<FrequencyType?>
                {
                    Id = src,
                    Text = src.ToString(),
                    Description = src.GetDescription()
                });

            #endregion Parameter

            #region ProcessMacro

            Mapper.CreateMap<ProcessMacro, ProcessMacroEditDto>()
                .ForMember(dest => dest.Process, opt => opt.MapFrom(src => src.Process.Id)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<ProcessMacro, ProcessMacroDetailDto>()
                .ForMember(dest => dest.Process, opt => opt.MapFrom(src => src.Process.Id)).IgnoreAllPropertiesWithAnInaccessibleSetter();

            #endregion ProcessMacro

            #region User

            Mapper.CreateMap<User, UserUpdateDto>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<User, UserCreateDto>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<User, UserDto>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            #endregion User

            #region Role

            Mapper.CreateMap<Role, RoleUpdateDto>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<Role, RoleCreateDto>().IgnoreAllPropertiesWithAnInaccessibleSetter();

            #endregion Role

            #region Claim

            Mapper.CreateMap<Claim, ClaimDto>().IgnoreAllPropertiesWithAnInaccessibleSetter();

            #endregion Claim

            #region InviteFriend
            Mapper.CreateMap<InviteFriend, InviteFriendDto>();
            #endregion
        }
    }
}