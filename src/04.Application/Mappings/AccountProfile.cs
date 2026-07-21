using AutoMapper;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Application.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            // Entity to Response
            CreateMap<Account, AccountListResponse>();
            CreateMap<Account, AccountDetailResponse>();
            // Requst to Entity
            CreateMap<AccountCreateRequest, Account>();
        }
    }
}
