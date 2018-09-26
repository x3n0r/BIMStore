CREATE TABLE [dbo].[tbl_transactions] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [type]             VARCHAR (50)    NULL,
    [dea_cust_id]      INT             NULL,
    [grandTotal]       DECIMAL (18, 2) NULL,
    [transaction_date] DATETIME        NULL,
    [tax]              DECIMAL (18, 2) NULL,
    [discount]         DECIMAL (18, 2) NULL,
    [added_by]         INT             NULL,
    [kontobez] CHAR(1) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

