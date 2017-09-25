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
,TestChildProp9Id bigint NULL
	,[ModifiedBY]	nvarchar(256)			NOT NULL
	 CONSTRAINT [DF_{FileMetOvererving}_ModifiedBy] DEFAULT SUSER_SNAME()
	,[ModifiedOn]	datetime2(7)			NOT NULL
	 CONSTRAINT [DF_{FileMetOvererving}_ModifiedOn] DEFAULT SYSDATETIME()
	 CONSTRAINT [PK_{FileMetOvererving}] PRIMARY KEY CLUSTERED (Id ASC)
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
,TestChildProp9Id bigint NULL   
   ,[ModifiedBy]                  nvarchar (256)     NOT NULL
   ,[ModifiedOn]                  DATETIME2(7)       NOT NULL
   ,DeletedBy                     nvarchar(256)      NULL
   ,DeletedAt                     datetime2(7)       NULL
);
GO
-- ================================================================================
-- I n d e x e s
-- ================================================================================

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
-- ================================================================================
-- T r i g g e r s
-- ================================================================================

CREATE TRIGGER [dbo].tu_FileMetOvererving
            ON [dbo].FileMetOvererving
           FOR UPDATE
AS
BEGIN
    INSERT
      INTO [dbo].FileMetOvererving_historie(
			Id,
		    [RowVersion],
		   TestChildProp1,
TestChildProp2,
TestChildProp3,
TestChildProp4,
TestChildProp5,
TestChildProp6,
TestChildProp7Id,
TestChildProp9Id,
            [ModifiedBy],
            [ModifiedOn],
            DeletedBy,
            DeletedAt
           )
    SELECT DELETED.Id,
           DELETED.[RowVersion],
		  Deleted.TestChildProp1,
Deleted.TestChildProp2,
Deleted.TestChildProp3,
Deleted.TestChildProp4,
Deleted.TestChildProp5,
Deleted.TestChildProp6,
Deleted.TestChildProp7Id,
Deleted.TestChildProp9Id,
           Deleted.ModifiedBy,
           Deleted.ModifiedOn,
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
		    [RowVersion],
           TestChildProp1,
TestChildProp2,
TestChildProp3,
TestChildProp4,
TestChildProp5,
TestChildProp6,
TestChildProp7Id,
TestChildProp9Id,
		    [ModifiedBy],
		    [ModifiedOn],
		    [DeletedBy],
		    [DeletedAt]
            )
	SELECT Deleted.Id,
	       Deleted.[RowVersion],
		  Deleted.TestChildProp1,
Deleted.TestChildProp2,
Deleted.TestChildProp3,
Deleted.TestChildProp4,
Deleted.TestChildProp5,
Deleted.TestChildProp6,
Deleted.TestChildProp7Id,
Deleted.TestChildProp9Id,
		   Deleted.ModifiedBy,
		   Deleted.ModifiedOn,
		   ISNULL(LTRIM(RTRIM(CONVERT(nvarchar(128), CONTEXT_INFO()))), SUSER_SNAME()),
		   SYSDATETIME()
	  FROM Deleted;
END;

