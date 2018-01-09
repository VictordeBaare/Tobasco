CREATE TABLE [dbo].[ChildCollectionObject](
	 [Id]			bigint	IDENTITY (1,1)  NOT NULL
	,[RowVersion]   rowversion         		NOT NULL
	,[UId]          uniqueidentifier        NOT NULL CONSTRAINT [DF_{ChildCollectionObject}_UId] DEFAULT NEWID()
	,TestChildProp1 money NOT NULL
,TestChildProp2 smallmoney NOT NULL
,FileMetOverervingId bigint NOT NULL
	,[ModifiedBy]	nvarchar(256)			NOT NULL
	 CONSTRAINT [DF_ChildCollectionObject_ModifiedBy] DEFAULT SUSER_SNAME()
	,[ModifiedOn]	datetime2(7)			NOT NULL
	 CONSTRAINT [DF_ChildCollectionObject_ModifiedOn] DEFAULT SYSDATETIME()
 	,[ModifiedOnUTC] datetime2(7)			   NOT NULL
	 CONSTRAINT [DF_ChildCollectionObject_ModifiedOnUTC] DEFAULT SYSUTCDATETIME()
	,CONSTRAINT [PK_ChildCollectionObject] PRIMARY KEY CLUSTERED (Id ASC)
	 ,CONSTRAINT [FK_ChildCollectionObject_FileMetOverervingId] FOREIGN KEY (FileMetOverervingId) REFERENCES [dbo].[FileMetOvererving] ([Id])

);
GO
CREATE TABLE [dbo].[ChildCollectionObject_historie] (
    [Id]                          bigint             NOT NULL
   ,[RowVersion]                  binary(8)          NOT NULL
   ,[UId]                         uniqueidentifier   NOT NULL
   ,TestChildProp1 money NOT NULL
,TestChildProp2 smallmoney NOT NULL
,FileMetOverervingId bigint NOT NULL   
   ,[ModifiedBy]                  nvarchar (256)     NOT NULL
   ,[ModifiedOn]                  datetime2(7)       NOT NULL
   ,[ModifiedOnUTC] 			  datetime2(7)	     NOT NULL
   ,DeletedBy                     nvarchar(256)      NULL
   ,DeletedAt                     datetime2(7)       NULL
);
GO
CREATE NONCLUSTERED INDEX IX_ChildCollectionObject_historie_Id
                       ON [dbo].ChildCollectionObject_historie
                         (Id ASC)
                  INCLUDE(ModifiedOn);
GO
CREATE NONCLUSTERED INDEX IX_UQ_ChildCollectionObject_UId
                       ON [dbo].ChildCollectionObject
                         ([UId] ASC
                         )
GO
CREATE NONCLUSTERED INDEX IX_ChildCollectionObject_FileMetOverervingId ON [dbo].[ChildCollectionObject] (FileMetOverervingId ASC)
GO

GO
CREATE TRIGGER [dbo].tu_ChildCollectionObject
            ON [dbo].ChildCollectionObject
           FOR UPDATE
AS
BEGIN
    INSERT
      INTO [dbo].ChildCollectionObject_historie(
		   Id,
[UId],
[RowVersion],
TestChildProp1,
TestChildProp2,
FileMetOverervingId,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC,
DeletedBy,
DeletedAt
           )
    SELECT 
		  Deleted.Id,
Deleted.[UId],
Deleted.[rowversion],
Deleted.TestChildProp1,
Deleted.TestChildProp2,
Deleted.FileMetOverervingId,
Deleted.ModifiedBy,
Deleted.ModifiedOn,
Deleted.ModifiedOnUTC,
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
[UId],
[RowVersion],
TestChildProp1,
TestChildProp2,
FileMetOverervingId,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC,
DeletedBy,
DeletedAt
            )
	SELECT 
		  Deleted.Id,
Deleted.[UId],
Deleted.[rowversion],
Deleted.TestChildProp1,
Deleted.TestChildProp2,
Deleted.FileMetOverervingId,
Deleted.ModifiedBy,
Deleted.ModifiedOn,
Deleted.ModifiedOnUTC,
ISNULL(LTRIM(RTRIM(CONVERT(nvarchar(128), CONTEXT_INFO()))), SUSER_SNAME()),
SYSDATETIME()
	  FROM Deleted;
