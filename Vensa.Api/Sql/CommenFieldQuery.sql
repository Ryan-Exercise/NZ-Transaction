ORDER BY
    CASE WHEN @OrderDirection=0 THEN
        CASE
            WHEN @OrderBy='FirstName' THEN FirstName
            WHEN @OrderBy='LastName' THEN LastName
            WHEN @OrderBy='MiddleName' THEN MiddleName
            WHEN @OrderBy='PreferredName' THEN PreferredName
            WHEN @OrderBy='DateOfBirth' THEN CONVERT(VARCHAR, DateOfBirth)
            WHEN @OrderBy='Mobile' THEN Mobile
        END
    END ASC,
    CASE WHEN @OrderDirection=1 THEN
        CASE
            WHEN @OrderBy='FirstName' THEN FirstName
            WHEN @OrderBy='LastName' THEN LastName
            WHEN @OrderBy='MiddleName' THEN MiddleName
            WHEN @OrderBy='PreferredName' THEN PreferredName
            WHEN @OrderBy='DateOfBirth' THEN CONVERT(VARCHAR, DateOfBirth)
            WHEN @OrderBy='Mobile' THEN Mobile
        END
    END DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

-- Balance & LastTransactionTime --
DECLARE @Page TABLE(ConsumerId BIGINT PRIMARY KEY, Balance DECIMAL(18,2), LastTransactionTime DATETIME);
INSERT INTO @Page
SELECT t1.ConsumerId, SUM(t2.[Value]) AS Balance, MAX(t2.[DateTime]) AS LastTransactionTime
FROM @IdPage t1 
INNER JOIN [Transaction] t2 ON t2.ConsumerId = t1.ConsumerId
GROUP BY t1.ConsumerId;

------ Most Frequent Payment Method------

SELECT * INTO #Method FROM (
SELECT t3.ConsumerId, t3.MethodCount, t4.Id as MethodId, t3.LastTransactionTime FROM (
SELECT t.ConsumerId, t1.TransactionMethod, COUNT(t1.TransactionMethod) MethodCount, MAX(t1.[DateTime]) LastTransactionTime
FROM @IdPage t 
INNER JOIN [Transaction] t1 ON t1.ConsumerId = t.ConsumerId 
GROUP BY t.ConsumerId, t1.TransactionMethod
) as t3 INNER JOIN [TransactionMethod] t4 ON t4.Id = t3.TransactionMethod)as t5;

DECLARE @MethodMax TABLE(ConsumerId BIGINT, MethodCount INT, MethodId INT, LastTransactionTime DATETIME);
INSERT INTO @MethodMax
SELECT * FROM #Method t1 WHERE MethodCount = (
    SELECT MAX(t2.MethodCount) FROM #Method t2 WHERE ConsumerId = t1.ConsumerId 
) 
DROP TABLE #Method

DECLARE @TransMethod TABLE(ConsumerId BIGINT PRIMARY KEY, MethodId INT, MethodCount INT)
INSERT INTO @TransMethod
SELECT t1.ConsumerId, t1.MethodId, t1.MethodCount FROM @MethodMax t1 WHERE LastTransactionTime = (
    SELECT MAX(t2.LastTransactionTime) FROM @MethodMax t2 WHERE ConsumerId = t1.ConsumerId
)

--- Join ---

SELECT t2.*, t3.Balance, t3.LastTransactionTime, t5.TransactionMethodName MostFrequentMethod, t4.MethodCount FROM (
        SELECT * FROM Consumer t1 WHERE t1.Id IN(SELECT ConsumerId FROM @IdPage)
    )t2 
    LEFT JOIN @Page t3 ON t3.ConsumerId = t2.Id
    LEFT JOIN @TransMethod t4 ON t4.ConsumerId = t2.Id
    LEFT JOIN TransactionMethod t5 ON t5.Id = t4.MethodId
ORDER BY
CASE WHEN @OrderDirection=0 THEN
        CASE
            WHEN @OrderBy='FirstName' THEN FirstName
            WHEN @OrderBy='LastName' THEN LastName
            WHEN @OrderBy='MiddleName' THEN MiddleName
            WHEN @OrderBy='PreferredName' THEN PreferredName
            WHEN @OrderBy='DateOfBirth' THEN CONVERT(VARCHAR, DateOfBirth)
            WHEN @OrderBy='Mobile' THEN Mobile
        END
    END ASC,
    CASE WHEN @OrderDirection=1 THEN
        CASE
            WHEN @OrderBy='FirstName' THEN FirstName
            WHEN @OrderBy='LastName' THEN LastName
            WHEN @OrderBy='MiddleName' THEN MiddleName
            WHEN @OrderBy='PreferredName' THEN PreferredName
            WHEN @OrderBy='DateOfBirth' THEN CONVERT(VARCHAR, DateOfBirth)
            WHEN @OrderBy='Mobile' THEN Mobile
        END
    END DESC