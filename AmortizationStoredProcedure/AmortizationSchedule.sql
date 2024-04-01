--Querying this code to your database will createe GetAmortizationSchedule stored procedure
--You can call it using EXEC [dbo].[GetAmortizationSchedule] command
CREATE PROCEDURE GetAmortizationSchedule
(
    --Inital parameters which can be changed by requirement
    @TotalLoan DECIMAL(19,2) = 36000,
    @PaymentCount INT = 36,	
    @RecycledPaymentCount INT = 48,
    @Interest DECIMAL(19,2) = 8,
    @RecycledInterest DECIMAL(19,2) = 4.5
)
AS
BEGIN
   --Declaring multiple variables for code simplifying
   DECLARE @MonthlyInterest DECIMAL(19,10);
   SET @MonthlyInterest = @Interest / 12 / 100;

   DECLARE @RecycledMonthlyInterest DECIMAL(19,10);
   SET @RecycledMonthlyInterest = @RecycledInterest / 12 / 100;

   DECLARE @MonthlyPayment DECIMAL(19,2);
   SET @MonthlyPayment = (@MonthlyInterest * @TotalLoan) / (1 - POWER(1 + @MonthlyInterest, -@PaymentCount));

   --Use Amortization Schedule for storing initial interest and number of payments
   WITH AmortizationSchedule AS (
       SELECT 
           1 AS [Payment],
		   @MonthlyPayment AS [PaymentValue],
           @TotalLoan AS [StartingBalance],
           CONVERT(DECIMAL(19, 2), @TotalLoan - (@MonthlyPayment - (@TotalLoan * @MonthlyInterest))) AS [EndingBalance],
           @MonthlyPayment - (@TotalLoan * @MonthlyInterest) AS [Principal],
           @TotalLoan * @MonthlyInterest AS [InterestValue]
       UNION ALL
       SELECT 
           [Payment] + 1,
		   @MonthlyPayment,
           [EndingBalance],
           CONVERT(DECIMAL(19, 2),[EndingBalance] - (@MonthlyPayment - ([EndingBalance] * @MonthlyInterest))),
           @MonthlyPayment - ([EndingBalance] * @MonthlyInterest),
		   [EndingBalance] * @MonthlyInterest
       FROM [AmortizationSchedule]
       WHERE [Payment] < 12
   ),
   --Using RecycledPaymentSchedule to get monthly payment based on ending balance
   RecycledPaymentSchedule AS (
	 SELECT CONVERT(DECIMAL(19, 10), (@RecycledMonthlyInterest * [EndingBalance]) / (1 - POWER(1 + @RecycledMonthlyInterest, -@RecycledPaymentCount))) AS [RecycledMonthlyPayment],
     [EndingBalance] AS [RecycledEndingBalance]
     FROM [AmortizationSchedule]
     WHERE [Payment] = 12),
   --Using RecycledAmortizationSchedule for adding recycled payments
   RecycledAmortizationSchedule AS (
       SELECT 
			12 + 1 AS [Payment],
			[RecycledPaymentSchedule].[RecycledEndingBalance] AS [StartingBalance],
			[RecycledPaymentSchedule].[RecycledMonthlyPayment] AS [PaymentValue],
			CONVERT(DECIMAL(19, 2), [RecycledPaymentSchedule].[RecycledEndingBalance] - ([RecycledPaymentSchedule].[RecycledMonthlyPayment] - ([RecycledPaymentSchedule].[RecycledEndingBalance] * @RecycledMonthlyInterest))) AS [EndingBalance],
			[RecycledPaymentSchedule].[RecycledMonthlyPayment] - [RecycledPaymentSchedule].[RecycledEndingBalance] * @RecycledMonthlyInterest AS [Principal],
			[RecycledPaymentSchedule].[RecycledEndingBalance] * @RecycledMonthlyInterest AS [InterestValue]
       FROM	[RecycledPaymentSchedule]
       UNION ALL
       SELECT 
           [Payment] + 1,
           [EndingBalance],
		   [PaymentValue],
           CONVERT(DECIMAL(19, 2), [EndingBalance] - ([RecycledMonthlyPayment] - ([EndingBalance] * @RecycledMonthlyInterest))),
           [RecycledMonthlyPayment] - ([EndingBalance] * @RecycledMonthlyInterest),
           [EndingBalance] * @RecycledMonthlyInterest
       FROM [RecycledAmortizationSchedule]	
	   CROSS JOIN [RecycledPaymentSchedule]
       WHERE [Payment] < 12 + @RecycledPaymentCount
   )

	SELECT 
	   [Payment],
	   [StartingBalance],
	   CAST([PaymentValue] AS DECIMAL(19,2)) AS [PaymentValue],
	   [EndingBalance],
	   CAST([Principal] AS DECIMAL(19,2)) AS [Principal],
	   CAST([InterestValue] AS DECIMAL(19,2)) AS [InterestValue]
	FROM [AmortizationSchedule]
	UNION ALL
	SELECT 
	   [Payment],
	   [StartingBalance],
	   CAST([PaymentValue] AS DECIMAL(19,2)) AS [PaymentValue],
	   [EndingBalance],
	   CAST([Principal] AS DECIMAL(19,2)) AS [Principal],
	   CAST([InterestValue] AS DECIMAL(19,2)) AS [InterestValue]
	FROM [RecycledAmortizationSchedule]
END