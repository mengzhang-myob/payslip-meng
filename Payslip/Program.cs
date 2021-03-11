using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Payslip
{
    class Program
    {
        // string inputType;
        // string inputValue;
        static void Main(string[] args)
        {
            PayDetails payDetails = new PayDetails();
            UserInput("name", payDetails);
            UserInput("annualSalary", payDetails);
            UserInput("superRate", payDetails);
            UserInput("pDate", payDetails);
            UserInput("print", payDetails);
        }
        static void UserInput (string inputType, PayDetails payDetails){
            Regex regexSalary = new Regex(@"^-?[0-9][0-9\.]+$"); //Regex to tolerate decimal point
            // Regex regexSuper = new Regex("^([1-9]\d?|100)$");
            // Regex regexDate = new Regex(@"^\d$");
            if (inputType == "name"){
                try {
                    Console.WriteLine("Please input your name:");
                    string firstName = Console.ReadLine();
                    Console.WriteLine("Please input your surname:");
                    string surName = Console.ReadLine();
                    payDetails.SetName (firstName, surName); //Consider to decouple
                }
                catch(Exception e) {
                    Console.WriteLine("The name input is invalid" + "\n" + e.Message);
                }

            }
            else if (inputType == "annualSalary"){
                    Console.WriteLine("Please enter your annual salary:");
                    try {
                        string inputSalary = Console.ReadLine();
                        while (regexSalary.IsMatch(inputSalary) == false){
                            Console.WriteLine("Wrong format, the annual salary should be numbers only, please re-enter your annual salary:");
                            inputSalary = Console.ReadLine();
                        }
                        double inputValue = Convert.ToDouble(inputSalary);
                        payDetails.SetGrossIncome (inputValue);
                        payDetails.SetIncomeTax (inputValue);
                        payDetails.SetNetIncome ();
                    }
                    catch (Exception e) {
                        throw e;
                }
            }
            else if (inputType == "superRate"){
                Console.WriteLine("Please enter your super rate:");
                try {
                    string inputRate = Console.ReadLine();
                    while (regexSalary.IsMatch(inputRate) == false){
                        Console.WriteLine("Wrong format, the super rate should be numbers only, please re-enter your super rate:");
                        inputRate = Console.ReadLine();
                    }
                    double inputValue = Convert.ToDouble(inputRate);
                    payDetails.SetSuper(inputValue);
                }
                catch (Exception e) {
                        throw e;
                }
            }
            else if (inputType == "pDate"){
                Console.WriteLine("Please enter your payment start date:");
                string pStartDate = Console.ReadLine();
                Console.WriteLine("Please enter your payment end date:");
                string pEndDate = Console.ReadLine();
                payDetails.SetPayPeriod (pStartDate, pEndDate);
            }
            else if (inputType == "print") {
                payDetails.PrintPayDetails();
            }
        }
    }
    public class PayDetails {
        private string _name;
        private string _payPeriod;
        private double _grossIncome;
        private double _incomeTax;
        private double _netIncome;
        private double _super;
        private static List<double> taxRate = new List<double> { 0.19, 0.325, 0.37, 0.45 };
        private static List<int> taxThreshold = new List<int> { 18200, 37000, 87000, 180000 };
        private static List<int> taxBracket = new List<int> {0, 3572, 19822, 54232};
        public void SetName (string firstName, string surName) {
            this._name = firstName + " " + surName;
        }
        public void SetPayPeriod (string startDate, string endDate){
            this._payPeriod = startDate + " - " + endDate;
        }
        public void SetGrossIncome (double annualSalary) {
            this._grossIncome = (int)Math.Floor(annualSalary/12);
        }
        public void SetIncomeTax (double annualSalary) {
            if (annualSalary <= taxThreshold[0]) { // When income <= 18200
                this._incomeTax = taxBracket[0];
            }
            else if (annualSalary <= taxThreshold[1]) { // When 18201 < income <= 37000
                this._incomeTax = (int)Math.Ceiling(((annualSalary - taxThreshold[0]) * taxRate[0])/12);
            }
            else if (taxThreshold[1] < annualSalary && annualSalary <= taxThreshold[2]) { // When 37000 < income <= 87000
                this._incomeTax = (int)Math.Ceiling(((annualSalary - taxThreshold [1]) * taxRate[1] + taxBracket[1])/12);
            }
            else if (taxThreshold[2] < annualSalary && annualSalary <= taxThreshold[3]) { // When 87000 < income <= 180000
                this._incomeTax = (int)Math.Ceiling(((annualSalary - taxThreshold [2]) * taxRate[2] + taxBracket[1] + taxBracket[2])/12);
            }
            else if (taxThreshold[3] < annualSalary) { // When 180000 < income
                this._incomeTax = (int)Math.Ceiling(((annualSalary - taxThreshold [3]) * taxRate[3] + taxBracket[1] + taxBracket[2] + taxBracket[3])/12);
            }
        }
        public void SetNetIncome () {
            this._netIncome = _grossIncome - _incomeTax;
        }
        public void SetSuper (double superRate) {
            this._super = (int)Math.Floor(_grossIncome * (superRate/100));
        }
        public void PrintPayDetails () {
            Console.WriteLine(
                "Your payslip has been generated:\n" +
                "Name: " + _name + "\n" +
                "Pay Period: " + _payPeriod + "\n" +
                "Gross Income: " + _grossIncome + "\n" +
                "Income Tax: " + _incomeTax + "\n" +
                "Net Income: " + _netIncome + "\n" +
                "Super: " + _super  + "\n" + "\n" +
                "Thank you for using MYOB!"
                );
        }
    }
}
