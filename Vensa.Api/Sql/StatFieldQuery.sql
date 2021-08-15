ORDER BY
CASE WHEN @OrderDirection=0 THEN
        CASE
            WHEN @OrderBy='LastTransactionTime' THEN CONVERT(INT, LastTransactionTime)
            WHEN @OrderBy='Balance' THEN Balance
        END
    END ASC,
    CASE WHEN @OrderDirection=1 THEN
        CASE
            WHEN @OrderBy='LastTransactionTime' THEN CONVERT(INT, LastTransactionTime)
            WHEN @OrderBy='Balance' THEN Balance
        END
    END DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

DECLARE @IdPage TABLE(ConsumerId BIGINT PRIMARY KEY);
INSERT INTO @IdPage
SELECT ConsumerId FROM @Page;

------ Most Frequent Payment Method------
SELECT * INTO #Method FROM (
SELECT t3.ConsumerId as ConsumerId, t3.MethodCount, t4.Id as MethodId, t3.LastTransactionTime FROM (
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
DROP TABLE #Method;

DECLARE @TransMethod TABLE(ConsumerId BIGINT PRIMARY KEY, MethodId INT, MethodCount INT)
INSERT INTO @TransMethod
SELECT t1.ConsumerId, t1.MethodId, t1.MethodCount FROM @MethodMax t1 WHERE LastTransactionTime = (
    SELECT MAX(t2.LastTransactionTime) FROM @MethodMax t2 WHERE ConsumerId = t1.ConsumerId
)

SELECT t2.*, t3.Balance, t3.LastTransactionTime, t5.TransactionMethodName MostFrequentMethod, t4.MethodCount FROM (
        SELECT * FROM Consumer t1 WHERE t1.Id IN(SELECT ConsumerId FROM @IdPage)
    )t2 
    LEFT JOIN @Page t3 ON t3.ConsumerId = t2.Id
    LEFT JOIN @TransMethod t4 ON t4.ConsumerId = t2.Id
    LEFT JOIN TransactionMethod t5 ON t5.Id = t4.MethodId
ORDER BY
CASE WHEN @OrderDirection=0 THEN
        CASE
            WHEN @OrderBy='LastTransactionTime' THEN CONVERT(INT, LastTransactionTime)
            WHEN @OrderBy='Balance' THEN Balance
        END
    END ASC,
    CASE WHEN @OrderDirection=1 THEN
        CASE
            WHEN @OrderBy='LastTransactionTime' THEN CONVERT(INT, LastTransactionTime)
            WHEN @OrderBy='Balance' THEN Balance
        END
    END DESC;

