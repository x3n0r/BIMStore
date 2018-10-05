CREATE TABLE [dbo].[tbl_categories] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [title]       VARCHAR (50)  NULL,
    [description] VARCHAR (MAX) NULL,
    [added_date]  DATETIME      NULL,
    [added_by]    INT           NULL,
    [tax] DECIMAL(18, 2) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

