using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Services;

namespace Obscura.FinanceTracker.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountListResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var accounts = await _accountService.GetAllAsync(cancellationToken);

            return Ok(ApiResponse<IEnumerable<AccountListResponse>>.SuccessResponse(accounts, "Accounts retrieved successfully"));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AccountDetailResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var account = await _accountService.GetByIdAsync(id, cancellationToken);

            return Ok(ApiResponse<AccountDetailResponse>.SuccessResponse(account, "Account retrieved successfully"));
        }

        [HttpPost]
        public async Task<ActionResult<AccountDetailResponse>> Create(AccountCreateRequest request, CancellationToken cancellationToken)
        {
            var account = await _accountService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = account.Id }, ApiResponse<AccountDetailResponse>.SuccessResponse(account, "Account created successfully"));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, AccountUpdateRequest request, CancellationToken cancellationToken)
        {
            await _accountService.UpdateAsync(id, request, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _accountService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }

        [HttpPatch("{id:guid}/restore")]
        public async Task<ActionResult> Restore(Guid id, CancellationToken cancellationToken)
        {
            await _accountService.RestoreAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