END;

GO
EXEC sp_addextendedproperty N'Description', 'ChildCollectionObject', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject', NULL, NULL
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp1', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject', 'COLUMN', N'TestChildProp1'
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp2', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject', 'COLUMN', N'TestChildProp2'
GO
EXEC sp_addextendedproperty N'Description', 'This is FileMetOverervingId', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject', 'COLUMN', N'FileMetOverervingId'
GO
EXEC sp_addextendedproperty N'Description', 'Primary key of the table, auto-incremented by 1 eacht time a row is added to the table.', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'Description', 'A data type that exposes automatically generated, unique binary numbers within a database. rowversion is generally used as a mechanism for version-stamping table rows. The storage size is 8 bytes. The rowversion data type is just an incrementing number and does not preserve a date or a time. Each database has a counter that is incremented for each insert or update operation that is performed on a table that contains a rowversion column within the database. This counter is the database rowversion. This tracks a relative time within a database, not an actual time that can be associated with a clock. A table can have only one rowversion column. Every time that a row with a rowversion column is modified or inserted, the incremented database rowversion value is inserted in the rowversion column. The rowversion value is incremented with any update statement, even if no row values are changed.', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject', 'COLUMN', N'Rowversion'
GO
EXEC sp_addextendedproperty N'Description', 'Generated Guid', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject', 'COLUMN', N'UId'
GO
EXEC sp_addextendedproperty N'Description', 'Local timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject', 'COLUMN', N'ModifiedOn'
GO
EXEC sp_addextendedproperty N'Description', 'Login name that made the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject', 'COLUMN', N'ModifiedBy'
GO
EXEC sp_addextendedproperty N'Description', 'UTC timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject', 'COLUMN', N'ModifiedOnUTC'
GO
GO
EXEC sp_addextendedproperty N'Description', 'ChildCollectionObject', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', NULL, NULL
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp1', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', 'COLUMN', N'TestChildProp1'
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp2', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', 'COLUMN', N'TestChildProp2'
GO
EXEC sp_addextendedproperty N'Description', 'This is FileMetOverervingId', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', 'COLUMN', N'FileMetOverervingId'
GO
EXEC sp_addextendedproperty N'Description', 'Primary key of the table, auto-incremented by 1 eacht time a row is added to the table.', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'Description', 'A data type that exposes automatically generated, unique binary numbers within a database. rowversion is generally used as a mechanism for version-stamping table rows. The storage size is 8 bytes. The rowversion data type is just an incrementing number and does not preserve a date or a time. Each database has a counter that is incremented for each insert or update operation that is performed on a table that contains a rowversion column within the database. This counter is the database rowversion. This tracks a relative time within a database, not an actual time that can be associated with a clock. A table can have only one rowversion column. Every time that a row with a rowversion column is modified or inserted, the incremented database rowversion value is inserted in the rowversion column. The rowversion value is incremented with any update statement, even if no row values are changed.', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', 'COLUMN', N'Rowversion'
GO
EXEC sp_addextendedproperty N'Description', 'Generated Guid', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', 'COLUMN', N'UId'
GO
EXEC sp_addextendedproperty N'Description', 'Local timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', 'COLUMN', N'ModifiedOn'
GO
EXEC sp_addextendedproperty N'Description', 'Login name that made the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', 'COLUMN', N'ModifiedBy'
GO
EXEC sp_addextendedproperty N'Description', 'UTC timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', 'COLUMN', N'ModifiedOnUTC'
GO
EXEC sp_addextendedproperty N'Description', 'Local timestamp that the row was deleted', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', 'COLUMN', N'DeletedAt'
GO
EXEC sp_addextendedproperty N'Description', 'Login name that deleted the row.', 'SCHEMA', N'dbo', 'TABLE', N'ChildCollectionObject_historie', 'COLUMN', N'DeletedBy'
GO
