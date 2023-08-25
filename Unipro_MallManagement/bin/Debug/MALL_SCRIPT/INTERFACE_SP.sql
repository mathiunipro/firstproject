
CREATE PROCEDURE [dbo].[sp_GenerateFile]                
@MachineId Varchar(20),                
@SalesDate datetime,                
@FileName varchar(2000),  
@TenantCode Varchar(20)  
                
AS                
BEGIN                
                
SET NOCOUNT ON                
            insert into tblPOSManagementData values('0' ,@SalesDate ,'')     
declare @sql varchar(8000)                
SET @sql = 'bcp ' + '"' + 'EXEC RetailPos_LG_CT..sp_getSalesByDate '  +''''+ Replace(CONVERT(VARCHAR(8), @SalesDate,112),'-','')   +''','''+@MachineId +''','''+@TenantCode + '''"' + ' queryout '  + '"'+  @FileName +'"' + ' -U sa -P ups -S '+ 'USER-PC123' 
  + ' -c'      
                  
Print @sql                    
--SELECT @cmd = @sql                     
insert into tblPOSManagementData values('0' ,@SalesDate ,@sql)                    
exec master..xp_cmdshell @sql                    
End           
    
          