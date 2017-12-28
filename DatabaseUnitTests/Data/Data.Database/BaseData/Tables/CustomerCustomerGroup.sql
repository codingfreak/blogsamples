CREATE TABLE [BaseData].[CustomerCustomerGroup] (
    [Id]              BIGINT IDENTITY (1, 1) NOT NULL,
    [CustomerId]      BIGINT NOT NULL,
    [CustomerGroupId] BIGINT NOT NULL,
    CONSTRAINT [PK_CustomerCustomerGroup] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CustomerCustomerGroup_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [BaseData].[Customer] ([Id]),
    CONSTRAINT [FK_CustomerCustomerGroup_CustomerGroup] FOREIGN KEY ([CustomerGroupId]) REFERENCES [BaseData].[CustomerGroup] ([Id])
);

