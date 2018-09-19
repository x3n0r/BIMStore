CREATE TABLE [dbo].[tbl_products] (
    [Id]          INT             NOT NULL IDENTITY,
    [name]        VARCHAR (50)    NULL,
    [category]    INT             NULL,
    [description] VARCHAR(MAX)            NULL,
    [rate]        DECIMAL (18, 2) NULL,
    [qty]         DECIMAL (18, 2) NULL,
    [added_date]  DATETIME        NULL,
    [added_by]    INT             NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

