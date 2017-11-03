CREATE PROCEDURE [dbo].[FileMetOvererving_Insert]
    @Id bigint output,
	@TestChildProp1 nvarchar(100),
@TestChildProp2 int,
@TestChildProp3 bigint,
@TestChildProp4 datetime2(7),
@TestChildProp5 tinyint,
@TestChildProp6 decimal(12,2),
    @ModifiedBy nvarchar(256)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT
      INTO [dbo].FileMetOvererving 
	       (
			TestChildProp1,
TestChildProp2,
TestChildProp3,
TestChildProp4,
TestChildProp5,
TestChildProp6,
			[ModifiedBy],
		    [ModifiedOn]
		   )
	OUTPUT Inserted.Id
              ,Inserted.[Uid]
		  ,Inserted.[RowVersion]
		  ,Inserted.ModifiedOn
    VALUES
         (
		   @TestChildProp1,
@TestChildProp2,
@TestChildProp3,
@TestChildProp4,
@TestChildProp5,
@TestChildProp6,
           @ModifiedBy,
           SYSDATETIME()
          );
END;
GO
CREATE PROCEDURE [dbo].[FileMetOvererving_Update]
		@Id [bigint],
		@TestChildProp1 nvarchar(100),
@TestChildProp2 int,
@TestChildProp3 bigint,
@TestChildProp4 datetime2(7),
@TestChildProp5 tinyint,
@TestChildProp6 decimal(12,2),
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

		UPDATE [dbo].FileMetOvererving
		   SET 
				FileMetOvererving.TestChildProp1 = @TestChildProp1,
FileMetOvererving.TestChildProp2 = @TestChildProp2,
FileMetOvererving.TestChildProp3 = @TestChildProp3,
FileMetOvererving.TestChildProp4 = @TestChildProp4,
FileMetOvererving.TestChildProp5 = @TestChildProp5,
FileMetOvererving.TestChildProp6 = @TestChildProp6,            
			    FileMetOvererving.ModifiedBy = ISNULL(@ModifiedBy, SUSER_SNAME()),
			    FileMetOvererving.ModifiedOn = SYSDATETIME()
		OUTPUT Inserted.[RowVersion]
			  ,Inserted.ModifiedOn
		  INTO #Output ([RowVersion]
						,ModifiedOn
					   )
		 WHERE FileMetOvererving.Id = @Id
		   AND FileMetOvererving.[RowVersion] = @RowVersion

		DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();

		IF @RowCountBig = 0
		BEGIN
            DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
													   ,N'Concurrency problem. '
                                                       ,N'The FileMetOvererving-row with Id=', @Id, N' and RowVersion=', CAST(@RowVersion AS binary(8)), N' cannot be altered by ', @ModifiedBy, N'. '
                                                       ,N'The row was already modified by a different user.'
                                                        );
			THROW 55501, @message, 1;
		  END;

		SELECT #Output.[RowVersion]
			  ,#Output.ModifiedOn
          FROM #Output;
END;
GO
CREATE PROCEDURE [dbo].[FileMetOvererving_Delete]
		@Id [bigint]
       ,@RowVersion [rowversion]
       ,@ModifiedBy nvarchar(256)
AS
BEGIN
		SET NOCOUNT ON;
		
		DECLARE @Context varbinary(128) = CAST(CAST(ISNULL(@ModifiedBy, SUSER_SNAME()) AS char(256)) AS varbinary(256));
		
		DELETE 
		  FROM [dbo].FileMetOvererving
		 WHERE FileMetOvererving.Id = @Id
		   AND FileMetOvererving.[RowVersion] = @RowVersion

		DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();
		
		SET CONTEXT_INFO 0x;
		
		IF @RowCountBig = 0
		BEGIN
            DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
													   ,N'Concurrency problem. '
                                                       ,N'The FileMetOvererving-row with Id=', @Id, N' and RowVersion=', CAST(@RowVersion AS binary(8)), N' cannot be altered by ', @ModifiedBy, N'. '
                                                       ,N'The row was already modified or deleted by a different user.'
                                                        );
			THROW 55501, @message, 1;
		  END;
END;
GO
CREATE PROCEDURE [dbo].FileMetOvererving_GetFullById
	@id bigint
AS
BEGIN

	EXEC ChildCollectionObject_GetFullByFileMetOvererving

	SELECT Id,
		   [RowVersion],
           TestChildProp1,
TestChildProp2,
TestChildProp3,
TestChildProp4,
TestChildProp5,
TestChildProp6,
		   [ModifiedBy],
		   [ModifiedOn]
	  FROM FileMetOvererving
	 WHERE FileMetOvererving.Id = @id;
END;
GO
CREATE TYPE [dbo].[FileMetOverervingMergeType] AS TABLE(
	 [Id] [bigint] NULL	
	,[RowVersion] [varbinary](8) NULL
	,TestChildProp1 nvarchar(100) NOT NULL
,TestChildProp2 int NULL
,TestChildProp3 bigint NULL
,TestChildProp4 datetime2(7) NULL
,TestChildProp5 tinyint NULL
,TestChildProp6 decimal(12,2) NULL
	,[Delete_Flag] [bit] NULL
	,[Id_Intern] [bigint] NOT NULL
)
GO
CREATE PROCEDURE [dbo].[FileMetOvererving_Merge]
		@DataTable [dbo].[FileMetOverervingMergeType] READONLY,
        @ModifiedBy nvarchar(256),
		@ModifiedOn datetime2(7),    
		@AbortOnConcurrency bit
