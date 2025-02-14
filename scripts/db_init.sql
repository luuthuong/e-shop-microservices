
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

-- Enable Agent XPs

sp_configure 'show advanced options', 1;
GO

RECONFIGURE;
GO

sp_configure 'Agent XPs', 1;
GO

RECONFIGURE;
GO;