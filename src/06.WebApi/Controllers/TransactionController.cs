using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Services;

namespace Obscura.FinanceTracker.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionListResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var transactions = await _transactionService.GetAllAsync(cancellationToken);

            return Ok(ApiResponse<IEnumerable<TransactionListResponse>>.SuccessResponse(transactions, "Transactions retrieved successfully"));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TransactionDetailResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _transactionService.GetByIdAsync(id, cancellationToken);

            return Ok(ApiResponse<TransactionDetailResponse>.SuccessResponse(transaction, "Transaction retrieved successfully"));
        }

        [HttpPost]
        public async Task<ActionResult> Create(TransactionCreateRequest request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, ApiResponse<TransactionDetailResponse>.SuccessResponse(transaction, "Transaction created successfully"));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, TransactionUpdateRequest request, CancellationToken cancellationToken)
        {
            await _transactionService.UpdateAsync(id, request, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete (Guid id, CancellationToken cancellationToken)
        {
            await _transactionService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }

        [HttpPatch("{id:guid}/restore")]
        public async Task<ActionResult> Restore(Guid id, CancellationToken cancellationToken)
        {
            await _transactionService.RestoreAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
