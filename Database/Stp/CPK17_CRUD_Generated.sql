CREATE PROCEDURE [dbo].[CPK17_Insert]
	@Id bigint output,
@Training nvarchar(100),
@Duur nvarchar(100),
@Kosten nvarchar(100),
@ModifiedBy nvarchar(256)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT
      INTO [dbo].CPK17 
	       (
			Training,
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
@Duur,
@Kosten,
@ModifiedBy,
SYSDATETIME(),
SYSUTCDATETIME()
          );
END;
GO
CREATE PROCEDURE [dbo].[CPK17_Update]
		@Id [bigint],
@Training nvarchar(100),
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

		UPDATE [dbo].CPK17
		   SET 
				CPK17.Training = @Training,
CPK17.Duur = @Duur,
CPK17.Kosten = @Kosten,
CPK17.ModifiedBy = ISNULL(@ModifiedBy, SUSER_SNAME()),
CPK17.ModifiedOn = SYSDATETIME()            
		OUTPUT Inserted.[RowVersion]
			  ,Inserted.ModifiedOn
		  INTO #Output ([RowVersion]
						,ModifiedOn
					   )
		 WHERE CPK17.Id = @Id
		   AND CPK17.[RowVersion] = @RowVersion

		DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();

		IF @RowCountBig = 0
		BEGIN
            DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
													   ,N'Concurrency problem. '
                                                       ,N'The CPK17-row with Id=', @Id, N' and RowVersion=', CAST(@RowVersion AS binary(8)), N' cannot be altered by ', @ModifiedBy, N'. '
                                                       ,N'The row was already modified by a different user.'
                                                        );
			THROW 55501, @message, 1;
		  END;

		SELECT #Output.[RowVersion]
			  ,#Output.ModifiedOn
          FROM #Output;
END;
GO
CREATE PROCEDURE [dbo].[CPK17_Delete]
		@Id [bigint]
       ,@RowVersion [rowversion]
       ,@ModifiedBy nvarchar(256)
AS
BEGIN
		SET NOCOUNT ON;
		
		DECLARE @Context varbinary(128) = CAST(CAST(ISNULL(@ModifiedBy, SUSER_SNAME()) AS char(256)) AS varbinary(256));
		
		DELETE 
		  FROM [dbo].CPK17
		 WHERE CPK17.Id = @Id
		   AND CPK17.[RowVersion] = @RowVersion

		DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();
		
		SET CONTEXT_INFO 0x;
		
		IF @RowCountBig = 0
		BEGIN
            DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
													   ,N'Concurrency problem. '
                                                       ,N'The CPK17-row with Id=', @Id, N' and RowVersion=', CAST(@RowVersion AS binary(8)), N' cannot be altered by ', @ModifiedBy, N'. '
                                                       ,N'The row was already modified or deleted by a different user.'
                                                        );
			THROW 55501, @message, 1;
		  END;
END;