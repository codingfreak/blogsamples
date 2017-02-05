CREATE TABLE [UserData].[User] (
    [Id]                   BIGINT             IDENTITY (1, 1) NOT NULL,
    [UserName]             NVARCHAR (256)     NOT NULL,
    [Email]                NVARCHAR (256)     NOT NULL,
    [EmailConfirmed]       BIT                NOT NULL,
    [PasswordHash]         NVARCHAR (MAX)     NOT NULL,
    [SecurityStamp]        NVARCHAR (MAX)     NULL,
    [PhoneNumber]          NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed] BIT                NOT NULL,
    [TwoFactorEnabled]     BIT                NOT NULL,
    [LockoutEndDateUtc]    DATETIMEOFFSET (7) NULL,
    [LockoutEnabled]       BIT                CONSTRAINT [DF_User_LockoutEnabled] DEFAULT ((0)) NOT NULL,
    [AccessFailedCount]    INT                NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_User_UserName]
    ON [UserData].[User]([UserName] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_User_Email]
    ON [UserData].[User]([Email] ASC);

