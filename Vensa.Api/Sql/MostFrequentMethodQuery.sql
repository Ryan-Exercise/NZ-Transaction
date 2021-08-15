-- Stat Field MostFrequentMethod --

SELECT t1.ConsumerId, t1.TransactionMethod, COUNT(t1.TransactionMethod) MethodCount, MAX(t1.[DateTime]) as LastTransactionTime
INTO #Method
FROM [Transaction] t1 GROUP BY t1.ConsumerId, t1.TransactionMethod;

DECLARE @MethodMax TABLE(ConsumerId BIGINT,  MethodId INT, MethodCount INT, LastTransactionTime DATETIME);
INSERT INTO @MethodMax
SELECT t2.ConsumerId, t3.TransactionMethod MethodId, t2.MethodCount, t3.LastTransactionTime 
FROM (SELECT t1.ConsumerId, MAX(t1.MethodCount) MethodCount FROM #Method t1 GROUP BY t1.ConsumerId)
t2 JOIN #Method t3 ON t3.ConsumerId = t2.ConsumerId AND t3.MethodCount = t2.MethodCount
ORDER BY t2.MethodCount, ConsumerId
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

DROP TABLE #Method;

DECLARE @TransMethod TABLE(ConsumerId BIGINT PRIMARY KEY, MethodId INT, MethodCount INT)
INSERT INTO @TransMethod
SELECT t1.ConsumerId, t1.MethodId, t1.MethodCount FROM @MethodMax t1 WHERE LastTransactionTime = (
    SELECT MAX(t2.LastTransactionTime) FROM @MethodMax t2 WHERE ConsumerId = t1.ConsumerId
);

DECLARE @IdPage TABLE(ConsumerId BIGINT PRIMARY KEY);
INSERT INTO @IdPage
SELECT ConsumerId FROM @TransMethod;

SELECT t6.*, t3.Balance, t3.LastTransactionTime, t5.TransactionMethodName as MostFrequentMethod, t4.MethodCount
 FROM (
SELECT t1.ConsumerId, MAX(t2.[DateTime]) AS LastTransactionTime, SUM(t2.[Value]) AS Balance FROM @IdPage t1 
LEFT JOIN [Transaction] t2 ON t2.ConsumerId = t1.ConsumerId GROUP BY t1.ConsumerId
) t3 
LEFT JOIN @TransMethod t4 ON t4.ConsumerId = t3.ConsumerId
LEFT JOIN TransactionMethod t5 ON t5.Id=t4.MethodId
LEFT JOIN Consumer t6 ON t6.Id = t3.ConsumerId
ORDER BY MethodCount;