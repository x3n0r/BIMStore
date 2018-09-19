CREATE TABLE [dbo].[tbl_categories] (
    [Id]          INT          NOT NULL IDENTITY,
    [title]       VARCHAR (50) NULL,
    [description] VARCHAR(MAX)         NULL,
    [added_date]  DATETIME     NULL,
    [added_by]    INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

