CREATE TABLE [dbo].[tbl_dea_cust] (
    [Id]         INT           NOT NULL IDENTITY,
    [type]       VARCHAR (50)  NULL,
    [name]       VARCHAR (150) NULL,
    [email]      VARCHAR (150) NULL,
    [contact]    VARCHAR (15)  NULL,
    [address]    VARCHAR(MAX)          NULL,
    [added_date] DATETIME      NULL,
    [added_by]   INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

