﻿CREATE TABLE [dbo].[CPK15](
	 [Id]			 bigint	IDENTITY (1,1)     NOT NULL
	,[RowVersion]    rowversion         	   NOT NULL
	,Training nvarchar(100) NOT NULL
,Duur nvarchar(100) NOT NULL
,Kosten nvarchar(100) NOT NULL
	,[ModifiedBy]	 nvarchar(256)			   NOT NULL
	 CONSTRAINT [DF_CPK15_ModifiedBy]    DEFAULT SUSER_SNAME()
	,[ModifiedOn]	 datetime2(7)			   NOT NULL
	 CONSTRAINT [DF_CPK15_ModifiedOn]    DEFAULT SYSDATETIME()
	,[ModifiedOnUTC] datetime2(7)			   NOT NULL
	 CONSTRAINT [DF_CPK15_ModifiedOnUTC] DEFAULT SYSUTCDATETIME()
	,CONSTRAINT [PK_CPK15] PRIMARY KEY CLUSTERED (Id ASC)
);
GO
CREATE TABLE [dbo].[CPK15_historie] (
    [Id]                          bigint             NOT NULL
   ,[RowVersion]                  binary(8)          NOT NULL
   ,Training nvarchar(100) NOT NULL
,Duur nvarchar(100) NOT NULL
,Kosten nvarchar(100) NOT NULL   
   ,[ModifiedBy]                  nvarchar (256)     NOT NULL
   ,[ModifiedOn]                  datetime2(7)       NOT NULL
   ,[ModifiedOnUTC] 			  datetime2(7)	     NOT NULL
   ,DeletedBy                     nvarchar(256)     NULL
   ,DeletedAt                     datetime2(7)      NULL
);
GO
CREATE NONCLUSTERED INDEX IX_CPK15_historie_Id
                       ON [dbo].CPK15_historie
                         (Id ASC)
                  INCLUDE(ModifiedOn);
GO

GO
CREATE TRIGGER [dbo].[tu_CPK15]
            ON [dbo].CPK15
           FOR UPDATE
AS
BEGIN

    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT
      INTO [dbo].CPK15_historie(
		   Id,
[RowVersion],
Training,
Duur,
Kosten,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC,
DeletedBy,
DeletedAt
           )
    SELECT 
		  Deleted.Id,
Deleted.[RowVersion],
Deleted.Training,
Deleted.Duur,
Deleted.Kosten,
Deleted.ModifiedBy,
Deleted.ModifiedOn,
Deleted.ModifiedOnUTC,
NULL,
NULL
      FROM Deleted;
END;
GO
CREATE TRIGGER [dbo].[td_CPK15]
            ON [dbo].CPK15
		   FOR DELETE
AS
BEGIN

    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

	INSERT
	  INTO [dbo].CPK15_historie(
           Id,
[RowVersion],
Training,
Duur,
Kosten,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC,
DeletedBy,
DeletedAt
            )
	SELECT 		  Deleted.Id,
Deleted.[RowVersion],
Deleted.Training,
Deleted.Duur,
Deleted.Kosten,
Deleted.ModifiedBy,
Deleted.ModifiedOn,
Deleted.ModifiedOnUTC,
ISNULL(LTRIM(RTRIM(CONVERT(varchar(128), CONTEXT_INFO()))), CAST(SUSER_SNAME() AS varchar(128))),
SYSDATETIME()
	  FROM Deleted;
END;

