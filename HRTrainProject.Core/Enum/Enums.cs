﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HRTrainProject.Core
{
    public enum UserRole
    {
        最大管理者 = 0,
        管理者 = 1,
        一般會員 =2
    }
    public enum PolicyGroup
    {
        管理者級別,
        基層級別
    }
    public enum SortBy
    {
        DESC,
        ASC
    }

}