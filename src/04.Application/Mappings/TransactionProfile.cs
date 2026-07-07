using AutoMapper;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Application.Mappings
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionListResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account!.Name));

            CreateMap<Transaction, TransactionDetailResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account!.Name));

            CreateMap<TransactionCreateRequest, Transaction>();
        }
    }
}
