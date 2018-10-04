CREATE TABLE [dbo].[tbl_users] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [first_name] VARCHAR (50)  NULL,
    [last_name]  VARCHAR (50)  NULL,
    [email]      VARCHAR (150) NULL,
    [username]   VARCHAR (50)  NULL,
    [password]   VARCHAR (50)  NULL,
    [contact]    VARCHAR (64)  NULL,
    [address]    VARCHAR (50)  NULL,
    [gender]     VARCHAR (50)  NULL,
    [user_type]  VARCHAR (50)  NULL,
    [added_date] DATETIME      NULL,
    [added_by]   INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

