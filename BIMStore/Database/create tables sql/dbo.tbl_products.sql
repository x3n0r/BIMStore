CREATE TABLE [dbo].[tbl_products] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [name]        VARCHAR (50)    NULL,
    [category]    INT             NULL,
    [description] VARCHAR (MAX)   NULL,
    [rate]        DECIMAL (18, 2) NULL,
    [qty]         DECIMAL (18, 2) NULL,
    [added_date]  DATETIME        NULL,
    [added_by]    INT             NULL,
    [hasqty] BIT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

