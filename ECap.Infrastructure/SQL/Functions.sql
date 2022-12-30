CREATE FUNCTION fnUser_GetOverallStatus (
	 @UserId int
)
RETURNS INT AS
BEGIN
 declare @upTake int, @return_value int;
 
 if exists(select top 1 * from ecap.tblUser_Product tp where tp.UP_UserId = @UserId order by tp.UP_Id desc)
 begin
    select top(1) @upTake = UP_Id from (select case when t1.UploadDate > t2.User_ActiveDate and t1.UploadDate is not null then t1.UploadDate else t2.User_ActiveDate end as mDate,t2.UP_Id  from dbo.tblTestReport  t1 right join ecap.tblUser_Product t2 on t1.UP_ID=t2.UP_Id where  t2.UP_UserId=@UserId) t  order by mDate desc
    
	Select @return_value = case 
    when exists 
   (  select top 1 * from ecap.tblUser_Product t,tblTestReport r where  t.UP_Id = r.UP_Id and
     (r.UploadDate is not null) and t.UP_Id =@upTake order by  t.UP_Id desc
   )
   then 3
    when exists
   (
   select top 1 * from ecap.tblUser_Product t
   where (t.User_Completedate is not null) and  
    t.UP_Id =@upTake order by t.UP_Id desc
   ) then 2   
   when exists(
   select  1 from ecap.tblUser_Product t
   where t.User_IsActive = 1 and
   DATEDIFF(dd, t.User_ActiveDate, GETDATE()) <= 11 and
   t.UP_UserId = @UserId  
   ) then 1 
   else
   0 end
 end
    RETURN @return_value;
END;