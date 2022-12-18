using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShop.Core.Enums
{
    
   

    public enum DbOperationStatusEnum
    {
        Success,
        Failed,
        Repeated,
        Intersected,
        OldPasswordNotCorrect,
        ItemNotExist,
        ClusterRepeated,
        SuccessWithMessage,
        DetailsRepeated,
        OrderRepeated,
        Updated,
        Inserted,
        RangeRepeated,
        ListEmpty,
    }



}
