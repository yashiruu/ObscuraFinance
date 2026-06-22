using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Application.Interfaces;

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
        public async Task<ActionResult<List<TransactionListResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var transactions = await _transactionService.GetAllAsync(cancellationToken);

            return Ok(new ApiResponse<IEnumerable<TransactionListResponse>>
            {
                Success = true,
                Message = "Transactions retrieved successfully",
                Data = transactions
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TransactionDetailResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _transactionService.GetByIdAsync(id, cancellationToken);

            return Ok(new ApiResponse<TransactionDetailResponse>
            {
                Success = true,
                Message = "Transaction retrieved successfully",
                Data = transaction
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create(TransactionCreateRequest request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, new ApiResponse<TransactionDetailResponse>
            {
                Success = true,
                Message = "Transaction created successfully",
                Data = transaction
            });
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
