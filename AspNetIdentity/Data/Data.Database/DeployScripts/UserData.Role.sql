SET IDENTITY_INSERT [UserData].[Role] ON
INSERT INTO [UserData].[Role]
([Id], [Name])
VALUES
(1, 'User'),
(2, 'Admin');
SET IDENTITY_INSERT [UserData].[Role] OFF