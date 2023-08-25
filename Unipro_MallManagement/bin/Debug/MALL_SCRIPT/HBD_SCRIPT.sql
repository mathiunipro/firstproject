CREATE procedure [dbo].[sp_getSalesByDate](                     
@CurDate varchar(8),                     
@MachineId nVarchar(10),  
@TenantCode nVarchar(20)  
) as                     
BEGIN      
    
 select @MachineId + ',' + @TenantCode + ',' + '1' + ',' + date + ',' + LTRIM(RTRIM(Cast(Convert(Decimal(18,2),GTO)as Char))) + ',' +   
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),gst)as Char))) + ',' + '0' + ',' + '0' + ',' + LTRIM(RTRIM(Cast(Convert(Decimal(18,2),netsales)as Char))) + ',' +   
 LTRIM(RTRIM(invoiceno)) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),discount)as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),cash)as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),nets)as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),visa)as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),[master card])as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),Amex)as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),Voucher)as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),[Other Charge Amount])as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),grabpay)as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),favepay)as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),[Paynow/paylah/payanyone])as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),[Apple Pay/Google Pay/Samsung Pay])as Char))) + ',' +  
 LTRIM(RTRIM(Cast(Convert(Decimal(18,2),[Other Digital Payment])as Char))) + ',' + '0' + ',' + '0' + ',' + '0' + ',' + '0'   
 from [vwSalesDateToMgmnt_UPDATE] Where InvoiceDAte = @CurDate  
  
END           
  
  
  