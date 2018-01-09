CREATE TABLE [dbo].[FileMetOvererving](
	 [Id]			bigint	IDENTITY (1,1)  NOT NULL
	,[RowVersion]   rowversion         		NOT NULL
	,[UId]          uniqueidentifier        NOT NULL CONSTRAINT [DF_{FileMetOvererving}_UId] DEFAULT NEWID()
	,TestChildProp1 nvarchar(100) NOT NULL
,TestChildProp2 int NULL
,TestChildProp3 bigint NULL
,TestChildProp4 datetime2(7) NULL
,TestChildProp5 tinyint NULL
,TestChildProp6 decimal(12,2) NULL
,TestChildProp7Id bigint NULL
	,[ModifiedBy]	nvarchar(256)			NOT NULL
	 CONSTRAINT [DF_FileMetOvererving_ModifiedBy] DEFAULT SUSER_SNAME()
	,[ModifiedOn]	datetime2(7)			NOT NULL
	 CONSTRAINT [DF_FileMetOvererving_ModifiedOn] DEFAULT SYSDATETIME()
 	,[ModifiedOnUTC] datetime2(7)			   NOT NULL
	 CONSTRAINT [DF_FileMetOvererving_ModifiedOnUTC] DEFAULT SYSUTCDATETIME()
	,CONSTRAINT [PK_FileMetOvererving] PRIMARY KEY CLUSTERED (Id ASC)
	 ,CONSTRAINT [FK_FileMetOvererving_ChildObject_Id] FOREIGN KEY (TestChildProp7Id) REFERENCES [dbo].[ChildObject] ([Id])

);
GO
CREATE TABLE [dbo].[FileMetOvererving_historie] (
    [Id]                          bigint             NOT NULL
   ,[RowVersion]                  binary(8)          NOT NULL
   ,[UId]                         uniqueidentifier   NOT NULL
   ,TestChildProp1 nvarchar(100) NOT NULL
,TestChildProp2 int NULL
,TestChildProp3 bigint NULL
,TestChildProp4 datetime2(7) NULL
,TestChildProp5 tinyint NULL
,TestChildProp6 decimal(12,2) NULL
,TestChildProp7Id bigint NULL   
   ,[ModifiedBy]                  nvarchar (256)     NOT NULL
   ,[ModifiedOn]                  datetime2(7)       NOT NULL
   ,[ModifiedOnUTC] 			  datetime2(7)	     NOT NULL
   ,DeletedBy                     nvarchar(256)      NULL
   ,DeletedAt                     datetime2(7)       NULL
);
GO
CREATE NONCLUSTERED INDEX IX_FileMetOvererving_historie_Id
                       ON [dbo].FileMetOvererving_historie
                         (Id ASC)
                  INCLUDE(ModifiedOn);
GO
CREATE NONCLUSTERED INDEX IX_UQ_FileMetOvererving_UId
                       ON [dbo].FileMetOvererving
                         ([UId] ASC
                         )
GO

GO
CREATE TRIGGER [dbo].tu_FileMetOvererving
            ON [dbo].FileMetOvererving
           FOR UPDATE
