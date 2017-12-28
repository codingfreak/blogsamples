SET IDENTITY_INSERT [BaseData].[CustomerGroup] ON
INSERT INTO [BaseData].[CustomerGroup] (Id, Label) VALUES
(1, N'Normale Kunden'),
(2,	N'Gute Kunden'),
(3,	N'Beste Kunden');
SET IDENTITY_INSERT [BaseData].[CustomerGroup] OFF