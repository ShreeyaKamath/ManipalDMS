CREATE LOGIN sa WITH PASSWORD = 'shreeyakamatH7', CHECK_POLICY = OFF;
ALTER SERVER ROLE sysadmin ADD MEMBER sa;
ALTER LOGIN sa ENABLE;
ALTER LOGIN sa WITH PASSWORD = 'shreeyakamatH7';
EXEC sp_addsrvrolemember 'sa', 'sysadmin';
SELECT name, is_disabled
FROM sys.sql_logins
WHERE name = 'sa';
ALTER LOGIN sa WITH PASSWORD = 'shreeyakamatH7' UNLOCK, CHECK_POLICY = OFF;
