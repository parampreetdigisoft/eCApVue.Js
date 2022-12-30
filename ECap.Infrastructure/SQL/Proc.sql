ALTER PROCEDURE [spUserList]
AS
BEGIN
 SET NOCOUNT ON;  -- the count (indicating the number of rows affected by a SQL statement) is not returned
 SET XACT_ABORT ON -- if a SQL statement raises a run-time error, the entire transaction is terminated and rolled back. 
 
 SELECT
  UserId,User_Login,User_Pwd,User_ClientGroup,
  (select top 1 CONVERT(VARCHAR(10),User_ActiveDate,101) from ecap.tblUser_Product
  where UP_UserId = UserId
  order by User_ActiveDate desc)AS LastTestCreationDate,
  --111) AS LastTestCreationDate,
     
   (select top 1  UP_ProductLang=(CASE UP_ProductLang
WHEN 'jp' THEN 'JP'
WHEN 'J' THEN 'JP'
WHEN 'E' THEN 'EN'
WHEN 'en' THEN 'EN'
WHEN 'ko' THEN 'KR'
WHEN 'K' THEN 'KR'
ELSE NULL
END)   from ecap.tblUser_Product
  where UP_UserId = UserId order by User_ActiveDate desc)
   AS ProdLang,
   
 
  (select case when exists
  (
  select 1 from ecap.tblUser_Product t
  where t.User_IsActive = 1 and
  DATEDIFF(dd, t.User_ActiveDate, GETDATE()) <= 11 and
  t.UP_UserId = UserId  
  ) then 1 else 0 end) as User_IsActive,
 
  (select top 1 User_CompleteDate from ecap.tblUser_Product
  where UP_UserId = UserId
  order by User_CompleteDate desc) as User_CompleteDate,
  
  
  (select top 1 r.UploadDate from tblTestReport r,ecap.tblUser_Product t
  where r.UP_Id = t.UP_Id Order by UploadDate desc) as UploadDate,
  
   (select top 1 C_Name from tblCompanyDetails  where C_Id=User_ClientGroup ) as C_Name,
  
  User_FirstName,
  User_LastName,User_Address1,User_Address2,User_City,User_State,User_Country,User_Zipcode,User_Phone,User_AdminRole,UserLogin_Flag,SendMail_Flag,
  [dbo].[fnUser_GetOverallStatus](tuser.UserId) AS UserStatus
 FROM tblUser  tuser  where User_Login <> 'admin' --and a.C_Id=tuser.User_ClientGroup   
 order by UserId desc 
 
END
 
