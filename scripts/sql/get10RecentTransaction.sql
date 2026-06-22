SELECT
	TOP(10)
	FORMAT(t.[Date] , 'dd MMMM yyyy') AS Date , t.Name , t.Amount , t.[Type] , a.Name AS Account, c.Name AS Category
FROM ObscuraFinanceDb.dbo.Transactions t
JOIN ObscuraFinanceDb.dbo.Accounts a ON t.AccountId = a.Id 
JOIN ObscuraFinanceDb.dbo.Categories c ON t.CategoryId  = c.Id
WHERE t.IsDeleted = 0