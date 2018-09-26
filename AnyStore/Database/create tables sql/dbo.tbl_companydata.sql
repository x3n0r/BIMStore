CREATE TABLE [dbo].[tbl_companydata] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [name]    VARCHAR (MAX) NULL,
    [slogan]  VARCHAR (MAX) NULL,
    [contact_phone]   VARCHAR (50)  NULL,
    [contact_email]   VARCHAR (50)  NULL,
    [IBAN]    VARCHAR (50)  NULL,
    [BIC]     VARCHAR (50)  NULL,
    [address_street] VARCHAR(64) NULL, 
    [address_postcode] VARCHAR(64) NULL, 
    [address_city] VARCHAR(64) NULL, 
    [address_country] VARCHAR(64) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

