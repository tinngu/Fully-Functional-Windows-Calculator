using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Written by Tin Nguyen

// I tried my very best to emulate every POSSIBLE function of the Windows calculator.
// Included is continuous calculation, by operations and also by pressing = continuously
// History box was introduced in Windows 10, just for fun and to help with debugging

namespace BasicCalc
{
    public partial class Form1 : Form
    {
        
        decimal mem = 0; // Holds memory for Memory functions
        decimal firstNum = 0; // Initally holds the first number inputted, but stores the RESULT value
        string op = ""; // Stores the expression to be used at the end from "=

        //Allows for continuos operations;  EXAMPLE: 5+5+5+5 = 20
        decimal temp = 0; // Holds the first value for possible continuous operations
        int opTotal = 0; // Stores the total number of operations currently being used
        
        //Allows for continous usage of the = button. EXAMPLE: 2*2=4=8=16=32=64
        string lastop = ""; // Allows for continuously pressing the = button

        // After pressing = , a new input should clear the current display
        bool resultFlag = false;

        // Prevents user from spamming Memory Recall button
        bool recallFlag = false;

        public Form1()
        {
            InitializeComponent();

            // Numbers 0-9
            n0.Click += new EventHandler(numberClick);
            n1.Click += new EventHandler(numberClick);
            n2.Click += new EventHandler(numberClick);
            n3.Click += new EventHandler(numberClick);
            n4.Click += new EventHandler(numberClick);
            n5.Click += new EventHandler(numberClick);
            n6.Click += new EventHandler(numberClick);
            n7.Click += new EventHandler(numberClick);
            n8.Click += new EventHandler(numberClick);
            n9.Click += new EventHandler(numberClick);

            // Math operations
            plusMinus.Click += new EventHandler(funClick);
            decim.Click += new EventHandler(funClick);
            divide.Click += new EventHandler(funClick);
            multiply.Click += new EventHandler(funClick);
            minus.Click += new EventHandler(funClick);
            add.Click += new EventHandler(funClick);
            equals.Click += new EventHandler(funClick);
            sqrt.Click += new EventHandler(funClick);
            percent.Click += new EventHandler(funClick);
            inverse.Click += new EventHandler(funClick);

            // Clearing and Delete
            del.Click += new EventHandler(funClick);
            clearEntry.Click += new EventHandler(funClick);
            clear.Click += new EventHandler(funClick);

            // Memory
            memClear.Click += new EventHandler(memClick);
            memRecall.Click += new EventHandler(memClick);
            memStore.Click += new EventHandler(memClick);
            memAdd.Click += new EventHandler(memClick);    
        }

        void numberClick(object sender, EventArgs e)
        {
            // Stores the button clicked for the number to be used
            Button button = (Button)sender;

            // Checks if the = button was previously used
            if (resultFlag)
            {
                resultFlag = false;
                input.Clear();
                input.Text += button.Text;
                output.Text += button.Text;
            }
            
            else
            {
                input.Text += button.Text;
                output.Text += button.Text;
            }    
        }

