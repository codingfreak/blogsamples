CREATE TABLE [UserData].[UserRole] (
    [Id]     BIGINT IDENTITY (1, 1) NOT NULL,
    [UserId] BIGINT NOT NULL,
    [RoleId] BIGINT NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [UserData].[Role] ([Id]),
    CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [UserData].[User] ([Id])
);

