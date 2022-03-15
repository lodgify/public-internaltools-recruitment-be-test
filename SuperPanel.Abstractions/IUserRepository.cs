﻿using SuperPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPanel.Abstractions
{
    public interface IUserRepository
    {
        List<User> QueryAll();
    }
}
