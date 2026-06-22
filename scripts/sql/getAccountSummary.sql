SELECT 
	a.Name as "Account Name",
	a.CurrentBalance as Balance
FROM ObscuraFinanceDb.dbo.Accounts a 
WHERE a.CurrentBalance != 0
ORDER BY a.CurrentBalance DESC