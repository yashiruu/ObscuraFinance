using FluentAssertions;
using Moq;
using Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses;
using Obscura.FinanceTracker.Domain.Enums;
using ObscuraFinance.Application.UnitTests.Base;
using Xunit;

namespace ObscuraFinance.Application.UnitTests.Services
{
    /// <summary>
    /// Unit tests for <see cref="DashboardService"/>.
    ///
    /// DashboardService.GetDashboardSummaryAsync is a pure pass-through method:
    /// it performs no aggregation, calculation, or transformation — it simply
    /// delegates to IDashboardRepository and returns the result as-is. Because
    /// of this, these tests intentionally use minimal response data and place
    /// the primary verification value on confirming the delegation itself
    /// (via Moq Verify), not on exercising business logic (there is none here).
    ///
    /// Note: DashboardService bypasses IUnitOfWork by design (direct
    /// IDashboardRepository injection) — see 02-architecture.md. This is why
    /// DashboardServiceTestBase does not wire up _unitOfWorkMock.Accounts-style
    /// nested mocks like the other service test bases do.
    /// </summary>
    public class DashboardServiceTest : DashboardServiceTestBase
    {
        /// <summary>
        /// Verifies that GetDashboardSummaryAsync returns exactly what the
        /// repository provides, confirming the service correctly delegates
        /// without altering the data.
        /// </summary>
        [Fact]
        public async Task GetDashboardSummaryAsync_Should_ReturnDashboardSummary_When_DataExists()
        {
            // Arrange
            var expected = new DashboardSummaryResponse
            {
                TotalIncome = 15_000_000m,
                TotalExpense = 4_250_000m,
                CurrentBalance = 10_750_000m,
                TotalTransaction = 8,
                LastUpdated = new DateTime(2026, 7, 20),

                RecentTransactions =
                [
                    new RecentTransactionResponse
                    {
                        Id = Guid.NewGuid(),
                        TransactionDate = new DateTime(2026, 7, 20),
                        Description = "Monthly Salary",
                        Type = TransactionType.Income,
                        Category = "Salary",
                        Account = "BCA",
                        Amount = 15_000_000m
                    }
                ],

                CategoryExpenses =
                [
                    new CategoryExpenseResponse
                    {
                        CategoryName = "Food",
                        TotalExpense = 1_250_000m
                    }
                ],

                AccountSummary =
                [
                    new AccountSummaryResponse
                    {
                        Id = Guid.NewGuid(),
                        AccountName = "BCA",
                        Balance = 9_500_000m
                    }
                ]
            };

            _dashboardRepositoryMock
                .Setup(repo => repo.GetDashboardSummaryAsync())
                .ReturnsAsync(expected);

            // Act
            var result = await _dashboardService.GetDashboardSummaryAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);

            _dashboardRepositoryMock.Verify(
                repo => repo.GetDashboardSummaryAsync(),
                Times.Once);
        }

        /// <summary>
        /// Verifies that GetDashboardSummaryAsync does not throw and correctly
        /// passes through an empty-state summary (zero totals, empty lists)
        /// when no transactions exist yet — the typical new-user scenario.
        /// </summary>
        [Fact]
        public async Task GetDashboardSummaryAsync_Should_ReturnZeroValues_When_NoDataExists()
        {
            // Arrange
            var expected = new DashboardSummaryResponse
            {
                TotalIncome = 0m,
                TotalExpense = 0m,
                CurrentBalance = 0m,
                TotalTransaction = 0,
                LastUpdated = new DateTime(2026, 7, 20),
                RecentTransactions = [],
                CategoryExpenses = [],
                AccountSummary = []
            };

            _dashboardRepositoryMock
                .Setup(repo => repo.GetDashboardSummaryAsync())
                .ReturnsAsync(expected);

            // Act
            var result = await _dashboardService.GetDashboardSummaryAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalIncome.Should().Be(0m);
            result.TotalExpense.Should().Be(0m);
            result.CurrentBalance.Should().Be(0m);
            result.RecentTransactions.Should().BeEmpty();
            result.CategoryExpenses.Should().BeEmpty();
            result.AccountSummary.Should().BeEmpty();

            _dashboardRepositoryMock.Verify(
                repo => repo.GetDashboardSummaryAsync(),
                Times.Once);
        }
    }
}