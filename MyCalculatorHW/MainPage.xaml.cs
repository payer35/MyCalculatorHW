using System;
using Microsoft.Maui.Controls;

namespace MyCalculatorHW
{
    public partial class MainPage : ContentPage
    {
        private double currentValue = 0; // Mevcut hesaplama sonucu
        private double memoryValue = 0;  // Hafıza değeri
        public MainPage()
        {
            InitializeComponent();
        }

        double FirstNumber = 0;
        double SecondNumber = 0;
        Operator PreviousOperator = Operator.None;
        bool isNewEntry = true;

        private void MemoryStoreButton_Clicked(object sender, EventArgs e)
        {
            if (double.TryParse(Display.Text, out double memory))
            {
                memoryValue = memory;
                Display.Text = "Memory Saved";
            }
        }
        private void MemoryRecallButton_Clicked(object sender, EventArgs e)
        {
            Display.Text = memoryValue.ToString();
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            SetOperator(Operator.Add, "+");
        }

        private void SubtractButton_Clicked(object sender, EventArgs e)
        {
            SetOperator(Operator.Subtract, "-");
        }

        private void MultiplyButton_Clicked(object sender, EventArgs e)
        {
            SetOperator(Operator.Multiply, "×");
        }

        private void DivideButton_Clicked(object sender, EventArgs e)
        {
            SetOperator(Operator.Divide, "÷");
        }

        private void EqualButton_Clicked(object sender, EventArgs e)
        {
            string[] parts = Display.Text.Split(' ');
            if (parts.Length == 3 && double.TryParse(parts[2], out double secondNumber))
            {
                SecondNumber = secondNumber;
                DoCalculation();
                Display.Text = FirstNumber.ToString();
            }
            else
            {
                Display.Text = "Error";
            }

            PreviousOperator = Operator.None;
            isNewEntry = true;
        }
        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            FirstNumber = 0;
            SecondNumber = 0;
            PreviousOperator = Operator.None;
            Display.Text = string.Empty;
            isNewEntry = true;
        }

        private void NumberButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                // Yeni bir giriş yapılacaksa
                if (isNewEntry)
                {
                    // Eğer operatör seçildiyse boşluk ekle, aksi takdirde doğrudan yaz
                    if (!string.IsNullOrEmpty(Display.Text) && PreviousOperator != Operator.None)
                    {
                        Display.Text += $" {button.Text}";
                    }
                    else
                    {
                        Display.Text += button.Text;
                    }
                    isNewEntry = false;
                }
                else
                {
                    // Operatörden sonra ikinci sayıyı girerken
                    string[] parts = Display.Text.Split(' ');
                    if (parts.Length == 3)
                    {
                        parts[2] += button.Text;
                        Display.Text = string.Join(" ", parts);
                    }
                    else
                    {
                        Display.Text += button.Text;
                    }
                }
            }
        }

        private void CommaButton_Clicked(object sender, EventArgs e)
        {
            string[] parts = Display.Text.Split(' ');

            // İlk sayıya virgül eklenmesi
            if (parts.Length == 1 && !string.IsNullOrEmpty(parts[0]) && !parts[0].Contains(","))
            {
                Display.Text = $"{parts[0]},";  // İlk sayıya virgül ekleniyor
            }
            // İkinci sayıya virgül eklenmesi
            else if (parts.Length == 3 && !string.IsNullOrEmpty(parts[2]) && !parts[2].Contains(","))
            {
                parts[2] += ",";  // İkinci sayıya virgül ekleniyor
                Display.Text = string.Join(" ", parts);
            }
        }
        private void SetOperator(Operator op, string operatorSymbol)
        {
            string[] parts = Display.Text.Split(' ');

            if (parts.Length == 1 && double.TryParse(parts[0], out double number))
            {
                // İlk kez operatör atanıyorsa
                FirstNumber = number;
                Display.Text = $"{FirstNumber} {operatorSymbol}";
            }
            else if (parts.Length == 2)
            {
                // Önceki operatör değiştirilirse
                parts[1] = operatorSymbol;
                Display.Text = string.Join(" ", parts);
            }

            PreviousOperator = op; // Operatör güncellenir
            isNewEntry = true;
        }
        private void BackspaceButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Display.Text))
            {
                string[] parts = Display.Text.Split(' ');

                if (parts.Length == 3 && parts[2].Length > 0)
                {
                    // İkinci sayıyı düzenliyoruz
                    parts[2] = parts[2].Remove(parts[2].Length - 1);
                    Display.Text = string.Join(" ", parts);
                }
                else if (parts.Length == 2)
                {
                    // Operatörü kaldırıyoruz
                    Display.Text = parts[0];
                }
                else if (parts.Length == 1 && parts[0].Length > 0)
                {
                    // İlk sayıyı düzenliyoruz
                    parts[0] = parts[0].Remove(parts[0].Length - 1);
                    Display.Text = parts[0];
                }
            }
        }

        void DoCalculation()
        {
            double secondNumber;

            if (double.TryParse(Display.Text.Split(' ')[^1], out secondNumber))
            {
                SecondNumber = secondNumber;

                switch (PreviousOperator)
                {
                    case Operator.Add:
                        FirstNumber += SecondNumber;
                        break;
                    case Operator.Subtract:
                        FirstNumber -= SecondNumber;
                        break;
                    case Operator.Multiply:
                        FirstNumber *= SecondNumber;
                        break;
                    case Operator.Divide:
                        if (SecondNumber != 0)
                            FirstNumber /= SecondNumber;
                        else
                        {
                            Display.Text = "Error";
                            FirstNumber = 0;
                            PreviousOperator = Operator.None;
                            return;
                        }
                        break;
                    case Operator.None:
                        FirstNumber = SecondNumber;
                        break;
                }
            }
        }
    }
}