GO
EXEC sp_addextendedproperty N'Description', 'CPK15', 'SCHEMA', N'dbo', 'TABLE', N'CPK15', NULL, NULL
GO
EXEC sp_addextendedproperty N'Description', 'This is Training', 'SCHEMA', N'dbo', 'TABLE', N'CPK15', 'COLUMN', N'Training'
GO
EXEC sp_addextendedproperty N'Description', 'This is Duur', 'SCHEMA', N'dbo', 'TABLE', N'CPK15', 'COLUMN', N'Duur'
GO
EXEC sp_addextendedproperty N'Description', 'This is Kosten', 'SCHEMA', N'dbo', 'TABLE', N'CPK15', 'COLUMN', N'Kosten'
GO
EXEC sp_addextendedproperty N'Description', 'Primary key of the table, auto-incremented by 1 eacht time a row is added to the table.', 'SCHEMA', N'dbo', 'TABLE', N'CPK15', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'Description', 'A data type that exposes automatically generated, unique binary numbers within a database. rowversion is generally used as a mechanism for version-stamping table rows. The storage size is 8 bytes. The rowversion data type is just an incrementing number and does not preserve a date or a time. Each database has a counter that is incremented for each insert or update operation that is performed on a table that contains a rowversion column within the database. This counter is the database rowversion. This tracks a relative time within a database, not an actual time that can be associated with a clock. A table can have only one rowversion column. Every time that a row with a rowversion column is modified or inserted, the incremented database rowversion value is inserted in the rowversion column. The rowversion value is incremented with any update statement, even if no row values are changed.', 'SCHEMA', N'dbo', 'TABLE', N'CPK15', 'COLUMN', N'Rowversion'
GO
EXEC sp_addextendedproperty N'Description', 'Local timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'CPK15', 'COLUMN', N'ModifiedOn'
GO
EXEC sp_addextendedproperty N'Description', 'Login name that made the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'CPK15', 'COLUMN', N'ModifiedBy'
GO
EXEC sp_addextendedproperty N'Description', 'UTC timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'CPK15', 'COLUMN', N'ModifiedOnUTC'
GO
GO
EXEC sp_addextendedproperty N'Description', 'CPK15', 'SCHEMA', N'dbo', 'TABLE', N'CPK15_historie', NULL, NULL
GO
EXEC sp_addextendedproperty N'Description', 'This is Training', 'SCHEMA', N'dbo', 'TABLE', N'CPK15_historie', 'COLUMN', N'Training'
GO
EXEC sp_addextendedproperty N'Description', 'This is Duur', 'SCHEMA', N'dbo', 'TABLE', N'CPK15_historie', 'COLUMN', N'Duur'
GO
EXEC sp_addextendedproperty N'Description', 'This is Kosten', 'SCHEMA', N'dbo', 'TABLE', N'CPK15_historie', 'COLUMN', N'Kosten'
GO
EXEC sp_addextendedproperty N'Description', 'Primary key of the table, auto-incremented by 1 eacht time a row is added to the table.', 'SCHEMA', N'dbo', 'TABLE', N'CPK15_historie', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'Description', 'A data type that exposes automatically generated, unique binary numbers within a database. rowversion is generally used as a mechanism for version-stamping table rows. The storage size is 8 bytes. The rowversion data type is just an incrementing number and does not preserve a date or a time. Each database has a counter that is incremented for each insert or update operation that is performed on a table that contains a rowversion column within the database. This counter is the database rowversion. This tracks a relative time within a database, not an actual time that can be associated with a clock. A table can have only one rowversion column. Every time that a row with a rowversion column is modified or inserted, the incremented database rowversion value is inserted in the rowversion column. The rowversion value is incremented with any update statement, even if no row values are changed.', 'SCHEMA', N'dbo', 'TABLE', N'CPK15_historie', 'COLUMN', N'Rowversion'
GO
EXEC sp_addextendedproperty N'Description', 'Local timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'CPK15_historie', 'COLUMN', N'ModifiedOn'
GO
EXEC sp_addextendedproperty N'Description', 'Login name that made the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'CPK15_historie', 'COLUMN', N'ModifiedBy'
GO
EXEC sp_addextendedproperty N'Description', 'UTC timestamp of the latest change to the row.', 'SCHEMA', N'dbo', 'TABLE', N'CPK15_historie', 'COLUMN', N'ModifiedOnUTC'
GO
EXEC sp_addextendedproperty N'Description', 'Local timestamp that the row was deleted', 'SCHEMA', N'dbo', 'TABLE', N'CPK15_historie', 'COLUMN', N'DeletedAt'
GO
EXEC sp_addextendedproperty N'Description', 'Login name that deleted the row.', 'SCHEMA', N'dbo', 'TABLE', N'CPK15_historie', 'COLUMN', N'DeletedBy'
GO
GO
CREATE VIEW [dbo].CPK15_historie_view
	WITH SCHEMABINDING
AS
	WITH CTE_historie 
	    (Id,
		 [RowVersion],
		Training,
Duur,
Kosten,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC,		   
		 DeletedBy,
		 DeletedAt,              
		 [FromSource]
		) AS
	(
		SELECT Id,
			   [RowVersion],
			  Training,
Duur,
Kosten,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC,		   
			   DeletedBy,
			   DeletedAt,
			   'History'
		  FROM [dbo].CPK15_historie
		  
		 UNION ALL
		 
		SELECT Id,
			   [RowVersion],
			   Training,
Duur,
Kosten,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC,			   
			   NULL,
			   NULL,
			   'Main'
		  FROM [dbo].CPK15
	)
		,CTE_historie_order
		(Id,
		 [RowVersion],
		Training,
Duur,
Kosten,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC,			   
		 DeletedBy,                    
		 DeletedAt,                   
		 [FromSource],
		 ChronologicalOrder
		) AS
	(
		SELECT Id,
			   [RowVersion],
			   Training,
Duur,
Kosten,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC,		   
			   DeletedBy,
               DeletedAt,
               [FromSource],
               ROW_NUMBER() OVER (PARTITION BY CTE_historie.Id
                                      ORDER BY CTE_historie.[RowVersion]
                                 ) AS ChronologicalOrder
		  FROM CTE_historie
	)
	SELECT Id,
           [RowVersion],
		  Training,
Duur,
Kosten,
ModifiedBy,
ModifiedOn,
ModifiedOnUTC,
           DeletedBy,
           DeletedAt,
           [FromSource],
           ChronologicalOrder
      FROM CTE_historie_order;
