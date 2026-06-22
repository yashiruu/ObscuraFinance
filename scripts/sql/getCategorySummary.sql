SELECT 
	c.Name as Category,
	SUM(t.Amount) AS Amount 
FROM ObscuraFinanceDb.dbo.Transactions t 
JOIN ObscuraFinanceDb.dbo.Categories c ON t.CategoryId = c.Id
WHERE t.IsDeleted = 0
GROUP BY c.Name