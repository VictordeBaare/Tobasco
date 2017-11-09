CREATE PROCEDURE [dbo].[ChildObject_Insert]
    @Id bigint output,
	@TestChildProp1 varchar(100),
    @ModifiedBy nvarchar(256)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT
      INTO [dbo].ChildObject 
	       (
			TestChildProp1,
			[ModifiedBy],
		    [ModifiedOn]
		   )
	OUTPUT Inserted.Id
		  ,Inserted.[RowVersion]
		  ,Inserted.ModifiedOn
    VALUES
         (
		   @TestChildProp1,
           @ModifiedBy,
           SYSDATETIME()
          );
END;
GO
CREATE PROCEDURE [dbo].[ChildObject_Update]
		@Id [bigint],
		@TestChildProp1 varchar(100),
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

		UPDATE [dbo].ChildObject
		   SET 
				ChildObject.TestChildProp1 = @TestChildProp1,            
			    ChildObject.ModifiedBy = ISNULL(@ModifiedBy, SUSER_SNAME()),
			    ChildObject.ModifiedOn = SYSDATETIME()
		OUTPUT Inserted.[RowVersion]
			  ,Inserted.ModifiedOn
		  INTO #Output ([RowVersion]
						,ModifiedOn
					   )
		 WHERE ChildObject.Id = @Id
		   AND ChildObject.[RowVersion] = @RowVersion

		DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();

		IF @RowCountBig = 0
		BEGIN
            DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
													   ,N'Concurrency problem. '
                                                       ,N'The ChildObject-row with Id=', @Id, N' and RowVersion=', CAST(@RowVersion AS binary(8)), N' cannot be altered by ', @ModifiedBy, N'. '
                                                       ,N'The row was already modified by a different user.'
                                                        );
			THROW 55501, @message, 1;
		  END;

		SELECT #Output.[RowVersion]
			  ,#Output.ModifiedOn
          FROM #Output;
END;
GO
CREATE PROCEDURE [dbo].[ChildObject_Delete]
		@Id [bigint]
       ,@RowVersion [rowversion]
       ,@ModifiedBy nvarchar(256)
AS
BEGIN
		SET NOCOUNT ON;
		
		DECLARE @Context varbinary(128) = CAST(CAST(ISNULL(@ModifiedBy, SUSER_SNAME()) AS char(256)) AS varbinary(256));
		
		DELETE 
		  FROM [dbo].ChildObject
		 WHERE ChildObject.Id = @Id
		   AND ChildObject.[RowVersion] = @RowVersion

		DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();
		
		SET CONTEXT_INFO 0x;
		
		IF @RowCountBig = 0
		BEGIN
            DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
													   ,N'Concurrency problem. '
                                                       ,N'The ChildObject-row with Id=', @Id, N' and RowVersion=', CAST(@RowVersion AS binary(8)), N' cannot be altered by ', @ModifiedBy, N'. '
                                                       ,N'The row was already modified or deleted by a different user.'
                                                        );
			THROW 55501, @message, 1;
		  END;
END;
GO
CREATE PROCEDURE [dbo].ChildObject_GetFullById
	@id bigint
AS
BEGIN

	SELECT Id,
		   [RowVersion],
           TestChildProp1,
		   [ModifiedBy],
		   [ModifiedOn]
	  FROM ChildObject
	 WHERE ChildObject.Id = @id;
END;
GO
CREATE TYPE [dbo].[ChildObjectMergeType] AS TABLE(
	 [Id] [bigint] NULL	
	,[RowVersion] [varbinary](8) NULL
	,TestChildProp1 varchar(100) NOT NULL
	,[Delete_Flag] [bit] NULL
	,[Id_Intern] [bigint] NOT NULL
)
GO
CREATE PROCEDURE [dbo].[ChildObject_Merge]
		@DataTable [dbo].[ChildObjectMergeType] READONLY,
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
					 ,TestChildProp1 varchar(100) NOT NULL
                     ,Id_Intern           bigint        NULL
                     ,MergeAction         nvarchar(10)  NOT NULL -- 'INSERT', 'UPDATE', or 'DELETE'
                     ,WasDeleted          bit           NOT NULL
                     ,WasUpdated          bit           NOT NULL
                     )

	MERGE
     INTO [dbo].[ChildObject] WITH(HOLDLOCK) AS [Target]
    USING @DataTable AS [Source]
       ON [Source].Id = [Target].Id
	 WHEN MATCHED
	  AND ISNULL([Source].Delete_Flag, 'FALSE') = 'FALSE'
		  THEN UPDATE
				  SET [Target].TestChildProp1 = [Source].TestChildProp1,
					  ModifiedBy     = @ModifiedBy,
                      ModifiedOn     = @ModifiedOn
	 WHEN MATCHED
      AND [Source].Delete_Flag = 'TRUE'
          THEN DELETE
	 WHEN NOT MATCHED BY TARGET
	      THEN INSERT
					 (
					 TestChildProp1,
					 ModifiedBy,
					 ModifiedOn
					 )
			   VALUES
					 (
					 [Source].TestChildProp1,
					 @ModifiedBy,
                     @ModifiedOn					 
					 )	 
	OUTPUT IIF($action = 'DELETE', deleted.Id, inserted.Id)
	      ,IIF($action = 'DELETE', deleted.[RowVersion], inserted.[RowVersion])
		  ,IIF($action = 'DELETE', deleted.TestChildProp1, inserted.TestChildProp1)
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