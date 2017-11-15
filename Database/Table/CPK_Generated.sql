CREATE TABLE [dbo].[CPK](
	 [Id]			 bigint	IDENTITY (1,1)     NOT NULL
	,[RowVersion]    rowversion         	   NOT NULL
	,Training nvarchar(100) NOT NULL
,Duur nvarchar(100) NOT NULL
,Kosten nvarchar(100) NOT NULL
	,[ModifiedBy]	 nvarchar(256)			   NOT NULL
	 CONSTRAINT [DF_CPK_ModifiedBy]    DEFAULT SUSER_SNAME()
	,[ModifiedOn]	 datetime2(7)			   NOT NULL
	 CONSTRAINT [DF_CPK_ModifiedOn]    DEFAULT SYSDATETIME()
	,[ModifiedOnUTC] datetime2(7)			   NOT NULL
	 CONSTRAINT [DF_CPK_ModifiedOnUTC] DEFAULT SYSUTCDATETIME()
	,CONSTRAINT [PK_CPK] PRIMARY KEY CLUSTERED (Id ASC)
	 
);
GO
CREATE TABLE [dbo].[CPK_historie] (
    [Id]                          bigint             NOT NULL
   ,[RowVersion]                  binary(8)          NOT NULL
   ,Training nvarchar(100) NOT NULL
,Duur nvarchar(100) NOT NULL
,Kosten nvarchar(100) NOT NULL   
   ,[ModifiedBy]                  nvarchar (256)     NOT NULL
   ,[ModifiedOn]                  DATETIME2(7)       NOT NULL
   ,DeletedBy                     nvarchar(256)     NULL
   ,DeletedAt                     datetime2(7)      NULL
);
GO
CREATE NONCLUSTERED INDEX IX_CPK_historie_Id
                       ON [dbo].CPK_historie
                         (Id ASC)
                  INCLUDE(ModifiedOn);
GO

GO
CREATE TRIGGER [dbo].tu_CPK
            ON [dbo].CPK
           FOR UPDATE
AS
BEGIN
    INSERT
      INTO [dbo].CPK_historie(
		   Id,
[RowVersion],
Training,
Duur,
Kosten,
ModifiedBy,
ModifiedOn,
DeletedBy,
DeletedAt
           )
    SELECT DELETED.Id,
           DELETED.[RowVersion],
		  Deleted.Training,
Deleted.Duur,
Deleted.Kosten,
Deleted.ModifiedBy,
Deleted.ModifiedOn,
NULL,
NULL
      FROM Deleted;
END;
GO
CREATE TRIGGER [dbo].td_CPK
            ON [dbo].CPK
		   FOR DELETE
AS
BEGIN
	INSERT
	  INTO [dbo].CPK_historie(
           Id,
[RowVersion],
Training,
Duur,
Kosten,
ModifiedBy,
ModifiedOn,
DeletedBy,
DeletedAt
            )
	SELECT Deleted.Id,
	       Deleted.[RowVersion],
		  Deleted.Training,
Deleted.Duur,
Deleted.Kosten,
Deleted.ModifiedBy,
Deleted.ModifiedOn,
ISNULL(LTRIM(RTRIM(CONVERT(nvarchar(128), CONTEXT_INFO()))), SUSER_SNAME()),
SYSDATETIME()
	  FROM Deleted;
END;

