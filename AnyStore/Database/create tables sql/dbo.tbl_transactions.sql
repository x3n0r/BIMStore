CREATE TABLE [dbo].[tbl_transactions] (
    [Id]               INT             NOT NULL IDENTITY,
    [type]             VARCHAR (50)    NULL,
    [dea_cust_id]      INT             NULL,
    [grandTotal]       DECIMAL (18, 2) NULL,
    [transaction_date] DATETIME        NULL,
    [tax]              DECIMAL (18, 2) NULL,
    [discount]         DECIMAL (18, 2) NULL,
    [added_by]         INT             NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

