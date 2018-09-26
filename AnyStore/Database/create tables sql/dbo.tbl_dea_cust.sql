CREATE TABLE [dbo].[tbl_dea_cust] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [type]       VARCHAR (64)  NULL,
    [first_name] VARCHAR(128) NULL, 
    [last_name] VARCHAR(128) NULL, 
    [form_of_address] VARCHAR(64) NULL, 
    [address_street] VARCHAR(64) NULL, 
    [address_postcode] VARCHAR(64) NULL, 
    [address_city] VARCHAR(64) NULL, 
    [address_country] VARCHAR(64) NULL, 
    [contact_phone] VARCHAR(64) NULL, 
    [contact_mail] VARCHAR(64) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

