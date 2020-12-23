﻿using AirlineProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProjectTest
{
    public static class TestConfig
    {
        internal static FlyingCenterSystem fcs = FlyingCenterSystem.GetInstance();
        internal static TestingFacade testFacade = new TestingFacade();
    }
}
