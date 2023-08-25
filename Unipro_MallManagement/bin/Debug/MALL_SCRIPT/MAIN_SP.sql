
 CREATE proc [dbo].[Sp_ManagementFilebyDate]                          
( @curDate datetime    )                      
as                          
Declare @Machineid varchar(10)                          
Declare @BatchId Numeric    
Declare @FileName varchar(500)         
Declare @TenantCode varchar(10)    
Begin                          
  	Set @Machineid= '12054'      
  Set @TenantCode = 'LA/2102/1201'  
      
    
    Set @FileName = 'C:\UNIPRO\MALLMGMT\' + replace(convert(varchar, @curDate,112),'/','') +   
 replace(convert(varchar, getdate(),108),':','') +'.CSV'  
 exec [sp_GenerateFile] @MachineId, @curDate ,@FileName,@TenantCode          
         
End     
