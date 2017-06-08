-- ================================================================================
-- T a b e l s
-- ================================================================================
CREATE TABLE [dbo].[ChildCollectionObjectDac] (
    [Id]                          bigint             IDENTITY (1, 1) NOT NULL
   ,[RowVersion]                  rowversion         NOT NULL
   ,DataId bigint NOT NULL
   ,Roltype tinyint NOT NULL
   ,RelatieId bigint NULL
   ,[ModifiedBy]                  nvarchar (256)     NOT NULL 
    CONSTRAINT [DF_ChildCollectionObjectDac_ModifiedBy] DEFAULT SUSER_SNAME()
   ,[ModifiedOn]                  DATETIME2(7)       NOT NULL
    CONSTRAINT [DF_ChildCollectionObjectDac_ModifiedOn] DEFAULT SYSDATETIME()
   ,CONSTRAINT [PK_ChildCollectionObjectDac] PRIMARY KEY CLUSTERED (Id ASC)
);
GO
GO
GO
CREATE TABLE [dbo].[ChildCollectionObjectDac_historie] (
    [Id]                          bigint             NOT NULL
   ,[RowVersion]                  binary(8)          NOT NULL
   ,DataId bigint NOT NULL
   ,Roltype tinyint NOT NULL
   ,RelatieId bigint NULL
   ,[ModifiedBy]                  nvarchar (256)     NOT NULL
   ,[ModifiedOn]                  DATETIME2(7)       NOT NULL
   ,DeletedBy                     nvarchar(256)     NULL
   ,DeletedAt                     datetime2(7)      NULL
);
GO

-- ================================================================================
-- I n d e x e s
-- ================================================================================
CREATE NONCLUSTERED INDEX IX_ChildCollectionObjectDac_historie_Id
                       ON [dbo].ChildCollectionObjectDac_historie
                         (Id ASC)
                  INCLUDE(ModifiedOn);
GO
-- ================================================================================
-- T r i g g e r s
-- ================================================================================
CREATE TRIGGER [dbo].tu_ChildCollectionObjectDac
            ON [dbo].ChildCollectionObjectDac
           FOR UPDATE
AS
BEGIN
     INSERT
       INTO [dbo].ChildCollectionObjectDac_historie
           (Id
           ,[RowVersion]
           ,DataId
           ,Roltype
           ,RelatieId
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,DeletedBy
           ,DeletedAt
           )
     SELECT DELETED.Id
           ,DELETED.[RowVersion]
           ,DataId
           ,Roltype
           ,RelatieId
           ,Deleted.ModifiedBy
           ,Deleted.ModifiedOn
           ,NULL
           ,NULL
       FROM Deleted;
END;
GO
CREATE TRIGGER [dbo].td_ChildCollectionObjectDac
            ON [dbo].ChildCollectionObjectDac
           FOR DELETE
AS
BEGIN
     INSERT
       INTO [dbo].ChildCollectionObjectDac_historie
           (Id
           ,[RowVersion]
           ,DataId
           ,Roltype
           ,RelatieId
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[DeletedBy]
           ,[DeletedAt]
           )
     SELECT Deleted.Id
           ,Deleted.[RowVersion]
           ,DataId
           ,Roltype
           ,RelatieId
           ,Deleted.ModifiedBy
           ,Deleted.ModifiedOn
           ,ISNULL(LTRIM(RTRIM(CONVERT(nvarchar(128), CONTEXT_INFO()))), SUSER_SNAME())
           ,SYSDATETIME()
       FROM Deleted;
END;
GO

