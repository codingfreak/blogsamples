SET IDENTITY_INSERT [BaseData].[CustomerCustomerGroup] ON
INSERT INTO [BaseData].[CustomerCustomerGroup] (Id, CustomerId, CustomerGroupId) VALUES
(1, 1, 2),
(2, 1, 1),
(3,	2, 3),
(4,	3, 1);
SET IDENTITY_INSERT [BaseData].[CustomerCustomerGroup] OFF