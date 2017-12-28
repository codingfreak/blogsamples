CREATE TABLE [BaseData].[CustomerGroup] (
    [Id]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [Label] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CustomerGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_CustomerGroup_Label]
    ON [BaseData].[CustomerGroup]([Label] ASC);

