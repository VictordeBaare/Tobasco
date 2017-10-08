CREATE TABLE [dbo].[ChildCollectionObject](
	 [Id]			bigint	IDENTITY (1,1)  NOT NULL
	,[RowVersion]   rowversion         		NOT NULL
	,[Uid]          uniqueidentifier        NOT NULL CONSTRAINT [DF_{ChildCollectionObject}_UId] DEFAULT NEWID()
	,TestChildProp1 money NOT NULL
,TestChildProp2 smallmoney NOT NULL
,FileMetOverervingId bigint NOT NULL
	,[ModifiedBy]	nvarchar(256)			NOT NULL
	 CONSTRAINT [DF_{ChildCollectionObject}_ModifiedBy] DEFAULT SUSER_SNAME()
	,[ModifiedOn]	datetime2(7)			NOT NULL
	 CONSTRAINT [DF_{ChildCollectionObject}_ModifiedOn] DEFAULT SYSDATETIME()
	 CONSTRAINT [PK_{ChildCollectionObject}] PRIMARY KEY CLUSTERED (Id ASC)
);
GO
CREATE TABLE [dbo].[ChildCollectionObject_historie] (
    [Id]                          bigint             NOT NULL
   ,[RowVersion]                  binary(8)          NOT NULL
   ,[Uid]                         uniqueidentifier   NOT NULL
   ,TestChildProp1 money NOT NULL
,TestChildProp2 smallmoney NOT NULL
,FileMetOverervingId bigint NOT NULL   
   ,[ModifiedBy]                  nvarchar (256)     NOT NULL
   ,[ModifiedOn]                  DATETIME2(7)       NOT NULL
   ,DeletedBy                     nvarchar(256)      NULL
   ,DeletedAt                     datetime2(7)       NULL
);
GO
-- ================================================================================
-- I n d e x e s
-- ================================================================================

CREATE NONCLUSTERED INDEX IX_ChildCollectionObject_historie_Id
                       ON [dbo].ChildCollectionObject_historie
                         (Id ASC)
                  INCLUDE(ModifiedOn);
GO
CREATE NONCLUSTERED INDEX IX_UQ_ChildCollectionObject_Uid
                       ON [dbo].ChildCollectionObject
                         ([Uid] ASC
                         )
GO
CREATE NONCLUSTERED INDEX IX_ChildCollectionObject_FileMetOverervingId ON [dbo].[ChildCollectionObject] (FileMetOverervingId ASC)
GO

GO
-- ================================================================================
-- T r i g g e r s
-- ================================================================================

CREATE TRIGGER [dbo].tu_ChildCollectionObject
            ON [dbo].ChildCollectionObject
           FOR UPDATE
AS
BEGIN
    INSERT
      INTO [dbo].ChildCollectionObject_historie(
			Id,
		    [RowVersion],
		   TestChildProp1,
TestChildProp2,
FileMetOverervingId,
            [ModifiedBy],
            [ModifiedOn],
            DeletedBy,
            DeletedAt
           )
    SELECT DELETED.Id,
           DELETED.[RowVersion],
		  Deleted.TestChildProp1,
Deleted.TestChildProp2,
Deleted.FileMetOverervingId,
           Deleted.ModifiedBy,
           Deleted.ModifiedOn,
           NULL,
           NULL
      FROM Deleted;
END;
GO
CREATE TRIGGER [dbo].td_ChildCollectionObject
            ON [dbo].ChildCollectionObject
		   FOR DELETE
AS
BEGIN
	INSERT
	  INTO [dbo].ChildCollectionObject_historie(
			Id,
		    [RowVersion],
           TestChildProp1,
TestChildProp2,
FileMetOverervingId,
		    [ModifiedBy],
		    [ModifiedOn],
		    [DeletedBy],
		    [DeletedAt]
            )
	SELECT Deleted.Id,
	       Deleted.[RowVersion],
		  Deleted.TestChildProp1,
Deleted.TestChildProp2,
Deleted.FileMetOverervingId,
		   Deleted.ModifiedBy,
		   Deleted.ModifiedOn,
		   ISNULL(LTRIM(RTRIM(CONVERT(nvarchar(128), CONTEXT_INFO()))), SUSER_SNAME()),
		   SYSDATETIME()
	  FROM Deleted;
END;

