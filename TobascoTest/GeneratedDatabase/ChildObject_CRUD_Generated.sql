CREATE PROCEDURE [dbo].[ChildObject_Insert]
    @Id bigint output,
    @TestChildProp1 nvarchar(100),
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
    @TestChildProp1 nvarchar(100),
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
                          ,ModifiedOn       datetime2(7) NOT NULL
                          );

     UPDATE [dbo].ChildObject
        SET
            ChildObject.TestChildProp1 = @TestChildProp1,
            ChildObject.ModifiedBy = ISNULL(@ModifiedBy, SUSER_SNAME()),
            ChildObject.ModifiedOn = SYSDATETIME()
     OUTPUT Inserted.[RowVersion]
           ,Inserted.ModifiedOn
       INTO #Output
           ([RowVersion]
           ,ModifiedOn
           )
      WHERE ChildObject.Id = @Id
        AND ChildObject.[RowVersion] = @RowVersion

     DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();

     IF @RowCountBig = 0
     BEGIN
         DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
                                                    ,N'Concurrency probleem. '
                                                    ,N'De ChildObject-rij met Id=', @Id, N' en RowVersion=', CAST(@RowVersion AS binary(8)), N' kon niet gewijzigd worden door ', @ModifiedBy, N'. '
                                                    ,N'De rij was tussendoor gewijzigd of verwijderd door een andere gebruiker.'
                                                    );
         THROW 55501, @message, 1;
     END;

     SELECT #Output.[RowVersion]
           ,#Output.ModifiedOn
       FROM #Output;
END;
GO

CREATE PROCEDURE [dbo].[ChildObject_Delete]
    @Id [bigint],
    @RowVersion rowversion,
    @ModifiedBy nvarchar(256)
AS
BEGIN
     SET NOCOUNT ON;

     DECLARE @Context varbinary(128) = CAST(CAST(ISNULL(@ModifiedBy, SUSER_SNAME()) AS char(256)) AS varbinary(256));
     SET CONTEXT_INFO @Context;

     DELETE
       FROM [dbo].ChildObject
      WHERE ChildObject.Id = @Id
        AND ChildObject.[RowVersion] = @RowVersion;

     DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();

     SET CONTEXT_INFO 0x;

     IF @RowCountBig = 0
     BEGIN
         DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '
                                                    ,N'Concurrency probleem. '
                                                    ,N'De ChildObject-rij met Id=', @Id, N' en RowVersion=', CAST(@RowVersion AS binary(8)), N' kon niet verwijderd worden door ', @ModifiedBy, N'. '
                                                    ,N'De rij was tussendoor gewijzigd of verwijderd door een andere gebruiker.'
                                                    );
         THROW 55501, @message, 1;
     END;

END;
GO