        void funClick(object sender, EventArgs e)
        {
            // Stores the button clicked for the expression or function to be used
            Button button = (Button)sender;
            if (button.Text == "+/-") // plusMinus: changes the sign the number currently in the textbox
            {
                try
                {
                    if (input.Text != "" && output.Text == "")
                    {
                        int itxtlength = input.Text.Length;
                        decimal currVal = decimal.Parse(input.Text);
                        currVal = -currVal;
                        input.Text = currVal.ToString("G15");
                        output.Text += input.Text;
                    }

                    if (input.Text != "" && output.Text != "")
                    {
                        int itxtlength = input.Text.Length;
                        int otxtlength = output.Text.Length;
                        output.Text = output.Text.Remove(otxtlength - itxtlength);

                        decimal currVal = decimal.Parse(input.Text);
                        currVal = -currVal;
                        input.Text = currVal.ToString("G15");
                        output.Text += input.Text;
                    }
                    else
                        MessageBox.Show("ERROR: No value to change the sign of.");
                }
                catch
                {
                    MessageBox.Show("ERROR: It is not possible to convert the sign of the current input.");
                }
                
            }

            else if (button.Text == ".") // decim: adds a decimal to the number also checks for exisiting
            {
                if (input.Text.Contains("."))
                {
                    input.Text = input.Text;
                    MessageBox.Show("ERROR: There is already a decimal included.");
                }
                else if (resultFlag)
                {
                    resultFlag = false;
                    input.Clear();
                    input.Text = input.Text + ".";
                    output.Text += button.Text;
                }
                else
                {
                    input.Text = input.Text + ".";
                    output.Text += button.Text;

                }
            }

            // Combined expression operations which REQUIRE two parts the firstNum && secondNum
            else if (button.Text == "*" || button.Text == "÷" || button.Text == "-" || button.Text == "+")
            {
                temp = firstNum;

                string check = output.Text; // Used to check if an operation already exists
                int otxtlength = output.Text.Length;
                int i = otxtlength - 1;
                try
                {
                    if (check[i] == '*' || check[i] == '÷' || check[i] == '-' || check[i] == '+')
                        MessageBox.Show("ERROR: There already exists an operation.");
                    else
                    {
                        opTotal++;
                        if (opTotal > 1) // Checks for continuous math operatiobns
                        {
                            firstNum = decimal.Parse(input.Text);
                            switch (op)
                            {
                                case "*":
                                    firstNum = temp * firstNum;
                                    break;
                                case "÷":
                                    firstNum = temp / firstNum;
                                    break;
                                case "-":
                                    firstNum = temp - firstNum;
                                    break;
                                case "+":
                                    firstNum = temp + firstNum;
                                    break;
                            }
                            // Testing purposes                             MessageBox.Show("New: " + firstNum + "\n opTotal: " + opTotal);
                        }

                        op = button.Text; // Stores the expression to be used after "="
                        if (input.Text != "")
                        {
                            if (opTotal > 1)
                            {
                                // No change, from the above calculation for continuous input
                            }

                            else
                                firstNum = decimal.Parse(input.Text);

                            input.Clear(); // Clears textbox for second number to be entered
                            output.Text += button.Text;
                        }
                        else output.Text += button.Text;
                    }
                    recallFlag = false; // Flag for memory recall

                }
                catch
                { // This catch occurs when there is no input, so it simply uses the previous result stored in firstNumber to compute with
                    op = button.Text; // Stores the expression to be used after "="
                    output.Text += firstNum.ToString("G15");
                    output.Text += button.Text;
                    input.Clear();
                    opTotal++;
                    recallFlag = false; // Flag for memory recall
                }

                
            }

            // Final output button is "=" evalutes the firstNum and secondNum using the expression stored
            else if (button.Text == "=")
            {
                if (input.Text == "")
                    MessageBox.Show("ERROR: No expression to evaluate.");

                else
                {
                    try
                    {
                        output.Text += button.Text;
                        decimal secondNum = decimal.Parse(input.Text);
                        if (op != "")
                        {
                            switch (op)
                            {
                                case "*":
                                    input.Text = (firstNum * secondNum).ToString("G15");
                                    lastop = "*";
                                    break;
                                case "÷":
                                    input.Text = (firstNum / secondNum).ToString("G15");
                                    lastop = "/";
                                    break;
                                case "-":
                                    input.Text = (firstNum - secondNum).ToString("G15");
                                    lastop = "-";
                                    break;
                                case "+":
                                    input.Text = (firstNum + secondNum).ToString("G15");
                                    lastop = "+";
                                    break;
                            }

                            temp = secondNum;
                            firstNum = decimal.Parse(input.Text); // Stores the result into firstNum
                            output.Text += input.Text; // Displays for the input to see

                            history.Items.Insert(0, output.Text); // Display output to histroy
                            output.Text = ""; // clears output after displaying it to history
                            recallFlag = false; // Flag for memory recall
                        }

                        // This allows for continually pressing the = button after an operation remember the last value and operation used
                        else // Example 2 + 2 = 4 = 6 = 8 = 10 = 12
                        {
                            if (lastop != "")
                            {
                                switch (lastop)
                                {
                                    case "*":
                                        input.Text = (firstNum * temp).ToString("G15");
                                        break;
                                    case "/":
                                        input.Text = (firstNum / temp).ToString("G15");
                                        break;
                                    case "-":
                                        input.Text = (firstNum - temp).ToString("G15");
                                        break;
                                    case "+":
                                        input.Text = (firstNum + temp).ToString("G15");
                                        break;
                                }
                                output.Text = firstNum + lastop + temp + "=" + input.Text; // Displays for the input to see
                                firstNum = decimal.Parse(input.Text); // Stores the result into firstNum


                                history.Items.Insert(0, output.Text); // Display output to histroy
                                output.Text = ""; // clears output after displaying it to history
                                recallFlag = false; // Flag for memory recall
                            }

                            else
                            {
                                firstNum = decimal.Parse(input.Text); // Stores the result into firstNum
                                history.Items.Insert(0, "=" + input.Text); // Display output to histroy
                                output.Text = ""; // clears output after displaying it to history
                                recallFlag = false; // Flag for memory recall
                            }
                            
                        }
                    }
                    catch
                    {
                        MessageBox.Show("ERROR: Mathematical Error.");
                        input.Text = "";
                        history.Items.Insert(0, "ERROR: " + output.Text); // Display output to histroy
                        output.Clear(); // Clears output
                    }



                    // Clears the operation for the next calculation
                    resultFlag = true; // Next input will allow the box to be cleared
                    op = ""; 
                    opTotal = 0;
                }
            }


            else if (button.Text == "√") // sqrt: Finds the square root of a SINGLE number
            {
                output.Text = "√" + input.Text; // Displays for the input to see
                try
                {
                    decimal currVal = decimal.Parse(input.Text);
                    currVal = (decimal)Math.Sqrt((double)currVal);
                    input.Text = currVal.ToString("G15");
                    output.Text += "=" + currVal.ToString("G15");

                    firstNum = decimal.Parse(input.Text);
                    history.Items.Insert(0, output.Text); // Display output to histroy
                    output.Text = ""; // clears output after displaying it to history
                    resultFlag = true; // Next input will allow the box to be cleared
                    recallFlag = false; // Flag for memory recall
                }
                catch
                {
                    MessageBox.Show("ERROR: Invalid Input.");
                    if (input.Text != "")
                    {
                        history.Items.Insert(0, "ERROR: " + output.Text); // Display output to histroy
                        output.Text = ""; // clears output after displaying it to history
                    }
                    input.Text = "";
                    output.Text = "";
                }
               
            }

            else if (button.Text == "%") // percent: I had no idea what this did until I googled it, even on Windows I didn't know
            {
                try
                {
                    if (op != "")
                    {
                        output.Text += button.Text;
                        decimal secondNum = decimal.Parse(input.Text);
                        switch (op)
                        {
                            case "*":
                                input.Text = (firstNum * (secondNum / 100 * firstNum)).ToString("G15");
                                break;
                            case "÷":
                                input.Text = (firstNum / (secondNum / 100 * firstNum)).ToString("G15");
                                break;
                            case "-":
                                input.Text = (firstNum - (secondNum / 100 * firstNum)).ToString("G15");
                                break;
                            case "+":
                                input.Text = (firstNum + (secondNum / 100 * firstNum)).ToString("G15");
                                break;
                        }

                        output.Text = firstNum + op + secondNum + "%" + "=" + firstNum + op + (secondNum/100*firstNum) + "=" + input.Text;
                        opTotal = 0;
                        op = "";
                        firstNum = decimal.Parse(input.Text);
                        history.Items.Insert(0, output.Text); // Display output to histroy
                        output.Text = ""; // clears output after displaying it to history
                        resultFlag = true; // Next input will allow the box to be cleared
                        recallFlag = false; // Flag for memory recall
                    }
                    else if (input.Text != "")
                    {
                        input.Text = "0";
                        output.Text = "";
                        firstNum = 0;
                        opTotal = 0;
                        resultFlag = true; // Next input will allow the box to be cleared
                        recallFlag = false; // Flag for memory recall
                    }

                    else MessageBox.Show("ERROR: There is no operation to use (%) percentage with.");

                }
                catch
                {
                    MessageBox.Show("ERROR: Mathematical Error.");
                }
            }

            else if (button.Text == "1/x") // inverse: flips the number or inverses it, also known as recipricoal
            {
                try
                {
                    decimal currVal = decimal.Parse(input.Text);
                    output.Text = "1/" + input.Text; // Displays for the input to see
                    currVal = 1 / currVal;
                    input.Text = currVal.ToString("G15");
                    output.Text += "=" + currVal.ToString("G15");

                    firstNum = decimal.Parse(input.Text);
                    history.Items.Insert(0, output.Text); // Display output to histroy
                    output.Text = ""; // clears output after displaying it to history
                    resultFlag = true; // Next input will allow the box to be cleared
                    recallFlag = false; // Flag for memory recall
                }
                catch
                {
                    MessageBox.Show("ERROR: Mathematical Error.");
                    input.Text = "";
                    if (output.Text != "")
                    {
                        history.Items.Insert(0, "ERROR: " + output.Text); // Display output to histroy
                        output.Text = ""; // clears output after displaying it to history
                    }
                }

            }


            else if (button.Text == "Delete") // del: deletes one number at the END
            {
                int itxtlength = input.Text.Length;
                int otxtlength = output.Text.Length;

                // Output has numbers, but the current display is empty, possible result after using the '=' button
                if (input.Text != string.Empty && output.Text == string.Empty)
                {
                    input.Text = input.Text.Remove(itxtlength - 1);
                }

                else if (input.Text != string.Empty && output.Text != string.Empty)
                {
                    // Simply deletes digits or .
                    input.Text = input.Text.Remove(itxtlength - 1);
                    output.Text = output.Text.Remove(otxtlength - 1);
                }

                else
                {
                   if (op == "")
                        MessageBox.Show("ERROR: The box is already empty, there is nothing to delete.");
                   else
                    {
                        string check = output.Text;
                        int i = otxtlength - 1;

                        // Used to check if an Operation (+,-,*,/) is at the END to reduce the operation total, and allow user to change operation
                        if (check[i] == '*' || check[i] == '÷' || check[i] == '-' || check[i] == '+')
                        {
                            output.Text = output.Text.Remove(otxtlength - 1);
                            opTotal--;
                            op = "";
                        }
                    }
                }
            }

            // clearEntry: ONLY clears the textbox 
            else if (button.Text == "CE")
            {
                input.Clear();
                opTotal = 0;
                recallFlag = false; // Flag for memory recall
            }

            // clear: Clears current numbers and operations
            else if (button.Text == "C")
            {
                firstNum = 0;
                op = "";
                opTotal = 0;
                input.Clear();
                output.Clear();
                lastop = "";
                recallFlag = false; // Flag for memory recall
            }
        }

