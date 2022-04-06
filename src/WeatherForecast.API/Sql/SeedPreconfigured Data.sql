--- Seed Preconfigured Data
USE WeatherForecastDataBase;

--- If already seeded then skip
if(EXISTS(SELECT * FROM dbo.AspNetUsers))
BEGIN
    PRINT 'Preconfigured data already seeded';
    RETURN;
END

BEGIN TRANSACTION;  
  
BEGIN TRY 
    --- Insert Roles
    INSERT INTO dbo.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES('fab4fac1-c546-41de-aebc-a14da6895711', 'Admin', 'Admin', '1'),
          ('cbbf3ac1-c542-41de-abbc-a14fa6895724', 'User', 'User', '2')

    --- Insert Admin User - Bob
    INSERT INTO dbo.AspNetUsers(Id, UserName, Email, EmailConfirmed, ConcurrencyStamp, PasswordHash, AccessFailedCount, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled)
    VALUES('b74ddd14-6340-4840-95c2-db12554843e5', 'Bob', 'Bob@ITech.com', 1, '1', 'AQAAAAEAACcQAAAAEL6ZsxjaYdjTjBbiA62Sv9bDAXdxkyhCNWJSTso9qd+hmJtX/SDbeQWE9+baZPPryA==', 3, '123456789', 1, 0, 0); --- PWD : Admin*123

    --- Set Bob as Admin user
    INSERT INTO dbo.AspNetUserRoles(UserId, RoleId)
    VALUES('b74ddd14-6340-4840-95c2-db12554843e5', 'fab4fac1-c546-41de-aebc-a14da6895711');
     
END TRY  
BEGIN CATCH  
    SELECT   
        ERROR_NUMBER() AS ErrorNumber  
        ,ERROR_SEVERITY() AS ErrorSeverity  
        ,ERROR_STATE() AS ErrorState  
        ,ERROR_PROCEDURE() AS ErrorProcedure  
        ,ERROR_LINE() AS ErrorLine  
        ,ERROR_MESSAGE() AS ErrorMessage;  
  
    IF @@TRANCOUNT > 0  
        ROLLBACK TRANSACTION;  
END CATCH;  
  
IF @@TRANCOUNT > 0  
    COMMIT TRANSACTION;  
GO  