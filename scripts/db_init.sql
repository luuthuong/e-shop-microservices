
IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'ProductSyncDB')
CREATE DATABASE ProductSyncDB;
GO

IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'CustomerManageDB')
CREATE DATABASE CustomerManageDB;
GO

IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'PaymentDB')
CREATE DATABASE PaymentDB;
GO

IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'OrderDB')
CREATE DATABASE OrderDB;
GO

