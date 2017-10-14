CREATE PROCEDURE [dbo].[CPK_Insert]
    @Id bigint output,
	@Training nvarchar(100),
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
Duur,
Kosten,
			[ModifiedBy],
		    [ModifiedOn]
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
           SYSDATETIME()
          );
END;
GO
CREATE PROCEDURE [dbo].[CPK_Update]
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

		UPDATE [dbo].CPK
		   SET 
				CPK.Training = @Training,
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

	

	

	

	SELECT Id,
		   [RowVersion],
           Duur,
Kosten,
Training,
		   [ModifiedBy],
		   [ModifiedOn]
	  FROM CPK
	 WHERE CPK.Id = @id;
END;
GO
CREATE TYPE [dbo].[CPKMergeType] AS TABLE(
	 [Id] [bigint] NULL	
	,[RowVersion] [varbinary](8) NULL
	,Training nvarchar(100) NOT NULL
,Duur nvarchar(100) NOT NULL
,Kosten nvarchar(100) NOT NULL
	,[Delete_Flag] [bit] NULL
	,[Id_Intern] [bigint] NOT NULL
)
GO
CREATE PROCEDURE [dbo].[CPK_Merge]
		@DataTable [dbo].[CPKMergeType] READONLY,
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
					 ,Training nvarchar(100) NOT NULL
,Duur nvarchar(100) NOT NULL
,Kosten nvarchar(100) NOT NULL
                     ,Id_Intern           bigint        NULL
                     ,MergeAction         nvarchar(10)  NOT NULL -- 'INSERT', 'UPDATE', or 'DELETE'
                     ,WasDeleted          bit           NOT NULL
                     ,WasUpdated          bit           NOT NULL
                     )

	MERGE
     INTO [dbo].[CPK] WITH(HOLDLOCK) AS [Target]
    USING @DataTable AS [Source]
       ON [Source].Id = [Target].Id
	 WHEN MATCHED
	  AND ISNULL([Source].Delete_Flag, 'FALSE') = 'FALSE'
		  THEN UPDATE
				  SET [Target].Training = [Source].Training,
[Target].Duur = [Source].Duur,
[Target].Kosten = [Source].Kosten,
					  ModifiedBy     = @ModifiedBy,
                      ModifiedOn     = @ModifiedOn
	 WHEN MATCHED
      AND [Source].Delete_Flag = 'TRUE'
          THEN DELETE
	 WHEN NOT MATCHED BY TARGET
	      THEN INSERT
					 (
					 Training,
Duur,
Kosten,
					 ModifiedBy,
					 ModifiedOn
					 )
			   VALUES
					 (
					 [Source].Training,
[Source].Duur,
[Source].Kosten,
					 @ModifiedBy,
                     @ModifiedOn					 
					 )	 
	OUTPUT IIF($action = 'DELETE', deleted.Id, inserted.Id)
	      ,IIF($action = 'DELETE', deleted.[RowVersion], inserted.[RowVersion])
		  ,IIF($action = 'DELETE', deleted.Training, inserted.Training)
,IIF($action = 'DELETE', deleted.Duur, inserted.Duur)
,IIF($action = 'DELETE', deleted.Kosten, inserted.Kosten)
		  ,IIF($action = 'DELETE', deleted.ModifiedBy, inserted.ModifiedBy)
          ,IIF($action = 'DELETE', deleted.ModifiedOn, inserted.ModifiedOn)
		  ,[Source].Id_Intern
          ,$action -- 'INSERT', 'UPDATE', or 'DELETE'
          ,IIF($action = 'INSERT' AND [Source].Id != 0, CAST('TRUE' AS bit), CAST('FALSE' AS bit))
          ,IIF($action IN ('UPDATE','DELETE') AND [Source].[RowVersion] <> Deleted.[RowVersion], CAST('TRUE' AS bit), CAST('FALSE' AS bit))
	  INTO #output
		   (Id,
	       [RowVersion],
			Training,
Duur,
Kosten,   
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
	   #output.Training,
#output.Duur,
#output.Kosten,
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