CREATE TABLE [UserData].[Claim] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserId]     BIGINT         NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Claim] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Claim_User] FOREIGN KEY ([UserId]) REFERENCES [UserData].[User] ([Id])
);

