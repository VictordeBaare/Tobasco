CREATE PROCEDURE [dbo].[CPK_Insert]
	@Id bigint output,
@Training nvarchar(100),
@aaaId bigint,
@Duur nvarchar(100),
@Kosten nvarchar(100),
@ModifiedBy nvarchar(256)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT
      INTO [dbo].CPK 
	       (
			Training,
aaaId,
Duur,
Kosten,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC
		   )
	OUTPUT Inserted.Id
		  ,Inserted.[RowVersion]
		  ,Inserted.ModifiedOn
    VALUES
         (
		   @Training,
@aaaId,
@Duur,
@Kosten,
@ModifiedBy,
SYSDATETIME(),
SYSUTCDATETIME()
          );
END;
GO
CREATE PROCEDURE [dbo].[CPK_Update]
		@Id [bigint],
@Training nvarchar(100),
@aaaId bigint,
@Duur nvarchar(100),
@Kosten nvarchar(100),
@RowVersion [rowversion],
@ModifiedBy nvarchar(256)
AS
BEGIN
		SET NOCOUNT ON;
		
		IF OBJECT_ID('tempdb..#Output') IS NOT NULL
		BEGIN
			DROP TABLE #Output;
		END;

		CREATE TABLE #Output ([RowVersion]     binary(8)    NOT NULL
							  ,ModifiedOn      datetime2(7) NOT NULL
							 );

		UPDATE [dbo].CPK
		   SET 
				CPK.Training = @Training,
CPK.aaaId = @aaaId,
CPK.Duur = @Duur,
CPK.Kosten = @Kosten,
CPK.ModifiedBy = ISNULL(@ModifiedBy, SUSER_SNAME()),
CPK.ModifiedOn = SYSDATETIME()            
		OUTPUT Inserted.[RowVersion]
			  ,Inserted.ModifiedOn
		  INTO #Output ([RowVersion]
						,ModifiedOn
					   )
		 WHERE CPK.Id = @Id
		   AND CPK.[RowVersion] = @RowVersion

		DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();

		IF @RowCountBig = 0
		BEGIN
            DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
													   ,N'Concurrency problem. '
                                                       ,N'The CPK-row with Id=', @Id, N' and RowVersion=', CAST(@RowVersion AS binary(8)), N' cannot be altered by ', @ModifiedBy, N'. '
                                                       ,N'The row was already modified by a different user.'
                                                        );
			THROW 55501, @message, 1;
		  END;

		SELECT #Output.[RowVersion]
			  ,#Output.ModifiedOn
          FROM #Output;
END;
GO
CREATE PROCEDURE [dbo].[CPK_Delete]
		@Id [bigint]
       ,@RowVersion [rowversion]
       ,@ModifiedBy nvarchar(256)
AS
BEGIN
		SET NOCOUNT ON;
		
		DECLARE @Context varbinary(128) = CAST(CAST(ISNULL(@ModifiedBy, SUSER_SNAME()) AS char(256)) AS varbinary(256));
		
		DELETE 
		  FROM [dbo].CPK
		 WHERE CPK.Id = @Id
		   AND CPK.[RowVersion] = @RowVersion

		DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();
		
		SET CONTEXT_INFO 0x;
		
		IF @RowCountBig = 0
		BEGIN
            DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
													   ,N'Concurrency problem. '
                                                       ,N'The CPK-row with Id=', @Id, N' and RowVersion=', CAST(@RowVersion AS binary(8)), N' cannot be altered by ', @ModifiedBy, N'. '
                                                       ,N'The row was already modified or deleted by a different user.'
                                                        );
			THROW 55501, @message, 1;
		  END;
END;
GO
CREATE PROCEDURE [dbo].CPK_GetFullById
	@id bigint
AS
BEGIN
            SET NOCOUNT ON;

	DECLARE @aaaId as bigint;
SELECT @aaaId = aaaId
  FROM [dbo].CPK
 WHERE CPK.Id = @id;

	EXEC [dbo].CPK1_GetFullById @aaaId;

	SELECT Id,
		   [RowVersion],
           Duur,
Kosten,
Training,
aaaId,
		   [ModifiedBy],
		   [ModifiedOn]
	  FROM CPK
	 WHERE CPK.Id = @id;
END;
GO
CREATE PROCEDURE [dbo].CPK_GetFullByIds
	@Ids AS [dbo].BigintType READONLY
AS
BEGIN
            SET NOCOUNT ON;
	DECLARE @aaaIds AS [dbo].BigintType;
INSERT INTO @aaaIds
SELECT aaaId
  FROM [dbo].CPK
where EXISTS(SELECT 1						 
	  FROM @Ids									 
	  WHERE[@Ids].Id = CPK.Id
      )
AND aaaId IS NOT NULL;

	EXEC CPK1_GetFullByIds @aaaIds

	SELECT Id,
		   [RowVersion],
           Duur,
Kosten,
Training,
aaaId,
		   [ModifiedBy],
		   [ModifiedOn]
	  FROM CPK
	  WHERE EXISTS (SELECT 1
                     FROM @Ids
                    WHERE [@Ids].Id = CPK.Id
                  );
END;