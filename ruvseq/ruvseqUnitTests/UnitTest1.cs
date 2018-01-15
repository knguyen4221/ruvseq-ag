using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ruvseqUnitTests
{
    [TestClass]
    public class RUVSeqTests
    {
        [TestMethod]
        [ExpectedException]
        public void InputTest()
        {
            ruvseq.RUVSeq obj = new ruvseq.RUVSeq("", "", "");
        }
    }
}
