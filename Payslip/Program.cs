using System;
using System.Collections.Generic;

namespace Payslip
{
    class Program
    {
        string inputType;
        string inputValue;
        static void Main(string[] args)
        {
            PayDetails payDetails = new PayDetails();
            userInput("name", payDetails);
            userInput("annualSalary", payDetails);
            userInput("superRate", payDetails);
            userInput("pDate", payDetails);
            userInput("print", payDetails);
        }
        static void userInput (string inputType, PayDetails payDetails){
            if (inputType == "name"){
                Console.WriteLine("Please input your name:");
                string firstName = Console.ReadLine();
                Console.WriteLine("Please input your surname:");
                string surName = Console.ReadLine();
                payDetails.setName (firstName, surName);
            }
            else if (inputType == "annualSalary"){
                try {
                    Console.WriteLine("Please enter your annual salary:");
                    double inputValue = Convert.ToDouble(Console.ReadLine());
                    payDetails.setGrossIncome (inputValue);
                    payDetails.setIncomeTax (inputValue);
                    payDetails.setNetIncome ();
                } catch (FormatException e) {
                    throw e;
                }
            }
            else if (inputType == "superRate"){
                Console.WriteLine("Please enter your super rate:");
                double inputValue = Convert.ToDouble(Console.ReadLine());
                payDetails.setSuper(inputValue);
            }
            else if (inputType == "pDate"){
                Console.WriteLine("Please enter your payment start date:");
                string pStartDate = Console.ReadLine();
                Console.WriteLine("Please enter your payment end date:");
                string pEndDate = Console.ReadLine();
                payDetails.setPayPeriod (pStartDate, pEndDate);
            }
            else if (inputType == "print") {
                payDetails.printPayDetails();
            }
        }
    }
    public class PayDetails {
        private string name;
        private string payPeriod;
        private double grossIncome;
        private double incomeTax;
        private double netIncome;
        private double super;
        public static List<double> taxRate = new List<double> { 0.19, 0.325, 0.37, 0.45 };
        private static List<int> taxThreshold = new List<int> { 18200, 37000, 87000, 180000 };
        public void setName (string name, string surName) {
            this.name = name + " " + surName;
        }
        public void setPayPeriod (string startDate, string endDate){
            this.payPeriod = startDate + " - " + endDate;
        }
        public void setGrossIncome (double annualSalary) {
            this.grossIncome = (int)Math.Floor(annualSalary/12);
        }
        public void setIncomeTax (double annualSalary) {
            if (annualSalary <= taxThreshold[0]) { // When income <= 18200
                this.incomeTax = 0;
            }
            else if (annualSalary <= taxThreshold[1]) { // When 18201 < income <= 37000
                this.incomeTax = (int)Math.Floor(((annualSalary - taxThreshold[0]) * taxRate[0])/12);
            }
            else if (taxThreshold[1] < annualSalary && annualSalary <= taxThreshold[2]) { // When 37000 < income <= 87000
                this.incomeTax = (int)Math.Floor(((annualSalary - taxThreshold [1]) * taxRate[1] + (taxThreshold [1] - taxThreshold[0]) * taxRate[0])/12);
            }
            else if (taxThreshold[2] < annualSalary && annualSalary <= taxThreshold[3]) { // When 87000 < income <= 180000
                this.incomeTax = (int)Math.Floor(((annualSalary - taxThreshold [2]) * taxRate[2] + (taxThreshold [2] - taxThreshold [1]) * taxRate[1] + (taxThreshold [1] - taxThreshold[0]) * taxRate[0])/12);
            }
            else if (taxThreshold[3] < annualSalary && annualSalary <= taxThreshold[4]) { // When 180000 < income
                this.incomeTax = (int)Math.Floor(((annualSalary - taxThreshold [3]) * taxRate[3] + (taxThreshold [3] - taxThreshold [2]) * taxRate[2] + (taxThreshold [2] - taxThreshold [1]) * taxRate[1] + (taxThreshold [1] - taxThreshold[0]) * taxRate[0])/12);
            }
        }
        public void setNetIncome () {
            this.netIncome = grossIncome - incomeTax;
        }
        public void setSuper (double superRate) {
            this.super = grossIncome * (superRate/100);
        }
        public void printPayDetails () {
            Console.WriteLine(
                "Your payslip has been generated:\n" +
                "Name: " + name + "\n" +
                "Pay Period: " + payPeriod + "\n" +
                "Gross Income: " + grossIncome + "\n" +
                "Income Tax: " + incomeTax + "\n" +
                "Net Income: " + netIncome + "\n" +
                "Super: " + super  + "\n" + "\n" +
                "Thank you for using MYOB!"
                );
        }
    }
}
