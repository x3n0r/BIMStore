CREATE TABLE [dbo].[tbl_animal] (
    [Id]            INT           NOT NULL IDENTITY,
    [name]          VARCHAR (64)  NULL,
    [species]       VARCHAR (64)  NULL,
    [race]          VARCHAR (64)  NULL,
    [date_of_birth] DATETIME      NULL,
    [notes]         VARCHAR (MAX) NULL,
    [cust_id]       INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

