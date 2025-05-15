
IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'ProductSyncDB')
PRINT 'Creating ProductSyncDB...'
CREATE DATABASE ProductSyncDB;
GO

IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'CustomerManageDB')
PRINT 'Creating CustomerManageDB...'
CREATE DATABASE CustomerManageDB;
GO

IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'PaymentDB')
PRINT 'Creating PaymentDB...'
CREATE DATABASE PaymentDB;
GO

IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'OrderDB')
PRINT 'Creating OrderDB...'
CREATE DATABASE OrderDB;
GO

-- Enable Agent XPs

PRINT 'Enabling Agent XPs...';

EXEC sp_configure 'show advanced options', 1;

GO

RECONFIGURE;
GO

sp_configure 'Agent XPs', 1;
GO

RECONFIGURE;