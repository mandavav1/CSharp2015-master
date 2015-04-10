using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.CheckBook;
using System.Linq;

namespace CalculatorTests
{
    [TestClass]
    public class LinqPractiseTests
    {
        [TestMethod]
        public void AverageAmountTest()
        {
            var ob = new CheckBookVM();
            ob.Fill();
            var total = ob.Transactions.GroupBy(t => t.Tag).Select(g => new { g.Key, Sum = g.Average(t => t.Amount) });
            Assert.AreEqual(32.625, total.First().Sum);
            Assert.AreEqual(75, total.Last().Sum);
        }

        [TestMethod]
        public void PaidToEachPayee()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var payee1 = "Bracha";
            var payee = ob.Transactions.Where(t => t.Payee == payee1);
            var total = payee.Sum(t => t.Amount);
            Assert.AreEqual(131, total);

            var payee2 = "Moshe";
            payee = ob.Transactions.Where(t => t.Payee == payee2);
            total = payee.Sum(t => t.Amount);
            Assert.AreEqual(130, total);

            var payee3 = "Tim";
            payee = ob.Transactions.Where(t => t.Payee == payee3);
            total = payee.Sum(t => t.Amount);
            Assert.AreEqual(300, total);
        }

        [TestMethod]
        public void PaidToEachPayeeForFood()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var payee1 = "Bracha";
            var payee = ob.Transactions.Where(t => t.Payee == payee1).Where(t => t.Tag == "Food");
            var total = payee.Sum(t => t.Amount);
            Assert.AreEqual(131, total);

            var payee2 = "Moshe";
            payee = ob.Transactions.Where(t => t.Payee == payee2).Where(t => t.Tag == "Food");
            total = payee.Sum(t => t.Amount);
            Assert.AreEqual(130, total);

            var payee3 = "Tim";
            payee = ob.Transactions.Where(t => t.Payee == payee3).Where(t => t.Tag == "Food");
            total = payee.Sum(t => t.Amount);
            Assert.AreEqual(0, total);
        }

        [TestMethod]
        public void AccountForMostAutoExpenses()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var max = ob.Transactions.GroupBy(t => t.Account).Select(g => new { g.Key, Account = g.Select(t => t.Tag == "Auto") }).Max(t => t.Key);
            Assert.AreEqual("Credit", max);
        }

        [TestMethod]
        public void TransactionsBetween5and7th()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var dates = ob.Transactions.Where(t => t.Date.ToShortDateString() == "4/6/2015").Select(t => t.Date).ToArray();
            Assert.AreEqual("4/6/2015", dates[0].ToShortDateString());
            Assert.AreEqual("4/6/2015", dates[1].ToShortDateString());
            Assert.AreEqual("4/6/2015", dates[2].ToShortDateString());
        }

        [TestMethod]
        public void CountOfTransactionsBetween5and7th()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var dates = ob.Transactions.Where(t => t.Date.ToShortDateString() == "4/6/2015").Select(t => t.Date).ToArray().Length;
            Assert.AreEqual(3, dates);
        }

        [TestMethod]
        public void DatesOfTransactions()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var trans = ob.Transactions.Where(t => t.Account == "Checking").Select(t => t.Date).ToArray();
            Assert.AreEqual("4/7/2015", trans[0].ToShortDateString());
            Assert.AreEqual("4/5/2015", trans[1].ToShortDateString());
            Assert.AreEqual("4/6/2015", trans[2].ToShortDateString());
            Assert.AreEqual("4/3/2015", trans[3].ToShortDateString());
            Assert.AreEqual("4/2/2015", trans[4].ToShortDateString());
            Assert.AreEqual("4/6/2015", trans[5].ToShortDateString());

            trans = ob.Transactions.Where(t => t.Account == "Credit").Select(t => t.Date).ToArray();
            Assert.AreEqual("4/7/2015", trans[0].ToShortDateString());
            Assert.AreEqual("4/6/2015", trans[1].ToShortDateString());
            Assert.AreEqual("4/5/2015", trans[2].ToShortDateString());
            Assert.AreEqual("4/4/2015", trans[3].ToShortDateString());
            Assert.AreEqual("4/3/2015", trans[4].ToShortDateString());
            Assert.AreEqual("4/2/2015", trans[5].ToShortDateString());
        }
    }
}