using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Services;
using Obscura.FinanceTracker.Shared.Models;

namespace Obscura.FinanceTracker.WebApi.Controllers
{
    /// <summary>
    /// Manages financial transactions within the system.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionController"/> class.
        /// </summary>
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Retrieves a paged list of active transactions.
        /// </summary>
        /// <param name="request">Pagination parameters (settable: <c>PageNumber</c>, <c>PageSize</c>).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="200">Returns the paged list of transactions.</response>
        /// <response code="400">If the pagination parameters cannot be bound.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TransactionListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<TransactionListResponse>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionService.GetAllAsync(request, cancellationToken);

            return Ok(ApiResponse<PagedResult<TransactionListResponse>>.SuccessResponse(transactions, "Transactions retrieved successfully"));
        }

        /// <summary>
        /// Retrieves a specific transaction by its unique identifier.
        /// </summary>
        /// <param name="id">The transaction id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="200">Returns the requested transaction.</response>
        /// <response code="404">If the transaction does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<TransactionDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransactionDetailResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _transactionService.GetByIdAsync(id, cancellationToken);

            return Ok(ApiResponse<TransactionDetailResponse>.SuccessResponse(transaction, "Transaction retrieved successfully"));
        }

        /// <summary>
        /// Creates a new financial transaction.
        /// </summary>
        /// <param name="request">The transaction data to create.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="201">Returns the newly created transaction.</response>
        /// <response code="400">If the request data is invalid (e.g., negative amount).</response>
        /// <response code="404">If the referenced account or category does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ApiResponse<TransactionDetailResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransactionDetailResponse>> Create(TransactionCreateRequest request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, ApiResponse<TransactionDetailResponse>.SuccessResponse(transaction, "Transaction created successfully"));
        }

        /// <summary>
        /// Updates an existing transaction.
        /// </summary>
        /// <param name="id">The id of the transaction to update.</param>
        /// <param name="request">The updated transaction data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="204">If the transaction was updated successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="404">If the transaction, account, or category does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(Guid id, TransactionUpdateRequest request, CancellationToken cancellationToken)
        {
            await _transactionService.UpdateAsync(id, request, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Soft deletes a transaction by its unique identifier.
        /// </summary>
        /// <param name="id">The id of the transaction to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="204">If the transaction was deleted successfully.</response>
        /// <response code="404">If the transaction does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _transactionService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Restores a previously soft-deleted transaction.
        /// </summary>
        /// <param name="id">The id of the transaction to restore.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="204">If the transaction was restored successfully.</response>
        /// <response code="404">If the transaction does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpPatch("{id:guid}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Restore(Guid id, CancellationToken cancellationToken)
        {
            await _transactionService.RestoreAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
