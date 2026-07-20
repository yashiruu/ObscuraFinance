using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Application.Services;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Shared.Exceptions;
using ObscuraFinance.Application.UnitTests.Base;
using Xunit;

namespace ObscuraFinance.Application.UnitTests.Services
{
    /// <summary>
    /// Comprehensive unit tests for <see cref="CategoryService"/> covering all business scenarios.
    /// </summary>
    public class CategoryServiceTest : CategoryServiceTestBase
    {
        #region GetAllAsync Tests

        /// <summary>
        /// Verifies that GetAllAsync retrieves categories from the repository and returns them as a mapped list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_Should_ReturnMappedList_When_CategoriesExist()
        {
            // Arrange
            var categories = new List<Category> { new Category { Id = Guid.NewGuid(), Name = "Salary" } };
            var expectedResponse = new List<CategoryResponse> { new CategoryResponse { Name = "Salary" } };

            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);
            _mapperMock.Setup(m => m.Map<IEnumerable<CategoryResponse>>(categories)).Returns(expectedResponse);

            // Act
            var result = await _categoryService.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
            _categoryRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        #endregion

        #region GetByTypeAsync Tests

        /// <summary>
        /// Verifies that GetByTypeAsync filters categories correctly based on the provided transaction type.
        /// </summary>
        [Fact]
        public async Task GetByTypeAsync_Should_ReturnFilteredMappedList_When_Called()
        {
            // Arrange
            var type = TransactionType.Income;
            var categories = new List<Category> { new Category { Id = Guid.NewGuid(), Type = type } };
            var expectedResponse = new List<CategoryResponse> { new CategoryResponse { Type = type } };

            _categoryRepositoryMock.Setup(repo => repo.GetAllByTypeAsync(type)).ReturnsAsync(categories);
            _mapperMock.Setup(m => m.Map<IEnumerable<CategoryResponse>>(categories)).Returns(expectedResponse);

            // Act
            var result = await _categoryService.GetByTypeAsync(type, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
            _categoryRepositoryMock.Verify(repo => repo.GetAllByTypeAsync(type), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Verifies that GetByIdAsync returns a mapped category when the ID is valid.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_Should_ReturnMappedCategory_When_CategoryExists()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Food" };
            var expectedResponse = new CategoryResponse { Id = categoryId, Name = "Food" };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<CategoryResponse>(category)).Returns(expectedResponse);

            // Act
            var result = await _categoryService.GetByIdAsync(categoryId, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
        }

        /// <summary>
        /// Verifies that GetByIdAsync throws a KeyNotFoundException when the category is missing.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_Should_ThrowKeyNotFoundException_When_CategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);

            // Act
            var act = async () => await _categoryService.GetByIdAsync(categoryId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*{categoryId}*");
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Verifies that CreateAsync successfully persists a new category when the request is valid.
        /// </summary>
        [Fact]
        public async Task CreateAsync_Should_CreateAndReturnResponse_When_RequestIsValid()
        {
            // Arrange
            var request = new CategoryCreateRequest { Name = "Freelance", Type = TransactionType.Income };
            var categoryEntity = new Category { Id = Guid.NewGuid(), Name = request.Name, Type = request.Type };
            var expectedResponse = new CategoryResponse { Id = categoryEntity.Id, Name = request.Name, Type = request.Type };

            _categoryRepositoryMock.Setup(repo => repo.IsNameTakenAsync(request.Name, null)).ReturnsAsync(false);
            _mapperMock.Setup(m => m.Map<Category>(request)).Returns(categoryEntity);
            _mapperMock.Setup(m => m.Map<CategoryResponse>(categoryEntity)).Returns(expectedResponse);

            // Act
            var result = await _categoryService.CreateAsync(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
            _categoryRepositoryMock.Verify(repo => repo.AddAsync(categoryEntity), Times.Once); // Assuming repo has Add sync or async
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that CreateAsync throws a BusinessException when the requested category name already exists.
        /// </summary>
        [Fact]
        public async Task CreateAsync_Should_ThrowBusinessException_When_NameIsTaken()
        {
            // Arrange
            var request = new CategoryCreateRequest { Name = "DuplicateName" };
            _categoryRepositoryMock.Setup(repo => repo.IsNameTakenAsync(request.Name, null)).ReturnsAsync(true);

            // Act
            var act = async () => await _categoryService.CreateAsync(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage($"Category with '{request.Name}' already exists.");

            _categoryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Never);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Verifies that UpdateAsync updates existing category successfully when data is valid.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_Should_UpdateCategory_When_RequestIsValid()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var request = new CategoryUpdateRequest { Name = "Updated Name", Description = "New Desc" };
            var existingCategory = new Category { Id = categoryId, Name = "Old Name" };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);
            _categoryRepositoryMock.Setup(repo => repo.IsNameTakenAsync(request.Name, categoryId)).ReturnsAsync(false);

            // Act
            var act = async () => await _categoryService.UpdateAsync(categoryId, request, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();
            //_mapperMock.Verify(m => m.Map(request, existingCategory), Times.Once);  // Assuming you have a mapping from request to existingCategory
            _categoryRepositoryMock.Verify(repo => repo.Update(existingCategory), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that UpdateAsync throws a KeyNotFoundException when the target category does not exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_Should_ThrowKeyNotFoundException_When_CategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var request = new CategoryUpdateRequest { Name = "Updated Name" };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);

            // Act
            var act = async () => await _categoryService.UpdateAsync(categoryId, request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*{categoryId}*");
        }

        /// <summary>
        /// Verifies that UpdateAsync throws a BusinessException when the new name is used by another category.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_Should_ThrowBusinessException_When_NameIsTaken()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var request = new CategoryUpdateRequest { Name = "Existing Name" };
            var existingCategory = new Category { Id = categoryId, Name = "Old Name" };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);
            _categoryRepositoryMock.Setup(repo => repo.IsNameTakenAsync(request.Name, categoryId)).ReturnsAsync(true);

            // Act
            var act = async () => await _categoryService.UpdateAsync(categoryId, request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage($"Category with '{request.Name}' already exists.");

            _categoryRepositoryMock.Verify(repo => repo.Update(It.IsAny<Category>()), Times.Never);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Verifies that DeleteAsync delegates the deletion process to the repository successfully.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_Should_DeleteCategory_When_CategoryExists()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var act = async () => await _categoryService.DeleteAsync(categoryId, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();
            _categoryRepositoryMock.Verify(repo => repo.Delete(category), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that DeleteAsync throws a KeyNotFoundException when attempting to delete a non-existent category.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_Should_ThrowKeyNotFoundException_When_CategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);

            // Act
            var act = async () => await _categoryService.DeleteAsync(categoryId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Category with '{categoryId}' was not found");

            _categoryRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region RestoreAsync Tests

        /// <summary>
        /// Verifies that RestoreAsync successfully marks a soft-deleted category as active.
        /// </summary>
        [Fact]
        public async Task RestoreAsync_Should_RestoreCategory_When_CategoryExistsAndNameIsNotTaken()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Freelance", IsDeleted = true };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdIncludingDeletedAsync(categoryId)).ReturnsAsync(category);
            _categoryRepositoryMock.Setup(repo => repo.IsNameTakenAsync(category.Name, categoryId)).ReturnsAsync(false);

            // Act
            var act = async () => await _categoryService.RestoreAsync(categoryId, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();
            category.IsDeleted.Should().BeFalse();
            _categoryRepositoryMock.Verify(repo => repo.Update(category), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that RestoreAsync throws BusinessException if restoring causes a name conflict.
        /// </summary>
        [Fact]
        public async Task RestoreAsync_Should_ThrowBusinessException_When_NameIsAlreadyTaken()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Utilities", IsDeleted = true };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdIncludingDeletedAsync(categoryId)).ReturnsAsync(category);
            _categoryRepositoryMock.Setup(repo => repo.IsNameTakenAsync(category.Name, categoryId)).ReturnsAsync(true);

            // Act
            var act = async () => await _categoryService.RestoreAsync(categoryId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage($"Category with '{category.Name}' already exists.");

            _categoryRepositoryMock.Verify(repo => repo.Update(It.IsAny<Category>()), Times.Never);
        }

        #endregion
    }
}