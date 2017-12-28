SET IDENTITY_INSERT [BaseData].[Customer] ON
INSERT INTO [BaseData].[Customer] (Id, Number, Label) VALUES
(1, N'K001', N'Müller GmbH'),
(2,	N'K002', N'Schulze AG'),
(3,	N'K003', N'Mustermann GbR');
SET IDENTITY_INSERT [BaseData].[Customer] OFF