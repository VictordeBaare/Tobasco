CREATE TABLE [dbo].[ChildObject](
	 [Id]			 bigint	IDENTITY (1,1)     NOT NULL
	,[RowVersion]    rowversion         	   NOT NULL
	,TestChildProp1 varchar(100) NOT NULL
	,[ModifiedBy]	 nvarchar(256)			   NOT NULL
	 CONSTRAINT [DF_ChildObject_ModifiedBy]    DEFAULT SUSER_SNAME()
	,[ModifiedOn]	 datetime2(7)			   NOT NULL
	 CONSTRAINT [DF_ChildObject_ModifiedOn]    DEFAULT SYSDATETIME()
	,[ModifiedOnUTC] datetime2(7)			   NOT NULL
	 CONSTRAINT [DF_ChildObject_ModifiedOnUTC] DEFAULT SYSUTCDATETIME()
	,CONSTRAINT [PK_ChildObject] PRIMARY KEY CLUSTERED (Id ASC)
	 
);
GO
CREATE TABLE [dbo].[ChildObject_historie] (
    [Id]                          bigint             NOT NULL
   ,[RowVersion]                  binary(8)          NOT NULL
   ,TestChildProp1 varchar(100) NOT NULL   
   ,[ModifiedBy]                  nvarchar (256)     NOT NULL
   ,[ModifiedOn]                  DATETIME2(7)       NOT NULL
   ,DeletedBy                     nvarchar(256)     NULL
   ,DeletedAt                     datetime2(7)      NULL
);
GO
CREATE NONCLUSTERED INDEX IX_ChildObject_historie_Id
                       ON [dbo].ChildObject_historie
                         (Id ASC)
                  INCLUDE(ModifiedOn);
GO

GO
CREATE TRIGGER [dbo].tu_ChildObject
            ON [dbo].ChildObject
           FOR UPDATE
AS
BEGIN
    INSERT
      INTO [dbo].ChildObject_historie(
			Id,
		    [RowVersion],
		   TestChildProp1,
            [ModifiedBy],
            [ModifiedOn],
            DeletedBy,
            DeletedAt
           )
    SELECT DELETED.Id,
           DELETED.[RowVersion],
		  Deleted.TestChildProp1,
           Deleted.ModifiedBy,
           Deleted.ModifiedOn,
           NULL,
           NULL
      FROM Deleted;
END;
GO
CREATE TRIGGER [dbo].td_ChildObject
            ON [dbo].ChildObject
		   FOR DELETE
AS
BEGIN
	INSERT
	  INTO [dbo].ChildObject_historie(
			Id,
		    [RowVersion],
           TestChildProp1,
		    [ModifiedBy],
		    [ModifiedOn],
		    [DeletedBy],
		    [DeletedAt]
            )
	SELECT Deleted.Id,
	       Deleted.[RowVersion],
		  Deleted.TestChildProp1,
		   Deleted.ModifiedBy,
		   Deleted.ModifiedOn,
		   ISNULL(LTRIM(RTRIM(CONVERT(nvarchar(128), CONTEXT_INFO()))), SUSER_SNAME()),
		   SYSDATETIME()
	  FROM Deleted;
END;

