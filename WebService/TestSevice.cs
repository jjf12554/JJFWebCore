using System;
using WebDao;

namespace WebService
{
    public class TestSevice
    {
        public TestDao testDao { get; set; }

        public string Test()
        {
            return testDao.Test();
        }
    }
}
