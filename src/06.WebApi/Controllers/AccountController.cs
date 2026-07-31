using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Services;
using Obscura.FinanceTracker.Shared.Models;

namespace Obscura.FinanceTracker.WebApi.Controllers
{
    /// <summary>
    /// Manages financial accounts within the system.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Retrieves a paged list of active accounts.
        /// </summary>
        /// <param name="request">Pagination parameters (settable: <c>PageNumber</c>, <c>PageSize</c>).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="200">Returns the paged list of accounts.</response>
        /// <response code="400">If the pagination parameters cannot be bound.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AccountListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<AccountListResponse>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken)
        {
            var accounts = await _accountService.GetAllAsync(request, cancellationToken);

            return Ok(ApiResponse<PagedResult<AccountListResponse>>.SuccessResponse(accounts, "Accounts retrieved successfully"));
        }

        /// <summary>
        /// Retrieves a specific account by its unique identifier.
        /// </summary>
        /// <param name="id">The account id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="200">Returns the requested account.</response>
        /// <response code="404">If the account does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<AccountDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDetailResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var account = await _accountService.GetByIdAsync(id, cancellationToken);

            return Ok(ApiResponse<AccountDetailResponse>.SuccessResponse(account, "Account retrieved successfully"));
        }

        /// <summary>
        /// Creates a new financial account.
        /// </summary>
        /// <param name="request">The account data to create.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="201">Returns the newly created account.</response>
        /// <response code="400">If the request is invalid or an account with the same name already exists.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ApiResponse<AccountDetailResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDetailResponse>> Create(AccountCreateRequest request, CancellationToken cancellationToken)
        {
            var account = await _accountService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = account.Id }, ApiResponse<AccountDetailResponse>.SuccessResponse(account, "Account created successfully"));
        }

        /// <summary>
        /// Updates an existing account.
        /// </summary>
        /// <param name="id">The id of the account to update.</param>
        /// <param name="request">The updated account data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="204">If the account was updated successfully.</response>
        /// <response code="400">If the request is invalid or the new name conflicts with an existing account.</response>
        /// <response code="404">If the account does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(Guid id, AccountUpdateRequest request, CancellationToken cancellationToken)
        {
            await _accountService.UpdateAsync(id, request, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Soft deletes an account by its unique identifier.
        /// </summary>
        /// <param name="id">The id of the account to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="204">If the account was deleted successfully.</response>
        /// <response code="404">If the account does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _accountService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Restores a previously soft-deleted account.
        /// </summary>
        /// <param name="id">The id of the account to restore.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="204">If the account was restored successfully.</response>
        /// <response code="400">If restoring the account causes a name conflict.</response>
        /// <response code="404">If the account does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpPatch("{id:guid}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Restore(Guid id, CancellationToken cancellationToken)
        {
            await _accountService.RestoreAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
