using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Services;

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

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Retrieves all active transactions.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of transaction summaries.</returns>
        /// <response code="200">Returns the list of transactions successfully.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TransactionListResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TransactionListResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var transactions = await _transactionService.GetAllAsync(cancellationToken);

            return Ok(ApiResponse<IEnumerable<TransactionListResponse>>.SuccessResponse(transactions, "Transactions retrieved successfully"));
        }

        /// <summary>
        /// Retrieves a specific transaction by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Detailed information about the requested transaction.</returns>
        /// <response code="200">Returns the requested transaction.</response>
        /// <response code="404">If the transaction with the specified ID does not exist.</response>
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
        /// <param name="request">The data required to create a new transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The newly created transaction details.</returns>
        /// <response code="201">Returns the newly created transaction.</response>
        /// <response code="400">If the request data is invalid (e.g., negative amount, invalid account ID).</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ApiResponse<TransactionDetailResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransactionDetailResponse>> Create(TransactionCreateRequest request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, ApiResponse<TransactionDetailResponse>.SuccessResponse(transaction, "Transaction created successfully"));
        }

        /// <summary>
        /// Updates an existing transaction.
        /// </summary>
        /// <param name="id">The unique identifier of the transaction to update.</param>
        /// <param name="request">The updated transaction data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">If the transaction was updated successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="404">If the transaction with the specified ID does not exist.</response>
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
        /// <param name="id">The unique identifier of the transaction to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">If the transaction was deleted successfully.</response>
        /// <response code="404">If the transaction with the specified ID does not exist.</response>
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
        /// <param name="id">The unique identifier of the transaction to restore.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">If the transaction was restored successfully.</response>
        /// <response code="404">If the transaction with the specified ID does not exist.</response>
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