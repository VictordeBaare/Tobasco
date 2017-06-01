CREATE TABLE [dbo].[FileMetOvererving] (
    [Id]                          bigint             IDENTITY (1, 1) NOT NULL
   ,[RowVersion]                  rowversion         NOT NULL
   ,TestChildProp1 nvarchar(100) NOT NULL
   ,TestChildProp2 int NULL
   ,TestChildProp3 bigint NULL
   ,TestChildProp4 datetime2(7) NULL
   ,TestChildProp5 tinyint NULL
   ,TestChildProp6 decimal(12,2) NULL
   ,TestChildProp7Id bigint NULL
   ,[ModifiedBy]                  nvarchar (256)     NOT NULL 
    CONSTRAINT [DF_FileMetOvererving_ModifiedBy] DEFAULT SUSER_SNAME()
   ,[ModifiedOn]                  DATETIME2(7)       NOT NULL
    CONSTRAINT [DF_FileMetOvererving_ModifiedOn] DEFAULT SYSDATETIME()
   ,CONSTRAINT [PK_FileMetOvererving] PRIMARY KEY CLUSTERED (Id ASC)
   ,CONSTRAINT [FK_FileMetOvererving_ChildObject_Id] FOREIGN KEY (TestChildProp7Id) REFERENCES [dbo].[ChildObject] ([Id])
);
GO
GO
GO
CREATE TABLE [dbo].[FileMetOvererving_historie] (
    [Id]                          bigint             NOT NULL
   ,[RowVersion]                  binary(8)          NOT NULL
   ,TestChildProp1 nvarchar(100) NOT NULL
   ,TestChildProp2 int NULL
   ,TestChildProp3 bigint NULL
   ,TestChildProp4 datetime2(7) NULL
   ,TestChildProp5 tinyint NULL
   ,TestChildProp6 decimal(12,2) NULL
   ,TestChildProp7Id bigint NULL
   ,[ModifiedBy]                  nvarchar (256)     NOT NULL
   ,[ModifiedOn]                  DATETIME2(7)       NOT NULL
   ,DeletedBy                     nvarchar(256)     NULL
   ,DeletedAt                     datetime2(7)      NULL
);
GO

CREATE NONCLUSTERED INDEX IX_FileMetOvererving_historie_Id
                       ON [dbo].FileMetOvererving_historie
                         (Id ASC)
                  INCLUDE(ModifiedOn);
GO
CREATE TRIGGER [dbo].tu_FileMetOvererving
            ON [dbo].FileMetOvererving
           FOR UPDATE
AS
BEGIN
     INSERT
       INTO [dbo].FileMetOvererving_historie
           (Id
           ,[RowVersion]
           ,TestChildProp1
           ,TestChildProp2
           ,TestChildProp3
           ,TestChildProp4
           ,TestChildProp5
           ,TestChildProp6
           ,TestChildProp7Id
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,DeletedBy
           ,DeletedAt
           )
     SELECT DELETED.Id
           ,DELETED.[RowVersion]
           ,TestChildProp1
           ,TestChildProp2
           ,TestChildProp3
           ,TestChildProp4
           ,TestChildProp5
           ,TestChildProp6
           ,TestChildProp7Id
           ,Deleted.ModifiedBy
           ,Deleted.ModifiedOn
           ,NULL
           ,NULL
       FROM Deleted;
END;
GO
CREATE TRIGGER [dbo].td_FileMetOvererving
            ON [dbo].FileMetOvererving
           FOR DELETE
AS
BEGIN
     INSERT
       INTO [dbo].FileMetOvererving_historie
           (Id
           ,[RowVersion]
           ,TestChildProp1
           ,TestChildProp2
           ,TestChildProp3
           ,TestChildProp4
           ,TestChildProp5
           ,TestChildProp6
           ,TestChildProp7Id
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[DeletedBy]
           ,[DeletedAt]
           )
     SELECT Deleted.Id
           ,Deleted.[RowVersion]
           ,TestChildProp1
           ,TestChildProp2
           ,TestChildProp3
           ,TestChildProp4
           ,TestChildProp5
           ,TestChildProp6
           ,TestChildProp7Id
           ,Deleted.ModifiedBy
           ,Deleted.ModifiedOn
           ,ISNULL(LTRIM(RTRIM(CONVERT(nvarchar(128), CONTEXT_INFO()))), SUSER_SNAME())
           ,SYSDATETIME()
       FROM Deleted;
END;
GO

