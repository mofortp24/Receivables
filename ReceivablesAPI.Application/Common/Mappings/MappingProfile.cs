using AutoMapper;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;
using ReceivablesAPI.Domain.Entities;
using ReceivablesAPI.Domain.Enums;

namespace ReceivablesAPI.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ReceivableDto, Receivable>()
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference))
            .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
            .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => DateTime.Parse(src.IssueDate)))
            .ForMember(dest => dest.OpeningValue, opt => opt.MapFrom(src => src.OpeningValue))
            .ForMember(dest => dest.PaidValue, opt => opt.MapFrom(src => src.PaidValue))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => DateTime.Parse(src.DueDate)))
            .ForMember(dest => dest.ClosedDate, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ClosedDate) ? null : (DateTime?)DateTime.Parse(src.ClosedDate)))
            .ForMember(dest => dest.Cancelled, opt => opt.MapFrom(src => src.Cancelled))
            .ForMember(dest => dest.Debtor, opt => opt.MapFrom(src => new ReceivableDebtor
            {
                DebtorReference = src.DebtorReference,
                DebtorName = src.DebtorName
            }))
            .ForMember(dest => dest.DebtorAddress, opt => opt.MapFrom(src => new ReceivableDebtorAddress
            {
                DebtorAddress1 = src.DebtorAddress1,
                DebtorAddress2 = src.DebtorAddress2,
                DebtorTown = src.DebtorTown,
                DebtorState = src.DebtorState,
                DebtorZip = src.DebtorZip,
                DebtorCountryCode = Enum.Parse<CountryCode>(src.DebtorCountryCode),
                DebtorRegistrationNumber = src.DebtorRegistrationNumber
            }));
    }
}