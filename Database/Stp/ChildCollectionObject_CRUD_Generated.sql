CREATE PROCEDURE [dbo].[ChildCollectionObject_Insert]
	@Id bigint output,
@TestChildProp1 money,
@TestChildProp2 smallmoney,
@FileMetOverervingId bigint,
@ModifiedBy nvarchar(256)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT
      INTO [dbo].ChildCollectionObject 
	       (
			TestChildProp1,
TestChildProp2,
FileMetOverervingId,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC
		   )
	OUTPUT Inserted.Id
              ,Inserted.[UId]
		  ,Inserted.[RowVersion]
		  ,Inserted.ModifiedOn
    VALUES
         (
		   @TestChildProp1,
@TestChildProp2,
@FileMetOverervingId,
@ModifiedBy,
SYSDATETIME(),
SYSUTCDATETIME()
          );
END;
GO
CREATE PROCEDURE [dbo].[ChildCollectionObject_Update]
		@Id [bigint],
@TestChildProp1 money,
@TestChildProp2 smallmoney,
@FileMetOverervingId bigint,
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

		UPDATE [dbo].ChildCollectionObject
		   SET 
				ChildCollectionObject.TestChildProp1 = @TestChildProp1,
ChildCollectionObject.TestChildProp2 = @TestChildProp2,
ChildCollectionObject.FileMetOverervingId = @FileMetOverervingId,
ChildCollectionObject.ModifiedBy = ISNULL(@ModifiedBy, SUSER_SNAME()),
ChildCollectionObject.ModifiedOn = SYSDATETIME()            
		OUTPUT Inserted.[RowVersion]
			  ,Inserted.ModifiedOn
		  INTO #Output ([RowVersion]
						,ModifiedOn
					   )
		 WHERE ChildCollectionObject.Id = @Id
		   AND ChildCollectionObject.[RowVersion] = @RowVersion

		DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();

		IF @RowCountBig = 0
		BEGIN
            DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
													   ,N'Concurrency problem. '
                                                       ,N'The ChildCollectionObject-row with Id=', @Id, N' and RowVersion=', CAST(@RowVersion AS binary(8)), N' cannot be altered by ', @ModifiedBy, N'. '
                                                       ,N'The row was already modified by a different user.'
                                                        );
			THROW 55501, @message, 1;
		  END;

		SELECT #Output.[RowVersion]
			  ,#Output.ModifiedOn
          FROM #Output;
END;
GO
CREATE PROCEDURE [dbo].[ChildCollectionObject_Delete]
		@Id [bigint]
       ,@RowVersion [rowversion]
       ,@ModifiedBy nvarchar(256)
AS
BEGIN
		SET NOCOUNT ON;
		
		DECLARE @Context varbinary(128) = CAST(CAST(ISNULL(@ModifiedBy, SUSER_SNAME()) AS char(256)) AS varbinary(256));
		
		DELETE 
		  FROM [dbo].ChildCollectionObject
		 WHERE ChildCollectionObject.Id = @Id
		   AND ChildCollectionObject.[RowVersion] = @RowVersion

		DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();
		
		SET CONTEXT_INFO 0x;
		
		IF @RowCountBig = 0
		BEGIN
            DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
													   ,N'Concurrency problem. '
                                                       ,N'The ChildCollectionObject-row with Id=', @Id, N' and RowVersion=', CAST(@RowVersion AS binary(8)), N' cannot be altered by ', @ModifiedBy, N'. '
                                                       ,N'The row was already modified or deleted by a different user.'
                                                        );
			THROW 55501, @message, 1;
		  END;
END;