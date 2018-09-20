CREATE TABLE [dbo].[tbl_companydata] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [name]    VARCHAR (MAX) NULL,
    [slogan]  VARCHAR (MAX) NULL,
    [address] VARCHAR (MAX) NULL,
    [country] VARCHAR (MAX) NULL,
    [telnb]   VARCHAR (50)  NULL,
    [email] VARCHAR(50) NULL, 
    [IBAN] VARCHAR(50) NULL, 
    [BIC] VARCHAR(50) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