AS
BEGIN
    INSERT
      INTO [dbo].FileMetOvererving_historie(
		   Id,
[UId],
[RowVersion],
TestChildProp1,
TestChildProp2,
TestChildProp3,
TestChildProp4,
TestChildProp5,
TestChildProp6,
TestChildProp7Id,
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
Deleted.TestChildProp3,
Deleted.TestChildProp4,
Deleted.TestChildProp5,
Deleted.TestChildProp6,
Deleted.TestChildProp7Id,
Deleted.ModifiedBy,
Deleted.ModifiedOn,
Deleted.ModifiedOnUTC,
NULL,
NULL
      FROM Deleted;
END;
GO
CREATE TRIGGER [dbo].td_FileMetOvererving
            ON [dbo].FileMetOvererving
		   FOR DELETE
AS
BEGIN
	INSERT
	  INTO [dbo].FileMetOvererving_historie(
           Id,
[UId],
[RowVersion],
TestChildProp1,
TestChildProp2,
TestChildProp3,
TestChildProp4,
TestChildProp5,
TestChildProp6,
TestChildProp7Id,
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
Deleted.TestChildProp3,
Deleted.TestChildProp4,
Deleted.TestChildProp5,
Deleted.TestChildProp6,
Deleted.TestChildProp7Id,
Deleted.ModifiedBy,
Deleted.ModifiedOn,
Deleted.ModifiedOnUTC,
ISNULL(LTRIM(RTRIM(CONVERT(nvarchar(128), CONTEXT_INFO()))), SUSER_SNAME()),
SYSDATETIME()
	  FROM Deleted;
END;

GO
EXEC sp_addextendedproperty N'Description', 'TestNaam', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', NULL, NULL
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp1', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'TestChildProp1'
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp2', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'TestChildProp2'
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp3', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'TestChildProp3'
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp4', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'TestChildProp4'
GO
EXEC sp_addextendedproperty N'Description', '
Enum values:
Name: Onbekend, value: 0
Name: Man, value: 1
Name: Vrouw, value: 2', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'TestChildProp5'
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp6', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'TestChildProp6'
GO
EXEC sp_addextendedproperty N'Description', 'TestColumn', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'TestChildProp7Id'
GO
EXEC sp_addextendedproperty N'Description', 'Primary key of the table, auto-incremented by 1 eacht time a row is added to the table.', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'Description', 'A data type that exposes automatically generated, unique binary numbers within a database. rowversion is generally used as a mechanism for version-stamping table rows. The storage size is 8 bytes. The rowversion data type is just an incrementing number and does not preserve a date or a time. Each database has a counter that is incremented for each insert or update operation that is performed on a table that contains a rowversion column within the database. This counter is the database rowversion. This tracks a relative time within a database, not an actual time that can be associated with a clock. A table can have only one rowversion column. Every time that a row with a rowversion column is modified or inserted, the incremented database rowversion value is inserted in the rowversion column. The rowversion value is incremented with any update statement, even if no row values are changed.', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'Rowversion'
GO
EXEC sp_addextendedproperty N'Description', 'Generated Guid', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'UId'
GO
EXEC sp_addextendedproperty N'Description', 'Local timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'ModifiedOn'
GO
EXEC sp_addextendedproperty N'Description', 'Login name that made the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'ModifiedBy'
GO
EXEC sp_addextendedproperty N'Description', 'UTC timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving', 'COLUMN', N'ModifiedOnUTC'
GO
GO
EXEC sp_addextendedproperty N'Description', 'TestNaam', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', NULL, NULL
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp1', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'TestChildProp1'
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp2', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'TestChildProp2'
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp3', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'TestChildProp3'
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp4', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'TestChildProp4'
GO
EXEC sp_addextendedproperty N'Description', '
Enum values:
Name: Onbekend, value: 0
Name: Man, value: 1
Name: Vrouw, value: 2', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'TestChildProp5'
GO
EXEC sp_addextendedproperty N'Description', 'This is TestChildProp6', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'TestChildProp6'
GO
EXEC sp_addextendedproperty N'Description', 'TestColumn', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'TestChildProp7Id'
GO
EXEC sp_addextendedproperty N'Description', 'Primary key of the table, auto-incremented by 1 eacht time a row is added to the table.', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'Description', 'A data type that exposes automatically generated, unique binary numbers within a database. rowversion is generally used as a mechanism for version-stamping table rows. The storage size is 8 bytes. The rowversion data type is just an incrementing number and does not preserve a date or a time. Each database has a counter that is incremented for each insert or update operation that is performed on a table that contains a rowversion column within the database. This counter is the database rowversion. This tracks a relative time within a database, not an actual time that can be associated with a clock. A table can have only one rowversion column. Every time that a row with a rowversion column is modified or inserted, the incremented database rowversion value is inserted in the rowversion column. The rowversion value is incremented with any update statement, even if no row values are changed.', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'Rowversion'
GO
EXEC sp_addextendedproperty N'Description', 'Generated Guid', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'UId'
GO
EXEC sp_addextendedproperty N'Description', 'Local timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'ModifiedOn'
GO
EXEC sp_addextendedproperty N'Description', 'Login name that made the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'ModifiedBy'
GO
EXEC sp_addextendedproperty N'Description', 'UTC timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'ModifiedOnUTC'
GO
EXEC sp_addextendedproperty N'Description', 'Local timestamp that the row was deleted', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'DeletedAt'
GO
EXEC sp_addextendedproperty N'Description', 'Login name that deleted the row.', 'SCHEMA', N'dbo', 'TABLE', N'FileMetOvererving_historie', 'COLUMN', N'DeletedBy'
GO