AS
BEGIN
		SET NOCOUNT ON;
		
		IF OBJECT_ID('tempdb..#output') IS NOT NULL
		BEGIN
			DROP TABLE #output
		END
		
		CREATE TABLE #output 
					(Id                   int           NOT NULL PRIMARY KEY
	                 ,[RowVersion]         binary(8)     NOT NULL
					 ,TestChildProp1 nvarchar(100) NOT NULL
,TestChildProp2 int NULL
,TestChildProp3 bigint NULL
,TestChildProp4 datetime2(7) NULL
,TestChildProp5 tinyint NULL
,TestChildProp6 decimal(12,2) NULL
                     ,Id_Intern           bigint        NULL
                     ,MergeAction         nvarchar(10)  NOT NULL -- 'INSERT', 'UPDATE', or 'DELETE'
                     ,WasDeleted          bit           NOT NULL
                     ,WasUpdated          bit           NOT NULL
                     )

	MERGE
     INTO [dbo].[FileMetOvererving] WITH(HOLDLOCK) AS [Target]
    USING @DataTable AS [Source]
       ON [Source].Id = [Target].Id
	 WHEN MATCHED
	  AND ISNULL([Source].Delete_Flag, 'FALSE') = 'FALSE'
		  THEN UPDATE
				  SET [Target].TestChildProp1 = [Source].TestChildProp1,
[Target].TestChildProp2 = [Source].TestChildProp2,
[Target].TestChildProp3 = [Source].TestChildProp3,
[Target].TestChildProp4 = [Source].TestChildProp4,
[Target].TestChildProp5 = [Source].TestChildProp5,
[Target].TestChildProp6 = [Source].TestChildProp6,
					  ModifiedBy     = @ModifiedBy,
                      ModifiedOn     = @ModifiedOn
	 WHEN MATCHED
      AND [Source].Delete_Flag = 'TRUE'
          THEN DELETE
	 WHEN NOT MATCHED BY TARGET
	      THEN INSERT
					 (
					 TestChildProp1,
TestChildProp2,
TestChildProp3,
TestChildProp4,
TestChildProp5,
TestChildProp6,
					 ModifiedBy,
					 ModifiedOn
					 )
			   VALUES
					 (
					 [Source].TestChildProp1,
[Source].TestChildProp2,
[Source].TestChildProp3,
[Source].TestChildProp4,
[Source].TestChildProp5,
[Source].TestChildProp6,
					 @ModifiedBy,
                     @ModifiedOn					 
					 )	 
	OUTPUT IIF($action = 'DELETE', deleted.Id, inserted.Id)
	      ,IIF($action = 'DELETE', deleted.[RowVersion], inserted.[RowVersion])
		  ,IIF($action = 'DELETE', deleted.TestChildProp1, inserted.TestChildProp1)
,IIF($action = 'DELETE', deleted.TestChildProp2, inserted.TestChildProp2)
,IIF($action = 'DELETE', deleted.TestChildProp3, inserted.TestChildProp3)
,IIF($action = 'DELETE', deleted.TestChildProp4, inserted.TestChildProp4)
,IIF($action = 'DELETE', deleted.TestChildProp5, inserted.TestChildProp5)
,IIF($action = 'DELETE', deleted.TestChildProp6, inserted.TestChildProp6)
		  ,IIF($action = 'DELETE', deleted.ModifiedBy, inserted.ModifiedBy)
          ,IIF($action = 'DELETE', deleted.ModifiedOn, inserted.ModifiedOn)
		  ,[Source].Id_Intern
          ,$action -- 'INSERT', 'UPDATE', or 'DELETE'
          ,IIF($action = 'INSERT' AND [Source].Id != 0, CAST('TRUE' AS bit), CAST('FALSE' AS bit))
          ,IIF($action IN ('UPDATE','DELETE') AND [Source].[RowVersion] <> Deleted.[RowVersion], CAST('TRUE' AS bit), CAST('FALSE' AS bit))
	  INTO #output
		   (Id,
	       [RowVersion],
			TestChildProp1,
TestChildProp2,
TestChildProp3,
TestChildProp4,
TestChildProp5,
TestChildProp6,   
            Id_Intern,
            MergeAction,
            WasDeleted,
            WasUpdated
           );
		   
IF @@ERROR <> 0
BEGIN
    RETURN
END

SELECT #output.Id,
       #output.[RowVersion],
	   #output.TestChildProp1,
#output.TestChildProp2,
#output.TestChildProp3,
#output.TestChildProp4,
#output.TestChildProp5,
#output.TestChildProp6,
       #output.Id_Intern,
       #output.MergeAction,
       #output.WasDeleted,
       #output.WasUpdated
  FROM #output
	
-- ================================================================================
-- R E S U L T A T E N   V E R I F I Ë R E N
-- ================================================================================

IF EXISTS (SELECT 1
             FROM #output
            WHERE #output.WasDeleted = 'TRUE'
               OR #output.WasUpdated = 'TRUE'
          )
AND @AbortOnConcurrency = 'TRUE'
BEGIN
    RAISERROR('Concurrency problem. One or multiple rows have a different version', 16, 1)
END
	
IF OBJECT_ID('tempdb..#output') IS NOT NULL
BEGIN
    DROP TABLE #output
END

END