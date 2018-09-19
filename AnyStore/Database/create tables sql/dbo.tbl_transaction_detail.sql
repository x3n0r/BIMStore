CREATE TABLE [dbo].[tbl_transaction_detail] (
    [Id]          INT             NOT NULL IDENTITY,
    [product_id]  INT             NULL,
    [rate]        DECIMAL (18, 2) NULL,
    [qty]         DECIMAL (18, 2) NULL,
    [total]       DECIMAL (18, 2) NULL,
    [dea_cust_id] INT             NULL,
    [added_date]  DATETIME        NULL,
    [added_by]    INT             NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

