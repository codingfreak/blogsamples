CREATE TABLE [UserData].[UserLogin] (
    [Id]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserId]        BIGINT         NOT NULL,
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_UserLogin] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserLogin_User] FOREIGN KEY ([UserId]) REFERENCES [UserData].[User] ([Id])
);