        // Memory Functions
        void memClick(object sender, EventArgs e)
        {
            // Stores the memory button clicked
            Button button = (Button)sender;

            if (button.Text == "MC") // memClear: clears value stored in memory
            {
                mem = 0;
                memory.Text = "";
                history.Items.Insert(0, "Memory cleared"); // Display output to histroy
            }

            else if (button.Text == "MR") // memRecall: recalls stored memory to textbox
            {
                if (memory.Text == "M")
                {
                    if (!recallFlag)
                    {
                        if (input.Text != "" && output.Text != "")
                        {
                            int itxtlength = input.Text.Length;
                            int otxtlength = output.Text.Length;
                            output.Text = output.Text.Remove(otxtlength - itxtlength);
                            input.Text = mem.ToString("G15");
                            output.Text += input.Text;
                            recallFlag = true; // Lets user know that memory has already been recalled
                        }
                        else
                        {
                            input.Text = mem.ToString("G15");
                            output.Text += input.Text;
                            recallFlag = true; // Lets user know that memory has already been recalled
                        }
                    }
                    else MessageBox.Show("Memory has already been recalled.");
                    
                    resultFlag = true; // Next input will allow the box to be cleared
                }
                    
                else
                    MessageBox.Show("ERROR: There is no memory is stored to recall.");
            }

            else if (button.Text == "MS") // memStore: stores memory currently in textbox
            {
                if (input.Text != "")
                {
                    mem = decimal.Parse(input.Text);
                    input.Clear();
                    output.Clear();
                    opTotal = 0;
                    memory.Text = "M";
                    history.Items.Insert(0, "Stored in Memory: " + mem); // Display output to histroy
                    recallFlag = false; // Flag for memory recall
                }
                else MessageBox.Show("ERROR: Cannot store an empty value.");              
            }

            else if (button.Text == "M+") // memAdd: adds the current value in the textbox to memory stored
            {
                try
                {
                    if (memory.Text == "M")
                    {
                        mem = mem + decimal.Parse(input.Text);
                        history.Items.Insert(0, "Stored in Memory: " + mem); // Display output to histroy
                        recallFlag = false; // Flag for memory recall
                    }
                       
                    else MessageBox.Show("ERROR: Cannot add to an empty value.");
                }
                catch
                {
                    MessageBox.Show("ERROR: Cannot add an empty value to memory.");
                }
                
            }
        }

        private void clearHistory_Click(object sender, EventArgs e) // Clears pretty much everything, kind of like a memory wipe
        {
            firstNum = 0;
            op = "";
            opTotal = 0;
            history.Items.Clear();
            input.Clear();
            output.Clear();
            lastop = "";
            recallFlag = false; // Flag for memory recall
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
